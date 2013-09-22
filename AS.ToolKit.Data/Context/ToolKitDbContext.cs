using System.Data.Entity;
using AS.ToolKit.Core.Entities;

namespace AS.ToolKit.Data.Context
{
    public class ToolKitDbContext : DbContext
    {
        static ToolKitDbContext()
        {
            //Database.SetInitializer<ToolKitDbContext>(null);
        }

        public ToolKitDbContext()
            : base("ToolKitDbConnection")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingInterval> ShoppingIntervals { get; set; }
        public DbSet<ShoppingPerson> ShoppingPersons { get; set; }
        public DbSet<ShoppingGroup> ShoppingGroups { get; set; }
        public DbSet<ShoppingContribution> ShoppingContributions { get; set; }
    }

}