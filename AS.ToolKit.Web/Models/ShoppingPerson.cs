using System;
using System.Collections.Generic;

namespace AS.ToolKit.Web.Models
{
    public class ShoppingPerson : IEquatable<ShoppingPerson>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
        public virtual User User { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ShoppingPerson);
        }

        public bool Equals(ShoppingPerson other)
        {
            return other != null && other.Id == this.Id;
        }
    }
}