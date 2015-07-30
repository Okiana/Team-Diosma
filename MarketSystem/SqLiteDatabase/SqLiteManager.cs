namespace SqLiteDatabase
{
    using System.Collections.Generic;
    using System.Linq;

    public class SqLiteManager
    {
        public static IEnumerable<MarketSystem.Models.TaxInformation> GetTaxes()
        {
            var context = new SqLiteEntities();

            var taxes = new HashSet<MarketSystem.Models.TaxInformation>();

            context.TaxInformations.ToList().ForEach(t => taxes.Add(new MarketSystem.Models.TaxInformation
            {
                Id = (int)t.Id,
                ProductName = t.ProductName,
                Tax = (int)t.Tax
            }));

            return taxes;
        }

        public static decimal GetTaxByProduct(string productName)
        {
            using (var context = new SqLiteEntities())
            {
               var tax = context.TaxInformations.FirstOrDefault(t => t.ProductName == productName);

               return tax != null ? (tax.Tax / 100M) : 0;
            }
        }
    }
}