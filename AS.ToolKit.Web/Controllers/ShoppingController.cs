using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AS.ToolKit.Data.Repository;
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
            var periods = _repo.GetPeriods(_userId).ToList();
            var highestEndDate = periods.Any() ? periods.Max(p => p.End) : DateTime.Today.AddDays(-8);
            var model = new IndexViewModel
                {
                    UserId = _userId,
                    Periods = periods.Select(p => new SelectableItemViewModel
                        {
                            Heading = DateToName(p.End),
                            Text = p.Start.ToString("dd MMM") + " - " + p.End.ToString("dd MMM"),
                            Hyperlink = Url.Action("Period", "Shopping", new {periodId = p.Id})
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
        public RedirectToRouteResult AddPeriod(AddPeriodViewModel model)
        {
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(model.Start, out dateStart) && DateTime.TryParse(model.End, out dateEnd))
            {
                _repo.CreatePeriod(model.UserId, dateStart, dateEnd);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Shopping", new { error = string.Format("The dates you provided were invalid: '{0}' '{1}'", model.Start, model.End) });
            }
        }
        
        public ViewResult Period(int periodId, string error)
        {
            var period = _repo.GetPeriod(periodId);
            var model = new PeriodViewModel
                {
                    PeriodId = periodId,
                    Name = DateToName(period.End),
                    CombinedTotal = string.Format(_nfi, "{0:c}", period.ShoppingGroups.Sum(g => g.ShoppingContributions.Sum(c => c.Amount))),
                    Contributors = period.ShoppingGroups.SelectMany(g => g.ShoppingContributions.Select(c => string.Join(" ", c.ShoppingPerson.FirstName, c.ShoppingPerson.LastName))),
                    Groups = period.ShoppingGroups.Select(g => new SelectableItemViewModel
                        {
                            Heading = g.Name,
                            Text = string.Join(", ", g.ShoppingContributions.Select(c => c.ShoppingPerson.FirstName)),
                            Hyperlink = Url.Action("Group", "Shopping", new {groupId = g.Id})
                        }).ToList(),
                    Start = period.Start.ToString("dd-MMM-yyyy"),
                    End = period.End.ToString("dd-MMM-yyyy"),
                    Message = new AlertMessage("", "", AlertMessage.Type.Hidden)
                };

            if (!string.IsNullOrEmpty(error))
            {
                model.Message = new AlertMessage("Error!", error, AlertMessage.Type.Error);
            }

            return View(model);
        }
        
        [HttpPost]
        public RedirectToRouteResult Period(PeriodViewModel model)
        {
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(model.Start, out dateStart) && DateTime.TryParse(model.End, out dateEnd))
            {
                _repo.UpdatePeriod(model.PeriodId, dateStart, dateEnd);

                return RedirectToAction("Period", new { periodId = model.PeriodId });
            }
            else
            {
                return RedirectToAction("Period", "Shopping", new { periodId = model.PeriodId, error = string.Format("The dates you provided were invalid: '{0}' '{1}'", model.Start, model.End) });
            }
        }

        public RedirectToRouteResult DeletePeriod(int periodId)
        {
            _repo.DeletePeriod(periodId);

            return RedirectToAction("Index");
        }

        public RedirectToRouteResult AddGroup(AddGroupViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                _repo.CreateGroup(model.PeriodId, model.Name);

                return RedirectToAction("Period", new {periodId = model.PeriodId});
            }
            else
            {
                return RedirectToAction("Period", "Shopping", new { periodId = model.PeriodId, error = string.Format("The name you provided was empty.") });
            }
        }

        public ViewResult Group(int groupId, string error)
        {
            var group = _repo.GetGroup(groupId);
            var model = new GroupViewModel
            {
                PeriodId = group.ShoppingPeriod.Id,
                PeriodName = DateToName(group.ShoppingPeriod.End),
                GroupId = group.Id,
                Name = group.Name,
                Contributions = group.ShoppingContributions.Select(c => new SelectableItemViewModel
                    {
                        Heading = string.Format("{0} {1}", c.ShoppingPerson.FirstName, c.ShoppingPerson.LastName),
                        Text = string.Join(", ", c.Amount.ToString("c")),
                        Hyperlink = "#"
                    }).ToList(),
                Available = _repo.GetAvailablePeopleByGroup(groupId, _userId).Select(p => new SelectableItemViewModel
                    {
                        Heading = string.Format("{0} {1}", p.FirstName, p.LastName),
                        Hyperlink = "#"
                    }).ToList(),
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
                _repo.UpdateGroup(model.GroupId, model.Name);

                return RedirectToAction("Group", new {groupId = model.GroupId});
            }
            else
            {
                return RedirectToAction("Group", "Shopping", new { groupId = model.GroupId, error = string.Format("The name you provided was empty.") });
            }
        }

        public RedirectToRouteResult DeleteGroup(int groupId, int periodId)
        {
            _repo.DeleteGroup(groupId);

            return RedirectToAction("Period", "Shopping", new {periodId = periodId});
        }
        /*
        public RedirectToRouteResult AddContribution(int personId, int groupId)
        {
            _db.ShoppingContributions.Add(new ShoppingContribution(personId, groupId));
            _db.SaveChanges();

            return RedirectToAction("Group", "Shopping", new {groupId});
        }

        [HttpGet]
        public PartialViewResult EditContribution(int contributionId)
        {
            var contribution = _db.ShoppingContributions.SingleOrDefault(c => c.Id == contributionId);

            return PartialView("_EditContribution", contribution);
        }

        [HttpPost]
        public RedirectToRouteResult EditContribution(ShoppingContribution contribution)
        {
            _db.ShoppingContributions.Attach(contribution);
            _db.Entry(contribution).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Group", new { groupId = contribution.GroupId });
        }

        public RedirectToRouteResult DeleteContribution(int contributionId)
        {
            var contribution = _db.ShoppingContributions.SingleOrDefault(c => c.Id == contributionId);
            _db.ShoppingContributions.Remove(contribution);
            _db.SaveChanges();

            return RedirectToAction("Group", "Shopping", new { groupId = contribution.GroupId });
        }

        public ViewResult PrintPeriod(int periodId)
        {
            var period = _db.ShoppingPeriods.SingleOrDefault(p => p.Id == periodId);

            return View(period);
        }*/
    }
}
