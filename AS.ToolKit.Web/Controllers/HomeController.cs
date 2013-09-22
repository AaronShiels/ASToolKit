using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AS.ToolKit.Core.Repositories;
using AS.ToolKit.Web.ViewModels;

namespace AS.ToolKit.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShoppingRepository _repo;

        public HomeController(IShoppingRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");
            _repo = repo;
        }

        public ActionResult Index(int? appId)
        {
            var model = new ApplicationSelectionViewModel
                {
                    Applications = GetApplications(),
                    SelectedApplicationId = appId ?? 1
                };

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        private static IEnumerable<ApplicationViewModels> GetApplications()
        {
            return new List<ApplicationViewModels>
                {
                    new ApplicationViewModels
                        {
                            Id = 1,
                            Name = "Shopping Calculator",
                            Description = "Calculates debts and repayment plans in group contribution situations",
                            Blurb = "<p>Have you ever entered into complex group shopping agreements? Have you found it difficult to work out who has to pay who to ensure everyone contributed the same amount?</p><p>Shopping Calculator can handle endless individual contributions over any given time interval, and can also roll-up simultaneous contribution groups into a single aggregate result.</p><p>This result is transformed into a payment plan using an efficiency algorithm to ensure minimal transactions need occur to reach evenness among the group/s.</p>",
                            UrlPath="/Shopping",
                            IconCss = "icon-shopping-cart"
                        },
                        new ApplicationViewModels
                        {
                            Id = 2,
                            Name = "Next Project",
                            Description = "Unknown as to what this will entail",
                            Blurb = "aaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaa",
                            UrlPath="/Fake",
                            IconCss = "icon-question"
                        }
                };
        } 
    }
}
