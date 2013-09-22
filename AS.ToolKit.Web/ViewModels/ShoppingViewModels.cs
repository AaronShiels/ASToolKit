﻿using System;
using System.Collections.Generic;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Web.ViewModels
{
    public class SelectableItemViewModel
    {
        public string Heading { get; set; }
        public string Text { get; set; }
        public string Hyperlink { get; set; }
        public string DataVal { get; set; }
        public string IconClass { get; set; }
    }

    public class IndexViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<SelectableItemViewModel> Intervals { get; set; }
        public string DefaultStartDateString { get; set; }
        public string DefaultEndDateString { get; set; }
        public AlertMessage Message { get; set; }
    }

    public class IntervalViewModel
    {
        public int IntervalId { get; set; }
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
        public int IntervalId { get; set; }
        public string IntervalName { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectableItemViewModel> Contributions { get; set; }
        public IEnumerable<SelectableItemViewModel> Available { get; set; }
        public AlertMessage Message { get; set; }
    }

    public class AddIntervalViewModel
    {
        public int UserId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class AddGroupViewModel
    {
        public int IntervalId { get; set; }
        public string Name { get; set; }
    }

    public class AddContributionViewModel
    {
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string Amount { get; set; }
    }

    public class EditContributionViewModel
    {
        public int ContrId { get; set; }
        public int GroupId { get; set; }
        public string PersonName { get; set; }
        public string Amount { get; set; }
    }
}