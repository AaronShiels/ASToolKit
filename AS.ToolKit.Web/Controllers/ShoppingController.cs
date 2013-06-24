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
            var highestEndDate = periods.Max(p => p.End);
            ViewBag.DefaultStartDate = highestEndDate.AddDays(1);
            ViewBag.DefaultEndDate = highestEndDate.AddDays(8);

            return View(periods);
        }

        [HttpPost]
        public RedirectToRouteResult AddPeriod(ShoppingPeriod period)
        {
            period.UserId = TempUser.CurrentUserId;
            _db.ShoppingPeriods.Add(period);
            _db.SaveChanges();

            _db.ShoppingGroups.Add(new ShoppingGroup
                {
                    PeriodId = period.Id
                });
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DeletePeriod(int periodId)
        {
            var period = _db.ShoppingPeriods.SingleOrDefault(p => p.Id == periodId);
            _db.ShoppingPeriods.Remove(period);
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

            return RedirectToAction("Period", new {periodId = period.Id});
        }

        public ViewResult Group(int groupId)
        {
            var group = _db.ShoppingGroups.SingleOrDefault(g => g.Id == groupId);

            return View(group);
        }

        public RedirectToRouteResult AddGroup(int periodId)
        {
            _db.ShoppingGroups.Add(new ShoppingGroup(periodId));
            _db.SaveChanges();

            return RedirectToAction("Period", new {periodId});
        }
    }
}
