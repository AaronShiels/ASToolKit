using System.Collections.Generic;

namespace AS.ToolKit.Web.Models
{
    public class ShoppingGroup
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
        public virtual ShoppingPeriod ShoppingPeriod { get; set; }

        public ShoppingGroup()
        {
            
        }

        public ShoppingGroup(int periodId)
        {
            PeriodId = periodId;
        }
    }
}