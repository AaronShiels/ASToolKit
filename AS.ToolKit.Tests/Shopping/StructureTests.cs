using System;
using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Entities;
using NUnit.Framework;

namespace AS.ToolKit.Tests.Shopping
{
    [TestFixture]
    public class StructureTests
    {
        private ShoppingPeriod _period;
        private ShoppingGroup _groupA;
        private ShoppingGroup _groupB;
        private ShoppingPerson _pAaron;
        private ShoppingPerson _pBill;
        private ShoppingPerson _pBen;
        private ShoppingPerson _pThom;

        [SetUp]
        public void SetUp()
        {
            //People
            _pAaron = new ShoppingPerson
                {
                    Id = 1,
                    FirstName = "Aaron",
                    LastName = "Shiels",
                    ShoppingContributions = new List<ShoppingContribution>()
                };

            _pBill = new ShoppingPerson
                {
                    Id = 2,
                    FirstName = "Bill",
                    LastName = "Farrell",
                    ShoppingContributions = new List<ShoppingContribution>()
                };

            _pBen = new ShoppingPerson
                {
                    Id = 3,
                    FirstName = "Ben",
                    LastName = "Fahey",
                    ShoppingContributions = new List<ShoppingContribution>()
                };

            _pThom = new ShoppingPerson
                {
                    Id = 4,
                    FirstName = "Thom",
                    LastName = "Gibson",
                    ShoppingContributions = new List<ShoppingContribution>()
                };

            //Period
            _period = new ShoppingPeriod
                {
                    Id = 1,
                    Start = new DateTime(2013,1,1),
                    End = new DateTime(2013,2,1),
                    ShoppingGroups = new LinkedList<ShoppingGroup>()
                };

            //Groups
            _groupA = new ShoppingGroup
            {
                Id = 1,
                ShoppingPeriod = _period,
                ShoppingContributions = new List<ShoppingContribution>()
            };

            _groupB = new ShoppingGroup
            {
                Id = 2,
                ShoppingPeriod = _period,
                ShoppingContributions = new List<ShoppingContribution>()
            };

            //Contributions Group A
            var contributionA = new ShoppingContribution
                {
                    Id = 1,
                    ShoppingGroup = _groupA,
                    ShoppingPerson = _pAaron,
                    Amount = 485
                };
            var contributionB = new ShoppingContribution
                {
                    Id = 2,
                    ShoppingGroup = _groupA,
                    ShoppingPerson = _pBill,
                    Amount = 372
                };
            var contributionC = new ShoppingContribution
                {
                    Id = 3,
                    ShoppingGroup = _groupA,
                    ShoppingPerson = _pThom,
                    Amount = 143
                };

            //Contributions Group B
            var contributionD = new ShoppingContribution
            {
                Id = 4,
                ShoppingGroup = _groupB,
                ShoppingPerson = _pAaron,
                Amount = 0
            };
            var contributionE = new ShoppingContribution
            {
                Id = 5,
                ShoppingGroup = _groupB,
                ShoppingPerson = _pBill,
                Amount = 277
            };
            var contributionF = new ShoppingContribution
            {
                Id = 6,
                ShoppingGroup = _groupB,
                ShoppingPerson = _pThom,
                Amount = 0
            };
            var contributionG = new ShoppingContribution
            {
                Id = 7,
                ShoppingGroup = _groupB,
                ShoppingPerson = _pBen,
                Amount = 0
            };

            _groupA.ShoppingContributions.Add(contributionA);
            _pAaron.ShoppingContributions.Add(contributionA);
            _groupA.ShoppingContributions.Add(contributionB);
            _pBill.ShoppingContributions.Add(contributionB);
            _groupA.ShoppingContributions.Add(contributionC);
            _pThom.ShoppingContributions.Add(contributionC);
            _groupB.ShoppingContributions.Add(contributionD);
            _pAaron.ShoppingContributions.Add(contributionD);
            _groupB.ShoppingContributions.Add(contributionE);
            _pBill.ShoppingContributions.Add(contributionE);
            _groupB.ShoppingContributions.Add(contributionF);
            _pThom.ShoppingContributions.Add(contributionF);
            _groupB.ShoppingContributions.Add(contributionG);
            _pBen.ShoppingContributions.Add(contributionG);
            _period.ShoppingGroups.Add(_groupA);
            _period.ShoppingGroups.Add(_groupB);
        }

        [Test]
        public void GroupASpending()
        {
            var spending = _groupA.ShoppingContributions.Sum(c => c.Amount);
            Assert.AreEqual(1000, spending);
        }

        [Test]
        public void GroupBSpending()
        {
            var spending = _groupB.ShoppingContributions.Sum(c => c.Amount);
            Assert.AreEqual(277, spending);
        }

        [Test]
        public void GroupAStandings()
        {
            var standings = _groupA.GetGroupStanding();
            Assert.AreEqual(152, Math.Round(standings[_pAaron]));
            Assert.AreEqual(39, Math.Round(standings[_pBill]));
            Assert.AreEqual(-190, Math.Round(standings[_pThom]));
        }

        [Test]
        public void GroupBStandings()
        {
            var standings = _groupB.GetGroupStanding();
            Assert.AreEqual(-69, Math.Round(standings[_pAaron]));
            Assert.AreEqual(208, Math.Round(standings[_pBill]));
            Assert.AreEqual(-69, Math.Round(standings[_pThom]));
            Assert.AreEqual(-69, Math.Round(standings[_pBen]));
        }

        [Test]
        public void PeriodSpending()
        {
            var spending = _period.ShoppingGroups.Sum(x => x.ShoppingContributions.Sum(c => c.Amount));
            Assert.AreEqual(1277, spending);
        }

        [Test]
        public void PeriodTotalContributions()
        {
            var totalCont = _period.GetTotalContributions();
            Assert.AreEqual(485, Math.Round(totalCont[_pAaron]));
            Assert.AreEqual(649, Math.Round(totalCont[_pBill]));
            Assert.AreEqual(143, Math.Round(totalCont[_pThom]));
            Assert.AreEqual(0, Math.Round(totalCont[_pBen]));
        }

        [Test]
        public void PeriodStandings()
        {
            var standings = _period.GetTotalStanding();
            Assert.AreEqual(82, Math.Round(standings[_pAaron]));
            Assert.AreEqual(-260, Math.Round(standings[_pThom]));
            Assert.AreEqual(246, Math.Round(standings[_pBill]));
            Assert.AreEqual(-69, Math.Round(standings[_pBen]));
        }
    }
}
