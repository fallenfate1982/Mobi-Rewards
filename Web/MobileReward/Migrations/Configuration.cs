using System.Web.Security;
using WebMatrix.WebData;
using System.Data.Entity.Migrations;

namespace MobileReward.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MobileReward.Models.MobiRewardDb>
    {
        public Configuration()
        {
          AutomaticMigrationsEnabled = true;
          AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MobileReward.Models.MobiRewardDb context)
        {
          //TODO: Add admin account
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

          SeedMembership();
        }

        private void SeedMembership()
        {
          DataBaseConfig.RegisterDatabaseConnection();

          var roles = (SimpleRoleProvider) Roles.Provider;
          
          if (!roles.RoleExists("Admin"))
          {
            roles.CreateRole("Admin");
          }

          if (!roles.RoleExists("Merchant"))
          {
            roles.CreateRole("Merchant");
          }
        }

    }
}
