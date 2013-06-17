using System;
using System.Collections.Generic;

namespace AS.ToolKit.Web.Models
{
    public class ShoppingPeriod
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<ShoppingGroup> ShoppingGroups { get; set; }
    }
}