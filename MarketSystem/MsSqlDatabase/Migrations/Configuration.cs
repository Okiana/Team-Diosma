namespace MarketSystem.MsSqlDatabase.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MarketSystem.MsSqlDatabase.SqlMarketContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationDataLossAllowed = true; // disable it for production!!!
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "MarketSystem.MsSqlDatabase.SqlMarketContext";
        }

        protected override void Seed(MarketSystem.MsSqlDatabase.SqlMarketContext context)
        {
        }
    }
}
