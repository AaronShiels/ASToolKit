using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AS.ToolKit.Core.Repositories;
using AS.ToolKit.Web.ViewModels;

namespace AS.ToolKit.Web.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IShoppingRepository _repo;
        private readonly int _userId;
        private readonly NumberFormatInfo _nfi;

        public ShoppingController(IShoppingRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");
            _repo = repo;
            _userId = 1;
            _nfi = new CultureInfo(CultureInfo.CurrentCulture.ToString()).NumberFormat;
            _nfi.CurrencyNegativePattern = 1;
        }

        public ViewResult Index(string error)
        {
            var intervals = _repo.Intervals.GetAllByUser(_userId).ToList();
            var people = _repo.People.GetAllByUser(_userId).ToList();
            var highestEndDate = intervals.Any() ? intervals.Max(p => p.End) : DateTime.Today.AddDays(-8);
            var model = new IndexViewModel
                {
                    UserId = _userId,
                    Intervals = intervals.Select(i => new SelectableItemViewModel
                        {
                            Heading = DateToName(i.End),
                            Text = i.Start.ToString("dd MMM") + " - " + i.End.ToString("dd MMM"),
                            Hyperlink = Url.Action("Interval", "Shopping", new {intervalId = i.Id}),
                            IconClass = "icon-calendar"
                        }).ToList(),
                    People = people.Select(p => new SelectableItemViewModel
                        {
                            Heading = p.FirstName + " " + p.LastName,
                            Text = string.Format("{0} total contributions made", p.ShoppingContributions.Count()),
                            Hyperlink = "#", //TODO: Url.Action("Person", "Shopping", new {personId = p.Id})
                            IconClass = "icon-user"
                        }).ToList(),
                    DefaultStartDateString = highestEndDate.AddDays(1).ToString("dd-MMM-yyyy"),
                    DefaultEndDateString = DateTime.Today.ToString("dd-MMM-yyyy"),
                    Message = new AlertMessage("", "", AlertMessage.Type.Hidden)
                };

            if (!string.IsNullOrEmpty(error))
            {
                model.Message = new AlertMessage("Error!", error, AlertMessage.Type.Error);
            }

            return View(model);
        }

        private static string DateToName(DateTime date)
        {
            return date.ToString("MMMM yyyy");
        }

        
        [HttpPost]
        public RedirectToRouteResult AddInterval(AddIntervalViewModel model)
        {
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(model.Start, out dateStart) && DateTime.TryParse(model.End, out dateEnd))
            {
                _repo.Intervals.Create(model.UserId, dateStart, dateEnd);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Shopping", new { error = string.Format("The dates you provided were invalid: '{0}' '{1}'", model.Start, model.End) });
            }
        }

        [HttpPost]
        public RedirectToRouteResult AddPerson(AddPersonViewModel model)
        {
            if (!string.IsNullOrEmpty(model.FirstName) && !string.IsNullOrEmpty(model.LastName))
            {
                _repo.People.Create(model.UserId, model.FirstName, model.LastName);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Shopping", new { error = string.Format("The name you provided was incomplete: '{0}' '{1}'", model.FirstName, model.LastName) });
            }
        }
        
        public ViewResult Interval(int intervalId, string error)
        {
            var interval = _repo.Intervals.Get(intervalId);
            var model = new IntervalViewModel
                {
                    IntervalId = interval.Id,
                    Name = DateToName(interval.End),
                    CombinedTotal = string.Format(_nfi, "{0:c}", interval.ShoppingGroups.Sum(g => g.ShoppingContributions.Sum(c => c.Amount))),
                    Contributors = interval.ShoppingGroups.SelectMany(g => g.ShoppingContributions.Select(c => string.Join(" ", c.ShoppingPerson.FirstName, c.ShoppingPerson.LastName))).Distinct(),
                    Groups = interval.ShoppingGroups.Select(g => new SelectableItemViewModel
                        {
                            Heading = g.Name,
                            Text = string.Join(", ", g.ShoppingContributions.Select(c => c.ShoppingPerson.FirstName)),
                            Hyperlink = Url.Action("Group", "Shopping", new {groupId = g.Id}),
                            IconClass = "icon-folder-open"
                        }).ToList(),
                    Start = interval.Start.ToString("dd-MMM-yyyy"),
                    End = interval.End.ToString("dd-MMM-yyyy"),
                    Message = new AlertMessage("", "", AlertMessage.Type.Hidden)
                };

            if (!string.IsNullOrEmpty(error))
            {
                model.Message = new AlertMessage("Error!", error, AlertMessage.Type.Error);
            }

            return View(model);
        }
        
        [HttpPost]
        public RedirectToRouteResult Interval(IntervalViewModel model)
        {
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(model.Start, out dateStart) && DateTime.TryParse(model.End, out dateEnd))
            {
                _repo.Intervals.Update(model.IntervalId, dateStart, dateEnd);

                return RedirectToAction("Interval", new { intervalId = model.IntervalId });
            }
            else
            {
                return RedirectToAction("Interval", "Shopping", new { intervalId = model.IntervalId, error = string.Format("The dates you provided were invalid: '{0}' '{1}'", model.Start, model.End) });
            }
        }

        public RedirectToRouteResult DeleteInterval(int intervalId)
        {
            _repo.Intervals.Delete(intervalId);

            return RedirectToAction("Index");
        }

        public RedirectToRouteResult AddGroup(AddGroupViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                _repo.Groups.Create(model.IntervalId, model.Name);

                return RedirectToAction("Interval", new { intervalId = model.IntervalId });
            }
            else
            {
                return RedirectToAction("Interval", "Shopping", new { intervalId = model.IntervalId, error = string.Format("The name you provided was empty.") });
            }
        }

        public ViewResult Group(int groupId, string error)
        {
            var group = _repo.Groups.Get(groupId);
            var availablePeople = _repo.People.GetAvailablesByGroup(groupId, _userId).ToList();

            var model = new GroupViewModel
            {
                IntervalId = group.ShoppingInterval.Id,
                IntervalName = DateToName(group.ShoppingInterval.End),
                GroupId = group.Id,
                Name = group.Name,
                Total = string.Format(_nfi, "{0:c}", group.ShoppingContributions.Sum(c => c.Amount)),
                Contributors = group.ShoppingContributions.Select(c => string.Join(" ", c.ShoppingPerson.FirstName, c.ShoppingPerson.LastName)),
                Contributions = group.ShoppingContributions.Select(c => new SelectableItemViewModel
                {
                    Heading = string.Format("{0} {1}", c.ShoppingPerson.FirstName, c.ShoppingPerson.LastName),
                    Text = string.Format(_nfi, "{0:c}", c.Amount),
                    DataVal = Url.Action("EditContribution", "Shopping", new { contrId = c.Id }),
                    IconClass = "icon-user"
                }),
                Available = availablePeople.Select(p => new SelectableItemViewModel
                {
                    Heading = string.Format("{0} {1}", p.FirstName, p.LastName),
                    Text = "Available",
                    DataVal = Url.Action("AddContribution", "Shopping", new { groupId = group.Id, personId = p.Id }),
                    IconClass = "icon-user"
                }),
                Message = new AlertMessage("", "", AlertMessage.Type.Hidden)
            };

            if (!string.IsNullOrEmpty(error))
            {
                model.Message = new AlertMessage("Error!", error, AlertMessage.Type.Error);
            }

            return View(model);
        }

        [HttpPost]
        public RedirectToRouteResult Group(GroupViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                _repo.Groups.Update(model.GroupId, model.Name);

                return RedirectToAction("Group", new {groupId = model.GroupId});
            }
            else
            {
                return RedirectToAction("Group", "Shopping", new { groupId = model.GroupId, error = string.Format("The name you provided was empty.") });
            }
        }

        public RedirectToRouteResult DeleteGroup(int groupId, int intervalId)
        {
            _repo.Groups.Delete(groupId);

            return RedirectToAction("Interval", "Shopping", new { intervalId = intervalId });
        }

        [HttpGet]
        public PartialViewResult AddContribution(int groupId, int personId)
        {
            var person = _repo.People.Get(personId);

            var model = new AddContributionViewModel
                {
                    GroupId = groupId,
                    PersonId = personId,
                    PersonName = string.Format("{0} {1}", person.FirstName, person.LastName),
                    Amount = "0.00"
                };

            return PartialView("_AddContribution", model);
        }

        [HttpPost]
        public RedirectToRouteResult AddContribution(AddContributionViewModel model)
        {
            decimal amount;

            if (Decimal.TryParse(model.Amount, out amount))
            {
                _repo.Contributions.Create(model.GroupId, model.PersonId, amount);

                return RedirectToAction("Group", new { groupId = model.GroupId });
            }
            else
            {
                return RedirectToAction("Group", "Shopping", new { groupId = model.GroupId, error = string.Format("The amount you entered was invalid.") });
            }
        }

        [HttpGet]
        public PartialViewResult EditContribution(int contrId)
        {
            var contr = _repo.Contributions.Get(contrId);

            var model = new EditContributionViewModel
            {
                ContrId = contrId,
                GroupId = contr.ShoppingGroup.Id,
                PersonName = string.Format("{0} {1}", contr.ShoppingPerson.FirstName, contr.ShoppingPerson.LastName),
                Amount = contr.Amount.ToString()
            };

            return PartialView("_EditContribution", model);
        }

        [HttpPost]
        public RedirectToRouteResult EditContribution(EditContributionViewModel model)
        {
            decimal amount;

            if (Decimal.TryParse(model.Amount, out amount))
            {
                _repo.Contributions.Update(model.ContrId, amount);

                return RedirectToAction("Group", new { groupId = model.ContrId });
            }
            else
            {
                return RedirectToAction("Group", "Shopping", new { groupId = model.GroupId, error = string.Format("The amount you entered was invalid.") });
            }
        }

        public RedirectToRouteResult DeleteContribution(int contrId, int groupId)
        {
            _repo.Contributions.Delete(contrId);

            return RedirectToAction("Group", "Shopping", new {groupId = groupId});
        }
        /*        
        public ViewResult PrintPeriod(int periodId)
        {
            var period = _db.ShoppingPeriods.SingleOrDefault(p => p.Id == periodId);

            return View(period);
        }*/

        [HttpGet]
        public ViewResult Person(int personid)
        {
            throw new NotImplementedException();
        }
    }
}
