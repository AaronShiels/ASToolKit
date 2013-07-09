using System.Composition;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AS.ToolKit.Web.Models;
using Alt.Composition;

namespace AS.ToolKit.Web.Parts
{
    [Export(typeof (ToolKitDb)),
     Export(typeof (DbContext)),
     Shared(Boundaries.HttpContext)]
    public class ToolKitDb : DbContext
    {
        public ToolKitDb()
            : base("ToolKitDbConnection")
        {
            Database.SetInitializer<ToolKitDb>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingPeriod> ShoppingPeriods { get; set; }
        public DbSet<ShoppingPerson> ShoppingPersons { get; set; }
        public DbSet<ShoppingGroup> ShoppingGroups { get; set; }
        public DbSet<ShoppingContribution> ShoppingContributions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ShoppingPeriod>()
                .HasRequired(p => p.User)
                .WithMany(u => u.ShoppingPeriods)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ShoppingPerson>()
                .HasRequired(p => p.User)
                .WithMany(u => u.ShoppingPersons)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ShoppingGroup>()
                .HasRequired(g => g.ShoppingPeriod)
                .WithMany(p => p.ShoppingGroups)
                .HasForeignKey(g => g.PeriodId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ShoppingContribution>()
                .HasRequired(c => c.ShoppingGroup)
                .WithMany(g => g.ShoppingContributions)
                .HasForeignKey(c => c.GroupId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ShoppingContribution>()
                .HasRequired(c => c.ShoppingPerson)
                .WithMany(p => p.ShoppingContributions)
                .HasForeignKey(c => c.PersonId)
                .WillCascadeOnDelete(true);
        }
    }

}