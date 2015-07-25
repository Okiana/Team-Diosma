namespace MarketSystem.XMLReportByVedor
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using MsSqlDatabase;

    public class XmlReportGenerator
    {
        private const string ResultFileName = "result.xml";

        public static string GenerateXmlReport(DateTime startDate, DateTime endDate, string exportFolder)
        {
            ValidateDates(startDate, endDate);

            using (var context = new SqlMarketContext())
            {
                var doc = new XDocument();

                var rootNode = new XElement("sales");

                var vendors =
                    context.Sales.Where(s => s.Date >= startDate && s.Date <= endDate)
                        .GroupBy(
                            v => v.Product.Vendor.Name,
                            (key, val) => new { Name = key, Sales = val.Select(d => new { d.Date, d.TotalSum }) });

                foreach (var vendor in vendors)
                {
                    var currVendorNode = new XElement("sale");
                    currVendorNode.Add(new XAttribute("vendor", vendor.Name));

                    var sales = vendor.Sales.GroupBy(
                        s => s.Date,
                        (key, val) => new { Date = key, TotalSum = val.Sum(t => t.TotalSum) });
                    foreach (var sale in sales)
                    {
                        var currSaleNode = new XElement("summary");
                        currSaleNode.Add(new XAttribute("date", sale.Date.ToString("dd-MMM-yyyy")));
                        currSaleNode.Add(new XAttribute("total-sum", string.Format("{0:F2}", sale.TotalSum)));

                        currVendorNode.Add(currSaleNode);
                    }

                    rootNode.Add(currVendorNode);
                }

                doc.Add(rootNode);

                doc.Save(exportFolder + ResultFileName);
            }

            return exportFolder + ResultFileName;
        }

        private static void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Enter valid date");
            }
        }
    }
}