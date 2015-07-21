namespace MarketSystem.MsSqlDatabase
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class SqlMarketContext : DbContext
    {
        public SqlMarketContext()
            : base("MarketSystem")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqlMarketContext, Configuration>());
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductsTypes { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }

        public virtual DbSet<VendorExpense> VendorExpenses { get; set; }
    }
}