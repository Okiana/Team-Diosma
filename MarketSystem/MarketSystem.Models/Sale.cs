namespace MarketSystem.Models
{
    using System;

    public class Sale
    {
        public int Id { get; set; }

        public int SupermarketId { get; set; }

        public DateTime Date { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalSum { get; set; }

        public virtual Supermarket Supermarket { get; set; }

        public virtual Product Product { get; set; }
    }
}