using System.Collections.Generic;
using System.Linq;

namespace AS.ToolKit.Core.Entities
{
    public class ShoppingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
        public virtual ShoppingInterval ShoppingInterval { get; set; }
    }
}