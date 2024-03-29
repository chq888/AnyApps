namespace BeYourMarket.Core.Migrations
{
    using AnyApps.Common;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class ConfigurationInstall<T> : DbMigrationsConfiguration<T> where T: DbContext
    {
        public ConfigurationInstall()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = BeYourMarketConfigurationManager.AutomaticMigrationDataLossAllowed;
            ContextKey = AnyConstants.CONTEXT_KEY;
            TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(AnyConstants.CONNECTION_STRING);            
        }

        protected override void Seed(T context)
        {
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
        }
    }
}
