namespace MarketSystem.OracleDatabase
{
    using System.Linq;
    using Models;

    public class OracleManager : MarketData
    {
        public MarketData GetData()
        {
            var context = new OracleContext();

            context.TOWNS
                .OrderBy(t => t.ID)
                .ToList()
                .ForEach(t => this.Towns.Add(new Town
                {
                    Id = t.ID,
                    Name = t.NAME
                }));

            context.SUPERMARKETS
                .OrderBy(s => s.ID)
                .ToList()
                .ForEach(s => this.Supermarkets.Add(new Supermarket
                {
                    Id = s.ID,
                    Name = s.NAME,
                    TownId = s.TOWNID
                }));

            context.MEASURES
                .OrderBy(m => m.ID)
                .ToList()
                .ForEach(m => this.Measures.Add(new Measure
                {
                    Id = m.ID,
                    Name = m.NAME
                }));

            context.PRODUCTSTYPES
                .OrderBy(pt => pt.ID)
                .ToList()
                .ForEach(pt => this.ProductsTypes.Add(new ProductType
                {
                    Id = pt.ID,
                    Name = pt.NAME
                }));

            context.VENDORS
                .OrderBy(v => v.ID)
                .ToList()
                .ForEach(v => this.Vendors.Add(new Vendor
                {
                    Id = v.ID,
                    Name = v.NAME
                }));

            context.PRODUCTS
                .OrderBy(p => p.ID)
                .ToList()
                .ForEach(p => this.Products.Add(new Product
                    {
                        Id = p.ID,
                        Name = p.NAME,
                        Price = p.PRICE,
                        VendorId = p.VENDORID,
                        ProductTypeId = p.TYPEID,
                        MeasureId = p.MEASUREID
                    }));

            return this;
        }
    }
}