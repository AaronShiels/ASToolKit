using System.Collections.Generic;

namespace AS.ToolKit.Web.Models
{
    public class ShoppingPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<ShoppingContribution> ShoppingContributions { get; set; }
        public virtual User User { get; set; }
    }
}