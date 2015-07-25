namespace MarketSystem.MsSqlDatabase
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    public class MsSqlManager
    {
        public static void TransferData(MarketData marketData)
        {
            var context = new SqlMarketContext();

            TransferTowns(marketData.Towns, context);
            TransferSupermarkets(marketData.Supermarkets, context);
            TransferProductsTypes(marketData.ProductsTypes, context);
            TransferVendors(marketData.Vendors, context);
            TransferMeasures(marketData.Measures, context);
            TransferProducts(marketData.Products, context);
            TransferSalesReports(marketData.SalesReports, context);
            TransferVendorExpenses(marketData.VendorExpenses, context);

            context.SaveChanges();
        }

        private static void TransferTowns(IEnumerable<Town> towns, SqlMarketContext context)
        {
            towns.ToList()
                .ForEach(t => context.Towns.AddOrUpdate(n => n.Id, t));
        }

        private static void TransferSupermarkets(IEnumerable<Supermarket> supermarkets, SqlMarketContext context)
        {
            supermarkets.ToList()
                .ForEach(s => context.Supermarkets.AddOrUpdate(n => n.Id, s));
        }

        private static void TransferProductsTypes(IEnumerable<ProductType> productsTypes, SqlMarketContext context)
        {
            productsTypes.ToList()
                .ForEach(pt => context.ProductsTypes.AddOrUpdate(n => n.Id, pt));
        }

        private static void TransferVendors(IEnumerable<Vendor> vendors, SqlMarketContext context)
        {
            vendors.ToList()
                .ForEach(v => context.Vendors.AddOrUpdate(n => n.Id, v));
        }

        private static void TransferMeasures(IEnumerable<Measure> measures, SqlMarketContext context)
        {
            measures.ToList()
                .ForEach(m => context.Measures.AddOrUpdate(n => n.Id, m));
        }

        private static void TransferProducts(IEnumerable<Product> products, SqlMarketContext context)
        {
            products.ToList()
                .ForEach(p => context.Products.AddOrUpdate(n => n.Id, p));
        }

        private static void TransferSalesReports(IEnumerable<Sale> salesReports, SqlMarketContext context)
        {
            salesReports.ToList()
                .ForEach(sr => context.Sales.AddOrUpdate(n => new { n.SupermarketId, n.ProductId, n.Date }, sr));
        }

        private static void TransferVendorExpenses(IEnumerable<VendorExpense> vendorExpenses, SqlMarketContext context)
        {
            vendorExpenses.ToList()
                .ForEach(vs => context.VendorExpenses.AddOrUpdate(n => new { n.VendorId, n.Month }, vs));
        }
    }
}