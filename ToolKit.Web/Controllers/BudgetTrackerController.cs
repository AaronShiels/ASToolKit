using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToolKit.Web.Controllers
{
    public class BudgetTrackerController : Controller
    {
        // GET: BudgetTracker
        public ViewResult Index()
        {
            return View();
        }
    }
}