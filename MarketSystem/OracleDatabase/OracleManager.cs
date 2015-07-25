namespace MarketSystem.OracleDatabase
{
    using System.Linq;
    using Models;

    public class OracleManager : MarketData
    {
        public MarketData GetData()
        {
            var context = new OracleContext();

            this.GetTowns(context.TOWNS);
            this.GetSupermarkets(context.SUPERMARKETS);
            this.GetMeasures(context.MEASURES);
            this.GetProductsTypes(context.PRODUCTSTYPES);
            this.GetVendors(context.VENDORS);
            this.GetProducts(context.PRODUCTS);

            return this;
        }

        private void GetTowns(IQueryable<TOWN> towns)
        {
            towns
                .OrderBy(t => t.ID)
                .ToList()
                .ForEach(t => this.Towns.Add(new Town
                {
                    Id = t.ID,
                    Name = t.NAME
                }));
        }

        private void GetSupermarkets(IQueryable<SUPERMARKET> supermarkets)
        {
            supermarkets
                .OrderBy(s => s.ID)
                .ToList()
                .ForEach(s => this.Supermarkets.Add(new Supermarket
                {
                    Id = s.ID,
                    Name = s.NAME,
                    TownId = s.TOWNID
                }));
        }

        private void GetMeasures(IQueryable<MEASURE> measures)
        {
            measures
                .OrderBy(m => m.ID)
                .ToList()
                .ForEach(m => this.Measures.Add(new Measure
                {
                    Id = m.ID,
                    Name = m.NAME
                }));
        }

        private void GetProductsTypes(IQueryable<PRODUCTSTYPE> productsTypes)
        {
            productsTypes
                .OrderBy(pt => pt.ID)
                .ToList()
                .ForEach(pt => this.ProductsTypes.Add(new ProductType
                {
                    Id = pt.ID,
                    Name = pt.NAME
                }));
        }

        private void GetVendors(IQueryable<VENDOR> vendors)
        {
            vendors
                .OrderBy(v => v.ID)
                .ToList()
                .ForEach(v => this.Vendors.Add(new Vendor
                {
                    Id = v.ID,
                    Name = v.NAME
                }));
        }

        private void GetProducts(IQueryable<PRODUCT> products)
        {
            products
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
        }
    }
}