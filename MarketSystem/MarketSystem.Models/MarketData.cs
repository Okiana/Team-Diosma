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
        }

        public ICollection<Product> Products { get; private set; }

        public ICollection<ProductType> ProductsTypes { get; private set; }

        public ICollection<Vendor> Vendors { get; private set; }

        public ICollection<Measure> Measures { get; private set; }

        public override string ToString()
        {
            var output = new StringBuilder();

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