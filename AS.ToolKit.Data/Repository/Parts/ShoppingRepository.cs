using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Data.Repository.Parts
{
    public class ShoppingRepository : IShoppingRepository
    {
        private readonly ToolKitDb _db;

        public ShoppingRepository(ToolKitDb db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;

            CheckGuestAccount();
        }

        private void CheckGuestAccount()
        {
            if (_db.Users.Find(1) != null) return;

            _db.Users.Add(new User
                {
                    Id = 1,
                    Username = "Guest",
                    Password = "password.123"
                });
            _db.SaveChanges();
        }

        public ShoppingPeriod GetPeriod(int periodId)
        {
            return _db.ShoppingPeriods.Find(periodId);
        }

        public IEnumerable<ShoppingPeriod> GetPeriods(int userId)
        {
            return _db.ShoppingPeriods.Where(p => p.User.Id == userId).OrderBy(p => p.End);
        }

        public ShoppingPeriod CreatePeriod(int userId, DateTime start, DateTime end)
        {
            var user = _db.Users.Find(userId);
            var newPeriod = new ShoppingPeriod
                {
                    End = end,
                    Start = start,
                    User = user
                };

            _db.ShoppingPeriods.Add(newPeriod);
            _db.SaveChanges();

            return newPeriod;
        }

        public void UpdatePeriod(int periodId, DateTime start, DateTime end)
        {
            var period = _db.ShoppingPeriods.Find(periodId);
            period.Start = start;
            period.End = end;
            _db.SaveChanges();
        }

        public void DeletePeriod(int periodId)
        {
            var period = _db.ShoppingPeriods.Find(periodId);
            _db.ShoppingPeriods.Remove(period);
            _db.SaveChanges();
        }

        public ShoppingGroup GetGroup(int groupId)
        {
            return _db.ShoppingGroups.Find(groupId);
        }

        public IEnumerable<ShoppingGroup> GetGroups(int periodId)
        {
            return _db.ShoppingGroups.Where(g => g.ShoppingPeriod.Id == periodId);
        }

        public ShoppingGroup CreateGroup(int periodId, string name)
        {
            var period = _db.ShoppingPeriods.Find(periodId);
            var newGroup = new ShoppingGroup
            {
                Name = name,
                ShoppingPeriod = period
            };

            _db.ShoppingGroups.Add(newGroup);
            _db.SaveChanges();

            return newGroup;
        }

        public void UpdateGroup(int groupId, string name)
        {
            var group = _db.ShoppingGroups.Find(groupId);
            group.Name = name;
            _db.SaveChanges();
        }

        public void DeleteGroup(int groupId)
        {
            var group = _db.ShoppingGroups.Find(groupId);
            _db.ShoppingGroups.Remove(group);
            _db.SaveChanges();
        }

        public ShoppingContribution GetContribution(int contrId)
        {
            return _db.ShoppingContributions.Find(contrId);
        }

        public IEnumerable<ShoppingContribution> GetContributionsByGroup(int groupId)
        {
            return _db.ShoppingContributions.Where(c => c.ShoppingGroup.Id == groupId);
        }

        public IEnumerable<ShoppingContribution> GetContributionsByPerson(int personId)
        {
            return _db.ShoppingContributions.Where(c => c.ShoppingPerson.Id == personId);
        }

        public ShoppingContribution CreateContribution(int groupId, int personId, decimal amount)
        {
            var group = _db.ShoppingGroups.Find(groupId);
            var person = _db.ShoppingPersons.Find(personId);

            var newContribution = new ShoppingContribution
            {
                Amount = amount,
                ShoppingGroup = group,
                ShoppingPerson = person
            };

            _db.ShoppingContributions.Add(newContribution);
            _db.SaveChanges();

            return newContribution;
        }

        public void UpdateContribution(int contrId, decimal amount)
        {
            var contr = _db.ShoppingContributions.Find(contrId);
            contr.Amount = amount;
            _db.SaveChanges();
        }

        public void DeleteContribution(int contrId)
        {
            var contr = _db.ShoppingContributions.Find(contrId);
            _db.ShoppingContributions.Remove(contr);
            _db.SaveChanges();
        }

        public ShoppingPerson GetPerson(int personId)
        {
            return _db.ShoppingPersons.Find(personId);
        }

        public IEnumerable<ShoppingPerson> GetAvailablePeopleByGroup(int groupId, int userId)
        {
            return
                _db.ShoppingPersons.Where(
                    p => p.ShoppingContributions.All(c => c.ShoppingGroup.Id != groupId) && p.User.Id == userId);

            /*var peopleIdList = _db.ShoppingPersons.Where(p => p.User.Id == userId).Select(p => p.Id);
            var group = _db.ShoppingGroups.Find(groupId);

            return people.Where(person => !group.ShoppingContributions.Select(c => c.ShoppingPerson.Id).Contains(person));*/
        }
    }
}
