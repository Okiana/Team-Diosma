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

            marketData.Towns.ToList().ForEach(t => context.Towns.AddOrUpdate(n => n.Id, t));

            marketData.Supermarkets.ToList().ForEach(s => context.Supermarkets.AddOrUpdate(n => n.Id, s));

            marketData.ProductsTypes.ToList().ForEach(pt => context.ProductsTypes.AddOrUpdate(n => n.Id, pt));

            marketData.Vendors.ToList().ForEach(v => context.Vendors.AddOrUpdate(n => n.Id, v));

            marketData.Measures.ToList().ForEach(m => context.Measures.AddOrUpdate(n => n.Id, m));

            marketData.Products.ToList().ForEach(p => context.Products.AddOrUpdate(n => n.Id, p));

            marketData.SalesReports.ToList().ForEach(sr => context.SalesReports.AddOrUpdate(n => new {n.SupermarketId, n.ProductId, n.Date}, sr));

            marketData.VendorExpenses.ToList().ForEach(vs => context.VendorExpenses.AddOrUpdate(n => new {n.VendorId, n.Month}, vs));

            context.SaveChanges();
        }
    }
}