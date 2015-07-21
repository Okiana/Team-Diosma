using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSystem.Models
{
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
