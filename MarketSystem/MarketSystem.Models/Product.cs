namespace MarketSystem.Models
{
    public class Product : Entity
    {
        public decimal Price { get; set; }

        public int VendorId { get; set; }

        public int MeasureId { get; set; }

        public int ProductTypeId { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Measure Measure { get; set; }

        public virtual ProductType ProductType { get; set; }
    }
}