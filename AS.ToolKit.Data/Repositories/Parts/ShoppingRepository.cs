using System;
using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Entities;
using AS.ToolKit.Core.Repositories;
using AS.ToolKit.Data.Context;

namespace AS.ToolKit.Data.Repositories.Parts
{
    public class ShoppingIntervalRepository: IShoppingIntervalRepository
    {
        private readonly ToolKitDbContext _db;

        public ShoppingIntervalRepository(ToolKitDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public ShoppingInterval Get(int intervalId)
        {
            return _db.ShoppingIntervals.Find(intervalId);
        }

        public IEnumerable<ShoppingInterval> GetAllByUser(int userId)
        {
            return _db.ShoppingIntervals.Where(p => p.User.Id == userId).OrderBy(p => p.End);
        }

        public ShoppingInterval Create(int userId, DateTime start, DateTime end)
        {
            var user = _db.Users.Find(userId);
            var newInterval = new ShoppingInterval
            {
                End = end,
                Start = start,
                User = user
            };

            _db.ShoppingIntervals.Add(newInterval);
            _db.SaveChanges();

            return newInterval;
        }

        public void Update(int intervalId, DateTime start, DateTime end)
        {
            var interval = _db.ShoppingIntervals.Find(intervalId);
            interval.Start = start;
            interval.End = end;
            _db.SaveChanges();
        }

        public void Delete(int intervalId)
        {
            var interval = _db.ShoppingIntervals.Find(intervalId);
            _db.ShoppingIntervals.Remove(interval);
            _db.SaveChanges();
        }
    }

    public class ShoppingGroupRepository : IShoppingGroupRepository
    {
        private readonly ToolKitDbContext _db;

        public ShoppingGroupRepository(ToolKitDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public ShoppingGroup Get(int groupId)
        {
            return _db.ShoppingGroups.Find(groupId);
        }

        public IEnumerable<ShoppingGroup> GetAllByInterval(int intervalId)
        {
            return _db.ShoppingGroups.Where(g => g.ShoppingInterval.Id == intervalId);
        }

        public ShoppingGroup Create(int intervalId, string name)
        {
            var interval = _db.ShoppingIntervals.Find(intervalId);
            var newGroup = new ShoppingGroup
            {
                Name = name,
                ShoppingInterval = interval
            };

            _db.ShoppingGroups.Add(newGroup);
            _db.SaveChanges();

            return newGroup;
        }

        public void Update(int groupId, string name)
        {
            var group = _db.ShoppingGroups.Find(groupId);
            group.Name = name;
            _db.SaveChanges();
        }

        public void Delete(int groupId)
        {
            var group = _db.ShoppingGroups.Find(groupId);
            _db.ShoppingGroups.Remove(group);
            _db.SaveChanges();
        }
    }

    public class ShoppingContributionRepository : IShoppingContributionRepository
    {
        private readonly ToolKitDbContext _db;

        public ShoppingContributionRepository(ToolKitDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public ShoppingContribution Get(int contrId)
        {
            return _db.ShoppingContributions.Find(contrId);
        }

        public IEnumerable<ShoppingContribution> GetAllByGroup(int groupId)
        {
            return _db.ShoppingContributions.Where(c => c.ShoppingGroup.Id == groupId);
        }

        public IEnumerable<ShoppingContribution> GetAllByPerson(int personId)
        {
            return _db.ShoppingContributions.Where(c => c.ShoppingPerson.Id == personId);
        }

        public ShoppingContribution Create(int groupId, int personId, decimal amount)
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

        public void Update(int contrId, decimal amount)
        {
            var contr = _db.ShoppingContributions.Find(contrId);
            contr.Amount = amount;
            _db.SaveChanges();
        }

        public void Delete(int contrId)
        {
            var contr = _db.ShoppingContributions.Find(contrId);
            _db.ShoppingContributions.Remove(contr);
            _db.SaveChanges();
        }
    }

    public class ShoppingPersonRepository : IShoppingPersonRepository
    {
        private readonly ToolKitDbContext _db;

        public ShoppingPersonRepository(ToolKitDbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public ShoppingPerson Get(int personId)
        {
            return _db.ShoppingPersons.Find(personId);
        }

        public IEnumerable<ShoppingPerson> GetAllByUser(int userId)
        {
            return _db.ShoppingPersons.Where(p => p.User.Id == userId);
        }

        public IEnumerable<ShoppingPerson> GetAvailablesByGroup(int groupId, int userId)
        {
            return
                 _db.ShoppingPersons.Where(
                     p => p.ShoppingContributions.All(c => c.ShoppingGroup.Id != groupId) && p.User.Id == userId);
        }

        public ShoppingPerson Create(int userId, string firstName, string lastName)
        {
            var user = _db.Users.Find(userId);
            var newPerson = new ShoppingPerson
            {
                FirstName = firstName,
                LastName = lastName,
                User = user
            };

            _db.ShoppingPersons.Add(newPerson);
            _db.SaveChanges();

            return newPerson;
        }

        public void Update(int personId, string firstName, string lastName)
        {
            var person = _db.ShoppingPersons.Find(personId);
            person.FirstName = firstName;
            person.LastName = lastName;
            _db.SaveChanges();
        }

        public void Delete(int personId)
        {
            var person = _db.ShoppingPersons.Find(personId);
            _db.ShoppingPersons.Remove(person);
            _db.SaveChanges();
        }
    }

    public class ShoppingRepository : IShoppingRepository, IDisposable
    {
        private readonly ToolKitDbContext _db;

        public ShoppingRepository()
        {
            _db = new ToolKitDbContext();
            Intervals = new ShoppingIntervalRepository(_db);
            Groups = new ShoppingGroupRepository(_db);
            Contributions = new ShoppingContributionRepository(_db);
            People = new ShoppingPersonRepository(_db);

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

        public void Dispose()
        {
            _db.Dispose();
        }

        public IShoppingIntervalRepository Intervals { get; private set; }
        public IShoppingGroupRepository Groups { get; private set; }
        public IShoppingContributionRepository Contributions { get; private set; }
        public IShoppingPersonRepository People { get; private set; }
    }
}
