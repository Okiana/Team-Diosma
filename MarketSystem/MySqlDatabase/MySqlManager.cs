namespace MarketSystem.MySqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using SqLiteDatabase;

    public class MySqlManager : MarketData
    {
        public static void TransferData(MarketData marketData)
        {
            var context = new MySqlContext();

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

        public IEnumerable<FinancialReport> GetVendorResults()
        {
            var context = new MySqlContext();
            var vendors = context.vendors.OrderBy(v => v.name).Select(v => v.name).ToList();
            var vendorsReport = new HashSet<FinancialReport>();

            var vendorSales = context.sales
                .Select(s => new
                {
                    Vendor = s.product.vendor.name,
                    Product = s.product.name,
                    Sum = s.total_sum
                }).ToList();

            var vendorTaxes = vendorSales.Select(s => new
                {
                    s.Vendor,
                    Tax = SqLiteManager.GetTaxByProduct(s.Product) * s.Sum
                })
                .GroupBy(v => v.Vendor, (k, v) => new
                {
                    Vendor = k,
                    Taxes = v.Sum(t => t.Tax)
                }).ToList();

            var groupedSales = vendorSales
                .GroupBy(s => s.Vendor, (k, v) => new
                {
                    Vendor = k,
                    Incomes = v.Sum(a => a.Sum)
                }).ToList();

            var vendorExpenses = context.vendor_expenses
                .GroupBy(e => e.vendor.name, (k, v) => new
                {
                    Vendor = k,
                    Expenses = v.Sum(s => s.expenses)
                })
                .ToList();

            foreach (var vendor in vendors)
            {
                var incomes = groupedSales.FirstOrDefault(v => v.Vendor == vendor);
                var expenses = vendorExpenses.FirstOrDefault(v => v.Vendor == vendor);
                var taxes = vendorTaxes.FirstOrDefault(v => v.Vendor == vendor);
                var vendorReport = new FinancialReport
                {
                    Vendor = vendor,
                    Incomes = incomes != null ? incomes.Incomes : 0M,
                    Expenses = expenses != null ? expenses.Expenses : 0M,
                    TotalTaxes = taxes != null ? taxes.Taxes : 0M
                };

                vendorsReport.Add(vendorReport);
            }

            return vendorsReport;
        }

        private static void TransferTowns(IEnumerable<Town> towns, MySqlContext context)
        {
            towns.ToList()
                .ForEach(t => context.towns.AddOrUpdate(i => i.id , new town
                {
                    id = t.Id,
                    name = t.Name
                }));
        }

        private static void TransferSupermarkets(IEnumerable<Supermarket> supermarkets, MySqlContext context)
        {
            supermarkets.ToList()
                .ForEach(s => context.supermarkets.AddOrUpdate(i => i.id, new supermarket
                {
                    id = s.Id,
                    name = s.Name,
                    town_id = s.TownId
                }));
        }

        private static void TransferProductsTypes(IEnumerable<ProductType> productsTypes, MySqlContext context)
        {
            productsTypes.ToList()
                .ForEach(pt => context.products_types.AddOrUpdate(i => i.id, new products_types
                {
                    id = pt.Id,
                    name = pt.Name
                }));
        }

        private static void TransferVendors(IEnumerable<Vendor> vendors, MySqlContext context)
        {
            vendors.ToList()
                .ForEach(v => context.vendors.AddOrUpdate(i => i.id, new vendor
                {
                    id = v.Id,
                    name = v.Name
                }));
        }

        private static void TransferMeasures(IEnumerable<Measure> measures, MySqlContext context)
        {
            measures.ToList()
                .ForEach(m => context.measures.AddOrUpdate(i => i.id, new measure
                {
                    id = m.Id,
                    name = m.Name
                }));
        }

        private static void TransferProducts(IEnumerable<Product> products, MySqlContext context)
        {
            products.ToList()
                .ForEach(p => context.products.AddOrUpdate(i => i.id, new product
                {
                    id = p.Id,
                    name = p.Name,
                    vendor_id = p.VendorId,
                    measure_id = p.MeasureId,
                    type_id = p.ProductTypeId,
                    price = p.Price
                }));
        }

        private static void TransferSalesReports(IEnumerable<Sale> salesReports, MySqlContext context)
        {
            salesReports.ToList()
                .ForEach(sr => context.sales.AddOrUpdate(n => new { n.supermarket_id, n.product_id, n.date }, new sale
                {
                    id = sr.Id,
                    date = sr.Date,
                    supermarket_id = sr.SupermarketId,
                    product_id = sr.ProductId,
                    quantity = sr.Quantity,
                    unit_price = sr.UnitPrice,
                    total_sum = sr.TotalSum
                }));
        }

        private static void TransferVendorExpenses(IEnumerable<VendorExpense> vendorExpenses, MySqlContext context)
        {
            vendorExpenses.ToList()
                .ForEach(vs => context.vendor_expenses.AddOrUpdate(n => new { n.vendor_id, n.month }, new vendor_expenses
                {
                    vendor_id = vs.VendorId,
                    month = vs.Month,
                    expenses = vs.Expenses
                }));
        }
    }
}