using System.Composition;
using System.Data.Entity;
using AS.ToolKit.Core.Entities;
using Alt.Composition;

namespace AS.ToolKit.Data.Repository.Parts
{
    public class ToolKitDb : DbContext
    {
        public ToolKitDb()
            : base("ToolKitDbConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingPeriod> ShoppingPeriods { get; set; }
        public DbSet<ShoppingPerson> ShoppingPersons { get; set; }
        public DbSet<ShoppingGroup> ShoppingGroups { get; set; }
        public DbSet<ShoppingContribution> ShoppingContributions { get; set; }
    }

}