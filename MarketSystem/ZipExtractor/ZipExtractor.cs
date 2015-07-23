namespace ZipExtractor
{
    using System;
    using System.IO;
    using System.Linq;
    using GemBox.Spreadsheet;
    using Ionic.Zip;
    using MarketSystem.Models;
    using MarketSystem.MsSqlDatabase;
    using System.Text.RegularExpressions;

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
            this.DeleteTempFolder();

            using (ZipFile zip = ZipFile.Read(this.ArchivePath))
            {
                zip.ExtractAll(TargetDirectory);

                var selection = zip.Entries
                    .Where(e => e.FileName.EndsWith(".xls"))
                    .Select(e => e);

                foreach (var report in selection)
                {
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                    ExcelFile ef = ExcelFile.Load(TargetDirectory + report.FileName);

                    var date = DateTime.Parse(report.FileName.Substring(0, 11));
                    
                    foreach (ExcelWorksheet sheet in ef.Worksheets)
                    {
                        Console.WriteLine(report.FileName);

                        var supermarket = sheet.Rows[1].AllocatedCells[1].Value.ToString();
                        supermarket = this.ReplaceMsCharacters(supermarket);

                        for (var i = 3; i < sheet.Rows.Count - 1; i++)
                        {
                            var product = sheet.Rows[i].AllocatedCells[1].Value.ToString().Normalize();
                            product = this.ReplaceMsCharacters(product);
                            var quantity = int.Parse(sheet.Rows[i].AllocatedCells[2].Value.ToString());
                            var unitPrice = decimal.Parse(sheet.Rows[i].AllocatedCells[3].Value.ToString());
                            var totalSum = decimal.Parse(sheet.Rows[i].AllocatedCells[4].Value.ToString());

                            var dbSupermarket =
                                this.SqlMarketContext.Supermarkets.FirstOrDefault(s => s.Name == supermarket);

                            var dbProduct = this.SqlMarketContext.Products.FirstOrDefault(p => p.Name == product);

                            if (dbSupermarket != null && dbProduct != null)
                            {
                                var salesReport = new SalesReport
                                {
                                    SupermarketId = dbSupermarket.Id,
                                    ProductId = dbProduct.Id,
                                    Quantity = quantity,
                                    UnitPrice = unitPrice,
                                    TotalSum = totalSum,
                                    Date = date
                                };

                                this.SalesReports.Add(salesReport);
                            }
                        }
                    }
                }
            }

            this.DeleteTempFolder();

            return this;
        }

        private void DeleteTempFolder()
        {
            if (Directory.Exists(TargetDirectory))
            {
                Directory.Delete(TargetDirectory, true);
            }
        }

        private string ReplaceMsCharacters(string name)
        {
            var newName = name.Trim();

            newName = newName.Replace('\u2013', '-');
            newName = newName.Replace('\u2014', '-');
            newName = newName.Replace('\u2015', '-');
            newName = newName.Replace('\u2017', '_');
            newName = newName.Replace('\u2018', '\'');
            newName = newName.Replace('\u2019', '\'');
            newName = newName.Replace('\u201a', ',');
            newName = newName.Replace('\u201b', '\'');
            newName = newName.Replace('\u201c', '\"');
            newName = newName.Replace('\u201d', '\"');
            newName = newName.Replace('\u201e', '\"');
            newName = newName.Replace("\u2026", "...");
            newName = newName.Replace('\u2032', '\'');
            newName = newName.Replace('\u2033', '\"');

            return newName; 
        }
    }
}