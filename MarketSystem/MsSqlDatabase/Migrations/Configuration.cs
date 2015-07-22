namespace MarketSystem.MsSqlDatabase.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MarketSystem.MsSqlDatabase.SqlMarketContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationDataLossAllowed = true; //disable it for production!!!
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "MarketSystem.MsSqlDatabase.SqlMarketContext";
        }

        protected override void Seed(MarketSystem.MsSqlDatabase.SqlMarketContext context)
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
