namespace MarketSystem.MsSqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    public class MsSqlManager : MarketData
    {
        public static void TransferData(MarketData marketData)
        {
            using (var context = new SqlMarketContext())
            {
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
        }

        public MarketData GetData()
        {
            using (var context = new SqlMarketContext())
            {
                this.GetTowns(context.Towns);
                this.GetSupermarkets(context.Supermarkets);
                this.GetMeasures(context.Measures);
                this.GetProductsTypes(context.ProductsTypes);
                this.GetVendors(context.Vendors);
                this.GetProducts(context.Products);
                this.GetSales(context.Sales);
                this.GetVendorExpenses(context.VendorExpenses);

                return this;
            }
        }

        public static IQueryable<IGrouping<DateTime, SaleReport>> FindSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            var context = new SqlMarketContext();

            var sales = context.Sales
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .Select(s => new SaleReport
                {
                    Product = s.Product.Name,
                    Quantity = s.Quantity,
                    ProductType = s.Product.ProductType.Name,
                    UnitPrice = s.UnitPrice,
                    Location = s.Supermarket.Name,
                    Sum = s.TotalSum,
                    Date = s.Date
                })
                .GroupBy(s => s.Date);

            return sales;
        }

        private void GetTowns(IQueryable<Town> towns)
        {
            towns
                .OrderBy(t => t.Id)
                .ToList()
                .ForEach(t => this.Towns.Add(new Town
                {
                    Id = t.Id,
                    Name = t.Name
                }));
        }

        private void GetSupermarkets(IQueryable<Supermarket> supermarkets)
        {
            supermarkets
                .OrderBy(s => s.Id)
                .ToList()
                .ForEach(s => this.Supermarkets.Add(new Supermarket
                {
                    Id = s.Id,
                    Name = s.Name,
                    TownId = s.TownId
                }));
        }

        private void GetMeasures(IQueryable<Measure> measures)
        {
            measures
                .OrderBy(m => m.Id)
                .ToList()
                .ForEach(m => this.Measures.Add(new Measure
                {
                    Id = m.Id,
                    Name = m.Name
                }));
        }

        private void GetProductsTypes(IQueryable<ProductType> productsTypes)
        {
            productsTypes
                .OrderBy(pt => pt.Id)
                .ToList()
                .ForEach(pt => this.ProductsTypes.Add(new ProductType
                {
                    Id = pt.Id,
                    Name = pt.Name
                }));
        }

        private void GetVendors(IQueryable<Vendor> vendors)
        {
            vendors
                .OrderBy(v => v.Id)
                .ToList()
                .ForEach(v => this.Vendors.Add(new Vendor
                {
                    Id = v.Id,
                    Name = v.Name
                }));
        }

        private void GetProducts(IQueryable<Product> products)
        {
            products
                .OrderBy(p => p.Id)
                .ToList()
                .ForEach(p => this.Products.Add(new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    VendorId = p.VendorId,
                    ProductTypeId = p.ProductTypeId,
                    MeasureId = p.MeasureId
                }));
        }

        private void GetSales(IQueryable<Sale> sales)
        {
            sales
                .OrderBy(s => s.Id)
                .ToList()
                .ForEach(s => this.SalesReports.Add(new Sale
                {
                    Id = s.Id,
                    Date = s.Date,
                    SupermarketId = s.SupermarketId,
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    TotalSum = s.TotalSum
                }));
        }

        private void GetVendorExpenses(IQueryable<VendorExpense> vendorExpenses)
        {
            vendorExpenses
                .ToList()
                .ForEach(v => this.VendorExpenses.Add(new VendorExpense
                {
                    VendorId = v.VendorId,
                    Month = v.Month,
                    Expenses = v.Expenses
                }));
        }

        public int? GetSupermarketIdByName(string supermarketName)
        {
            var context = new SqlMarketContext();

            var supermarketId = context.Supermarkets.FirstOrDefault(s => s.Name == supermarketName).Id;

            return supermarketId;
        }

        public int? GetProductIdByName(string productName)
        {
            var context = new SqlMarketContext();

            var productId = context.Products.FirstOrDefault(s => s.Name == productName).Id;

            return productId;
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