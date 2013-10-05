using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Core.Calculations
{
    public class EntityAggregation
    {
        public static Dictionary<ShoppingPerson, decimal> GetTotalStandings(ShoppingInterval interval)
        {
            var standingDict = new Dictionary<ShoppingPerson, decimal>();

            foreach (var group in interval.ShoppingGroups)
            {
                var groupAverage = GetSafeAverage(group.ShoppingContributions.Sum(ctr => ctr.Amount), group.ShoppingContributions.Count);

                foreach (var contr in group.ShoppingContributions)
                {
                    if (!standingDict.ContainsKey(contr.ShoppingPerson))
                    {
                        standingDict.Add(contr.ShoppingPerson, 0);
                    }
                    standingDict[contr.ShoppingPerson] += contr.Amount - groupAverage;
                }
            }

            return standingDict;
        }

        public static Dictionary<ShoppingPerson, decimal> GetTotalContributions(ShoppingInterval interval)
        {
            var contributionDict = new Dictionary<ShoppingPerson, decimal>();

            foreach (var contr in interval.ShoppingGroups.SelectMany(group => group.ShoppingContributions))
            {
                if (!contributionDict.ContainsKey(contr.ShoppingPerson))
                {
                    contributionDict.Add(contr.ShoppingPerson, 0);
                }
                contributionDict[contr.ShoppingPerson] += contr.Amount;
            }

            return contributionDict;
        }

        public static decimal GetSafeAverage(decimal total, int count)
        {
            if (count > 0)
            {
                return total / count;
            }
            else
            {
                return 0;
            }
        }
    }
}
