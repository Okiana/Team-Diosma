namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class MarketData
    {
        public MarketData()
        {
            this.Products = new HashSet<Product>();
            this.ProductsTypes = new HashSet<ProductType>();
            this.Vendors = new HashSet<Vendor>();
            this.Measures = new HashSet<Measure>();
            this.Supermarkets = new HashSet<Supermarket>();
            this.Towns = new HashSet<Town>();
            this.SalesReports = new HashSet<Sale>();
            this.VendorExpenses = new HashSet<VendorExpense>();
        }

        public ICollection<Product> Products { get; private set; }

        public ICollection<ProductType> ProductsTypes { get; private set; }

        public ICollection<Vendor> Vendors { get; private set; }

        public ICollection<Measure> Measures { get; private set; }

        public ICollection<Supermarket> Supermarkets { get; private set; }

        public ICollection<Town> Towns { get; private set; }

        public ICollection<Sale> SalesReports { get; private set; }

        public ICollection<VendorExpense> VendorExpenses { get; private set; }
    }
}