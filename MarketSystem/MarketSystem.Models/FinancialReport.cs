namespace MarketSystem.Models
{
    public class FinancialReport
    {
        public string Vendor { get; set; }

        public decimal Incomes { get; set; }

        public decimal Expenses { get; set; }

        public decimal? TotalTaxes { get; set; }
    }
}