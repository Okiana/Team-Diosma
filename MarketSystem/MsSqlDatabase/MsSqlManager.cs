namespace MarketSystem.MsSqlDatabase
{
    using Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class MsSqlManager
    {
        public static void TransferData(MarketData marketData)
        {
            var context = new SqlMarketContext();

            marketData.ProductsTypes.ToList().ForEach(pt => context.ProductsTypes.AddOrUpdate(n => n.Id, pt));
            context.SaveChanges();

            marketData.Vendors.ToList().ForEach(v => context.Vendors.AddOrUpdate(n => n.Id, v));
            context.SaveChanges();

            marketData.Measures.ToList().ForEach(m => context.Measures.AddOrUpdate(n => n.Id, m));
            context.SaveChanges();

            marketData.Products.ToList().ForEach(p => context.Products.AddOrUpdate(n => n.Id, p));
            context.SaveChanges();
        }
    }
}