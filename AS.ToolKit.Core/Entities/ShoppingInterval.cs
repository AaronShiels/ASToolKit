﻿using System;
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

        public Dictionary<ShoppingPerson, decimal> GetTotalContributions()
        {
            var contributionDict = new Dictionary<ShoppingPerson, decimal>();

            foreach (var contr in ShoppingGroups.SelectMany(@group => @group.ShoppingContributions))
            {
                if (!contributionDict.ContainsKey(contr.ShoppingPerson))
                {
                    contributionDict.Add(contr.ShoppingPerson, 0);
                }
                contributionDict[contr.ShoppingPerson] += contr.Amount;
            }

            return contributionDict;
        }

        public Dictionary<ShoppingPerson, decimal> GetTotalStanding()
        {
            var standingDict = new Dictionary<ShoppingPerson, decimal>();

            foreach (var group in ShoppingGroups)
            {
                var groupStanding = group.GetGroupStanding();

                foreach (var key in groupStanding.Keys)
                {
                    if (!standingDict.ContainsKey(key))
                    {
                        standingDict.Add(key, 0);
                    }
                    standingDict[key] += groupStanding[key];
                }
            }

            return standingDict;
        }

        public IEnumerable<OweVector> GetRepaymentList(Dictionary<ShoppingPerson, decimal> totalStandings)
        {
            var graph = new OweGraph(totalStandings);
            graph.Simplify();

            return graph.GetVectors();
        } 
    }
}