namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class Vendor : Entity
    {
        public Vendor()
        {
            this.Products = new HashSet<Product>();
        }

        public virtual ICollection<Product> Products { get; set; }
    }
}