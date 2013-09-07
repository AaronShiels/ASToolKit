using System.Collections.Generic;

namespace AS.ToolKit.Web.ViewModels
{
    public class ApplicationSelectionViewModel
    {
        public IEnumerable<ApplicationViewModels> Applications { get; set; }
        public int SelectedApplicationId { get; set; }
    }

    public class ApplicationViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Blurb { get; set; }
        public string UrlPath { get; set; }
        public string IconCss { get; set; }
    }
}