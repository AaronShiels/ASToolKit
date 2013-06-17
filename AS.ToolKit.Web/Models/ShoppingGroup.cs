using System.Collections.Generic;

namespace AS.ToolKit.Web.Models
{
    public class ShoppingGroup
    {
        public int Id { get; set; }
        public int ShoppingPeriodId { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
    }
}