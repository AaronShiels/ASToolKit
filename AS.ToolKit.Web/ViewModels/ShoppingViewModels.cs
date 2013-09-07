using System;
using System.Collections.Generic;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Web.ViewModels
{
    public class SelectableItemViewModel
    {
        public string Heading { get; set; }
        public string Text { get; set; }
        public string Hyperlink { get; set; }
    }

    public class IndexViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<SelectableItemViewModel> Periods { get; set; }
        public string DefaultStartDateString { get; set; }
        public string DefaultEndDateString { get; set; }
        public AlertMessage Message { get; set; }
    }

    public class PeriodViewModel
    {
        public int PeriodId { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public IEnumerable<string> Contributors { get; set; }
        public string CombinedTotal { get; set; }
        public IEnumerable<SelectableItemViewModel> Groups { get; set; }
        public AlertMessage Message { get; set; }
    }

    public class GroupViewModel
    {
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectableItemViewModel> Contributions { get; set; }
        public IEnumerable<SelectableItemViewModel> Available { get; set; }
        public AlertMessage Message { get; set; }
    }

    public class AddPeriodViewModel
    {
        public int UserId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class AddGroupViewModel
    {
        public int PeriodId { get; set; }
        public string Name { get; set; }
    }

    public class AddContributionViewModel
    {
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public decimal Amount { get; set; }
    }

    public class EditContributionViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}