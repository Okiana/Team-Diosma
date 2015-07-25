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

        public virtual IDbSet<Supermarket> Supermarkets { get; set; }

        public virtual IDbSet<Town> Towns { get; set; }
                       
        public virtual IDbSet<Product> Products { get; set; }
                       
        public virtual IDbSet<ProductType> ProductsTypes { get; set; }
                       
        public virtual IDbSet<Vendor> Vendors { get; set; }
                       
        public virtual IDbSet<Measure> Measures { get; set; }
                       
        public virtual IDbSet<VendorExpense> VendorExpenses { get; set; }
                       
        public virtual IDbSet<Sale> Sales { get; set; }
    }
}