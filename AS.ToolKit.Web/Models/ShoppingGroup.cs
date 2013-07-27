using System.Collections.Generic;
using System.Linq;

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

        public decimal GetTotalSpending()
        {
            return ShoppingContributions.Sum(x => x.Amount);
        }

        public Dictionary<ShoppingPerson, decimal> GetGroupStanding()
        {
            var requiredAverage = GetTotalSpending()/ShoppingContributions.Count;

            var standingDict = new Dictionary<ShoppingPerson, decimal>();

            foreach (var contr in ShoppingContributions)
            {
                if (!standingDict.ContainsKey(contr.ShoppingPerson))
                {
                    standingDict.Add(contr.ShoppingPerson, 0);
                }
                standingDict[contr.ShoppingPerson] = contr.Amount - requiredAverage;
            }

            return standingDict;
        }
    }
}