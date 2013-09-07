using System.Collections.Generic;
using System.Linq;

namespace AS.ToolKit.Core.Entities
{
    public class ShoppingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
        public virtual ShoppingPeriod ShoppingPeriod { get; set; }

        public decimal GetAverageContribution()
        {
            if (ShoppingContributions.Count > 0)
            {
                return ShoppingContributions.Sum(x => x.Amount)/ShoppingContributions.Count;
            }
            else
            {
                return 0;
            }
        }

        public Dictionary<ShoppingPerson, decimal> GetGroupStanding()
        {
            var standingDict = new Dictionary<ShoppingPerson, decimal>();

            var requiredAverage = GetAverageContribution();
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