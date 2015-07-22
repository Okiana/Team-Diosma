namespace MarketSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
            this.SalesReports = new HashSet<SalesReport>();
            this.VendorExpenses = new HashSet<VendorExpense>();
        }

        public ICollection<Product> Products { get; private set; }

        public ICollection<ProductType> ProductsTypes { get; private set; }

        public ICollection<Vendor> Vendors { get; private set; }

        public ICollection<Measure> Measures { get; private set; }

        public ICollection<Supermarket> Supermarkets { get; private set; }

        public ICollection<Town> Towns { get; private set; }

        public ICollection<SalesReport> SalesReports { get; private set; }

        public ICollection<VendorExpense> VendorExpenses { get; private set; }

        public override string ToString()
        {
            var output = new StringBuilder();

            if (this.Towns.Any())
            {
                Console.WriteLine("====================================={0}Towns:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.Towns));
            }

            if (this.Supermarkets.Any())
            {
                Console.WriteLine("====================================={0}Supermarkets:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.Supermarkets));
            }

            if (this.Products.Any())
            {
                Console.WriteLine("====================================={0}Products:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.Products));
            }

            if (this.ProductsTypes.Any())
            {
                Console.WriteLine("====================================={0}Products types:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.ProductsTypes));
            }

            if (this.Vendors.Any())
            {
                Console.WriteLine("====================================={0}Vendors:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.Vendors));
            }

            if (this.Measures.Any())
            {
                Console.WriteLine("====================================={0}Measures:{0}=====================================", Environment.NewLine);
                Console.WriteLine(string.Join("-------------------------------------\n", this.Measures));
            }

            return output.ToString();
        }
    }
}