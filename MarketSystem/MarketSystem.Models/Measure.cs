namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class Measure : Entity
    {
        public Measure()
        {
            this.Products = new HashSet<Product>();
        }

        public virtual ICollection<Product> Products { get; set; }
    }
}