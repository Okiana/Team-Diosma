namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class Vendor : Entity
    {
        public Vendor()
        {
            this.Products = new HashSet<Product>();
            this.VendorExpenses = new HashSet<VendorExpense>();
        }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<VendorExpense> VendorExpenses { get; set; }
    }
}