using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AS.ToolKit.Web.Infrastructure.Security;
using AS.ToolKit.Web.Models;
using AS.ToolKit.Web.Parts;

namespace AS.ToolKit.Web.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ToolKitDb _db;

        public ShoppingController(ToolKitDb db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public ViewResult Index()
        {
            var periods = _db.ShoppingPeriods.Where(p => p.UserId == TempUser.CurrentUserId);
            var highestEndDate = DateTime.Today.AddDays(-8);
            if (_db.ShoppingPeriods.Any())
            {
                highestEndDate = periods.Max(p => p.End);
            }
            ViewBag.DefaultStartDate = highestEndDate.AddDays(1);
            ViewBag.DefaultEndDate = DateTime.Today;

            return View(periods);
        }

        [HttpPost]
        public RedirectToRouteResult AddPeriod(ShoppingPeriod period)
        {
            period.UserId = TempUser.CurrentUserId;
            _db.ShoppingPeriods.Add(period);
            _db.SaveChanges();

            _db.ShoppingGroups.Add(new ShoppingGroup(period.Id));
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Period(int periodId)
        {
            var period = _db.ShoppingPeriods.SingleOrDefault(p => p.Id == periodId);

            return View(period);
        }

        [HttpPost]
        public RedirectToRouteResult Period(ShoppingPeriod period)
        {
            _db.ShoppingPeriods.Attach(period);
            _db.Entry(period).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Period", new { periodId = period.Id });
        }

        public RedirectToRouteResult DeletePeriod(int periodId)
        {
            var period = _db.ShoppingPeriods.SingleOrDefault(p => p.Id == periodId);
            _db.ShoppingPeriods.Remove(period);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public RedirectToRouteResult AddGroup(int periodId)
        {
            _db.ShoppingGroups.Add(new ShoppingGroup(periodId));
            _db.SaveChanges();

            return RedirectToAction("Period", new {periodId});
        }

        public ViewResult Group(int groupId)
        {
            var group = _db.ShoppingGroups.SingleOrDefault(g => g.Id == groupId);

            ViewBag.AvailablePeople =
                _db.ShoppingPersons.Where(
                    p => p.UserId == TempUser.CurrentUserId && p.ShoppingContributions.All(c => c.GroupId != groupId));

            return View(group);
        }

        public RedirectToRouteResult DeleteGroup(int groupId)
        {
            var group = _db.ShoppingGroups.SingleOrDefault(g => g.Id == groupId);
            _db.ShoppingGroups.Remove(group);
            _db.SaveChanges();

            return RedirectToAction("Period", "Shopping", new {periodId = group.PeriodId});
        }

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
    }
}
