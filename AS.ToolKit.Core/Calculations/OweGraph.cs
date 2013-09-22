using System;
using System.Collections.Generic;
using System.Linq;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Core.Calculations
{
    public class OweGraph
    {
        private readonly List<OweVector> _vectors;

        public OweGraph(Dictionary<ShoppingPerson, decimal> standings)
        {
            _vectors = new List<OweVector>();

            var combinedTotalPositive = standings.Keys.Sum(person => Math.Abs(standings[person]))/2;

            foreach (var person in standings.Keys.Where(person => standings[person] < 0))
            {
                foreach (var otherPerson in standings.Keys.Where(otherPerson => standings[otherPerson] > 0))
                {
                    _vectors.Add(new OweVector
                        {
                            Giver = person,
                            Receiver = otherPerson,
                            Amount = (standings[otherPerson] / combinedTotalPositive) * -standings[person]
                        });
                }
            }
        }

        public void Simplify()
        {
            var didSomething = true;

            while (didSomething)
            {
                didSomething = false;

                foreach (var rAgA in _vectors)
                {
                    var receiverA = rAgA.Receiver;
                    var giverA = rAgA.Giver;
                    foreach (var rAgB in _vectors.Where(v => v.Receiver.Equals(receiverA) && !v.Giver.Equals(giverA)))
                    {
                        var giverB = rAgB.Giver;
                        foreach (
                            var rBgB in _vectors.Where(v => !v.Receiver.Equals(receiverA) && v.Giver.Equals(giverB)))
                        {
                            var receiverB = rBgB.Receiver;
                            foreach (
                                var rBgA in _vectors.Where(v => v.Receiver.Equals(receiverB) && v.Giver.Equals(giverA)))
                            {
                                if (rAgA.Amount > 0 && rAgB.Amount > 0 && rBgB.Amount > 0 && rBgA.Amount > 0)
                                {
                                    if (rAgA.Amount > rBgB.Amount)
                                    {
                                        rAgB.Amount += rBgB.Amount;
                                        rBgA.Amount += rBgB.Amount;
                                        rAgA.Amount -= rBgB.Amount;
                                        rBgB.Amount -= rBgB.Amount;

                                        didSomething = true;
                                    }
                                }
                            }
                        }
                    }
                }

                _vectors.RemoveAll(v => v.Amount == 0);
            }
        }

        public List<OweVector> GetVectors()
        {
            return _vectors;
        } 
    }

    public class OweVector
    {
        public ShoppingPerson Giver { get; set; }
        public decimal Amount { get; set; }
        public ShoppingPerson Receiver { get; set; }
    }
}