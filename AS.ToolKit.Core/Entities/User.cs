using System.Collections.Generic;

namespace AS.ToolKit.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ShoppingPeriod> ShoppingPeriods { get; set; }
        public virtual ICollection<ShoppingPerson> ShoppingPersons { get; set; } 
    }
}