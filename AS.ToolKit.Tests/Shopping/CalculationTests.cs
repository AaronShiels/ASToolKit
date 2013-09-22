using System;
using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Calculations;
using AS.ToolKit.Core.Entities;
using NUnit.Framework;

namespace AS.ToolKit.Tests.Shopping
{
    public class CalculationTests
    {
        private OweGraph _oweGraph;
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

            //Standing Dictionary
            var standings = new Dictionary<ShoppingPerson, decimal>
                {
                    {_pAaron, 82},
                    {_pThom, -260},
                    {_pBill, 246},
                    {_pBen, -69}
                };

            //Create Graph
            _oweGraph = new OweGraph(standings);
        }

        [Test]
        public void RawGraph()
        {
            var vectors = _oweGraph.GetVectors();

            Assert.AreEqual(82, Math.Round(vectors.Where(v => v.Receiver.Equals(_pAaron)).Sum(v => v.Amount)));
            Assert.AreEqual(260, Math.Round(vectors.Where(v => v.Giver.Equals(_pThom)).Sum(v => v.Amount)));
            Assert.AreEqual(246, Math.Round(vectors.Where(v => v.Receiver.Equals(_pBill)).Sum(v => v.Amount)));
            Assert.AreEqual(69, Math.Round(vectors.Where(v => v.Giver.Equals(_pBen)).Sum(v => v.Amount)));
        }

        [Test]
        public void SimplifiedGraph()
        {
            var originalCount = _oweGraph.GetVectors().Count;
            _oweGraph.Simplify();
            var vectors = _oweGraph.GetVectors();

            Assert.AreEqual(82, Math.Round(vectors.Where(v => v.Receiver.Equals(_pAaron)).Sum(v => v.Amount)));
            Assert.AreEqual(260, Math.Round(vectors.Where(v => v.Giver.Equals(_pThom)).Sum(v => v.Amount)));
            Assert.AreEqual(246, Math.Round(vectors.Where(v => v.Receiver.Equals(_pBill)).Sum(v => v.Amount)));
            Assert.AreEqual(69, Math.Round(vectors.Where(v => v.Giver.Equals(_pBen)).Sum(v => v.Amount)));

            Assert.Greater(originalCount, vectors.Count);
        }
    }
}
