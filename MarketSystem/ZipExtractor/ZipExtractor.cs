namespace ZipExtractor
{
    using System;
    using System.IO;
    using System.Linq;
    using GemBox.Spreadsheet;
    using Ionic.Zip;
    using MarketSystem.Models;
    using MarketSystem.MsSqlDatabase;

    public class ZipExtractor : MarketData
    {
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
                var xlsFiles = zip.Where(z => z.FileName.EndsWith(".xls"));

                foreach (var report in xlsFiles)
                {
                    this.ParseReportData(report);
                }
            }

            return this;
        }

        private void ParseReportData(ZipEntry report)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                report.Extract(ms);
                ms.Position = 0;

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                ExcelFile ef = ExcelFile.Load(ms, LoadOptions.XlsDefault);
                var date = DateTime.Parse(report.FileName.Substring(0, 11));

                this.ParseExcelSheets(report.FileName, ef, date);
            }
        }

        private void ParseExcelSheets(string filename, ExcelFile report, DateTime reportDate)
        {
            foreach (ExcelWorksheet sheet in report.Worksheets)
            {
                Console.WriteLine(filename);

                var supermarket = sheet.Rows[1].AllocatedCells[1].Value.ToString();
                supermarket = this.ReplaceSpecialCharacters(supermarket);

                var dbSupermarket =
                    this.SqlMarketContext.Supermarkets.FirstOrDefault(s => s.Name == supermarket);

                if (dbSupermarket == null)
                {
                    Console.WriteLine("Supermarket does not exist in the database!");
                    continue;
                }

                var productsImported = this.ParseRowsData(reportDate, sheet, dbSupermarket.Id);

                Console.WriteLine("{0}/{1} sales imported.\n", productsImported, sheet.Rows.Count - 4);
            }
        }

        private int ParseRowsData(DateTime reportDate, ExcelWorksheet sheet, int dbSupermarketId)
        {
            var productsImported = 0;

            for (var i = 3; i < sheet.Rows.Count - 1; i++)
            {
                var product = sheet.Rows[i].AllocatedCells[1].Value.ToString().Normalize();
                product = this.ReplaceSpecialCharacters(product);

                var dbProduct = this.SqlMarketContext.Products.FirstOrDefault(p => p.Name == product);

                if (dbProduct != null)
                {
                    this.GenerateSalesReport(reportDate, sheet.Rows[i], dbSupermarketId, dbProduct.Id);
                    productsImported++;
                }
            }

            return productsImported;
        }

        private void GenerateSalesReport(DateTime date, ExcelRow row, int dbSupermarketId, int dbProductId)
        {
            var quantity = int.Parse(row.AllocatedCells[2].Value.ToString());
            var unitPrice = decimal.Parse(row.AllocatedCells[3].Value.ToString());
            var totalSum = decimal.Parse(row.AllocatedCells[4].Value.ToString());

            var salesReport = new Sale
            {
                SupermarketId = dbSupermarketId,
                ProductId = dbProductId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                TotalSum = totalSum,
                Date = date
            };

            this.SalesReports.Add(salesReport);
        }

        private string ReplaceSpecialCharacters(string name)
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