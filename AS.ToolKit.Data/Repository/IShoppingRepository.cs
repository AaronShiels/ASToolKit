using System;
using System.Collections.Generic;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Data.Repository
{
    public interface IShoppingRepository
    {
        ShoppingPeriod GetPeriod(int periodId);
        IEnumerable<ShoppingPeriod> GetPeriods(int userId);
        ShoppingPeriod CreatePeriod(int userId, DateTime start, DateTime end);
        void UpdatePeriod(int periodId, DateTime start, DateTime end);
        void DeletePeriod(int periodId);

        ShoppingGroup GetGroup(int groupId);
        IEnumerable<ShoppingGroup> GetGroups(int periodId);
        ShoppingGroup CreateGroup(int periodId, string name);
        void UpdateGroup(int groupId, string name);
        void DeleteGroup(int groupId);

        ShoppingContribution GetContribution(int contrId);
        IEnumerable<ShoppingContribution> GetContributionsByGroup(int groupId);
        IEnumerable<ShoppingContribution> GetContributionsByPerson(int personId);
        ShoppingContribution CreateContribution(int groupId, int personId, decimal amount);
        void UpdateContribution(int contrId, decimal amount);
        void DeleteContribution(int contrId);

        ShoppingPerson GetPerson(int personId);

        IEnumerable<ShoppingPerson> GetAvailablePeopleByGroup(int groupId, int userId);
    }
}
