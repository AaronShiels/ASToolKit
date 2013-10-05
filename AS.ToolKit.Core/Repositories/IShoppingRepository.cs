using System;
using System.Collections.Generic;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Core.Repositories
{
    public interface IShoppingRepository
    {
        IShoppingIntervalRepository Intervals { get; }
        IShoppingGroupRepository Groups { get; }
        IShoppingContributionRepository Contributions { get; }
        IShoppingPersonRepository People { get; }
    }

    public interface IShoppingIntervalRepository
    {
        ShoppingInterval Get(int intervalId);
        IEnumerable<ShoppingInterval> GetAllByUser(int userId);
        ShoppingInterval Create(int userId, DateTime start, DateTime end);
        void Update(int intervalId, DateTime start, DateTime end);
        void Delete(int intervalId);
    }

    public interface IShoppingGroupRepository
    {
        ShoppingGroup Get(int groupId);
        IEnumerable<ShoppingGroup> GetAllByInterval(int intervalId);
        ShoppingGroup Create(int intervalId, string name);
        void Update(int groupId, string name);
        void Delete(int groupId);
    }

    public interface IShoppingContributionRepository
    {
        ShoppingContribution Get(int contrId);
        IEnumerable<ShoppingContribution> GetAllByGroup(int groupId);
        IEnumerable<ShoppingContribution> GetAllByPerson(int personId);
        ShoppingContribution Create(int groupId, int personId, decimal amount);
        void Update(int contrId, decimal amount);
        void Delete(int contrId);
    }

    public interface IShoppingPersonRepository
    {
        ShoppingPerson Get(int personId);
        IEnumerable<ShoppingPerson> GetAllByUser(int userId);
        IEnumerable<ShoppingPerson> GetAvailablesByGroup(int groupId, int userId);
        ShoppingPerson Create(int userId, string firstName, string lastName);
        void Update(int personId, string firstName, string lastName);
        void Delete(int personId);
    }
}
