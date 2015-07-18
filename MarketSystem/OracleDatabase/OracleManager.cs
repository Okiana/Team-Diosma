namespace MarketSystem.OracleDatabase
{
    using System.Linq;
    using Models;

    public class OracleManager : MarketData
    {
        public MarketData GetData()
        {
            var context = new OracleContext();

            context.PRODUCTS.OrderBy(p => p.PRODUCT_ID).ToList().ForEach(p => this.Products.Add(new Product
            {
                Id = (int)p.PRODUCT_ID,
                Name = p.PRODUCTNAME,
                Price = p.PRICE,
                VendorId = (int)p.VENDOR_ID,
                ProductTypeId = (int)p.TYPE_ID,
                MeasureId = (int)p.MEASURE_ID
            }));

            context.MEASURES.OrderBy(m => m.MEASURE_ID).ToList().ForEach(m => this.Measures.Add(new Measure
            {
                Id = (int)m.MEASURE_ID,
                Name = m.MEASURENAME
            }));

            context.PRODUCTSTYPES.OrderBy(pt => pt.TYPE_ID).ToList().ForEach(pt => this.ProductsTypes.Add(new ProductType
            {
                Id = (int)pt.TYPE_ID,
                Name = pt.TYPENAME
            }));

            context.VENDORS.OrderBy(v => v.VENDOR_ID).ToList().ForEach(v => this.Vendors.Add(new Vendor
            {
                Id = (int)v.VENDOR_ID,
                Name = v.VENDORNAME
            }));

            return this;
        }
    }
}
