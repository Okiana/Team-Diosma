namespace MarketSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class VendorExpense
    {
        [Key]
        [Column(Order = 1)] 
        public int VendorId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime Month { get; set; }

        public virtual Vendor Vendor { get; set; }

        public decimal Expenses { get; set; }
    }
}
