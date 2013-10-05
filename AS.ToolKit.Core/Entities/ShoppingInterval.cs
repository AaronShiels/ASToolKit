using System;
using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Calculations;

namespace AS.ToolKit.Core.Entities
{
    public class ShoppingInterval
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public virtual ICollection<ShoppingGroup> ShoppingGroups { get; set; }
        public virtual User User { get; set; }
    }
}