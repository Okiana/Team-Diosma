namespace MarketSystem.Models
{
    using System;
    using System.Text;

    public class Product : Entity
    {
        public decimal Price { get; set; }

        public int VendorId { get; set; }

        public int MeasureId { get; set; }

        public int ProductTypeId { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Measure Measure { get; set; }

        public virtual ProductType ProductType { get; set; }

        public override string ToString()
        {
            var product = new StringBuilder(base.ToString());
            product.AppendFormat("Type: {0}{1}", this.ProductTypeId, Environment.NewLine);
            product.AppendFormat("Vendor: {0}{1}", this.VendorId, Environment.NewLine);
            product.AppendFormat("Measure: {0}{1}", this.MeasureId, Environment.NewLine);
            product.AppendFormat("Price: {0}{1}", this.Price, Environment.NewLine);

            return product.ToString();
        }
    }
}