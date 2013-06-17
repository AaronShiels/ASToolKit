using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ViewResult Periods()
        {
            var periods = _db.ShoppingPeriods.Where(p => p.UserId == 1);
            return View(periods);
        }

        public ViewResult Groups(int periodId)
        {
            var groups = _db.ShoppingGroups.Where(g => g.ShoppingPeriodId == periodId);
            return View(groups);
        }

        public ViewResult Contributions(int groupId)
        {
            var contributions = _db.ShoppingContributions.Where(c => c.GroupId == groupId);
            return View(contributions);
        }

    }
}
