namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class ProductType : Entity
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }

        public virtual ICollection<Product> Products { get; set; }
    }
}