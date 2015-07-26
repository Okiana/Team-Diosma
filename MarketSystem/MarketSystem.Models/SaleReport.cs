namespace MarketSystem.Models
{
    using System;

    public class SaleReport
    {
        public string Product { get; set; }

        public int Quantity { get; set; }

        public string ProductType { get; set; }

        public decimal UnitPrice { get; set; }

        public string Location { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }
    }
}