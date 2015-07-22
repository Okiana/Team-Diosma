namespace ZipExtractor
{
    using System;
    using System.Linq;
    using System.Text;
    using GemBox.Spreadsheet;
    using Ionic.Zip;
    using MarketSystem.Models;
    using MarketSystem.MsSqlDatabase;

    public class ZipExtractor : MarketData
    {
        private const string TargetDirectory = @"TempFiles\";

        public ZipExtractor(string archivePath, SqlMarketContext context)
        {
            this.ArchivePath = archivePath;
            this.SqlMarketContext = context;
        }

        public string ArchivePath { get; set; }

        public SqlMarketContext SqlMarketContext { get; set; }

        public MarketData ExtractData()
        {
            using (ZipFile zip = ZipFile.Read(this.ArchivePath))
            {
                var selection = zip.Entries
                    .Where(e => e.FileName.EndsWith(".xls"))
                    .Select(e => e);

                foreach (var report in selection)
                {
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                    ExcelFile ef = ExcelFile.Load(TargetDirectory + report.FileName);

                    var date = DateTime.Parse(report.FileName.Substring(0, 11));

                    //StringBuilder sb = new StringBuilder();

                    foreach (ExcelWorksheet sheet in ef.Worksheets)
                    {
                        Console.WriteLine(report.FileName);

                        //sb.AppendLine();
                        //sb.AppendFormat("--------- {0} ---------", sheet.Name);

                        var supermarket = sheet.Rows[1].AllocatedCells[1].Value.ToString();
                        

                        //sb.AppendFormat("{0}{1}{0}", Environment.NewLine, supermarket);

                        for (var i = 3; i < sheet.Rows.Count - 1; i++)
                        {
                            var product = sheet.Rows[i].AllocatedCells[1].Value.ToString();
                            var quantity = int.Parse(sheet.Rows[i].AllocatedCells[2].Value.ToString());
                            var unitPrice = decimal.Parse(sheet.Rows[i].AllocatedCells[3].Value.ToString());
                            var totalSum = decimal.Parse(sheet.Rows[i].AllocatedCells[4].Value.ToString());

                            //sb.AppendFormat("{0}\t{1}\t{2:F2}\t{3:F2}{4}", product, quantity, unitPrice, totalSum,
                            //    Environment.NewLine);

                            var dbSupermarket =
                                this.SqlMarketContext.Supermarkets.FirstOrDefault(s => s.Name == supermarket);

                            var dbProduct = this.SqlMarketContext.Products.FirstOrDefault(p => p.Name == product);

                            if (dbSupermarket != null && dbProduct != null)
                            {
                                var salesReport = new SalesReport
                                {
                                    Supermarket = dbSupermarket,
                                    Product = dbProduct,
                                    Quantity = quantity,
                                    UnitPrice = unitPrice,
                                    TotalSum = totalSum,
                                    Date = date
                                };

                                this.SalesReports.Add(salesReport);
                            }
                        }

                        //Console.WriteLine(sb);
                    }
                }
            }

            return this;
        }
    }
}