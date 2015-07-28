namespace MarketSystem.Reports
{
    using System;
    using System.IO;
    using System.Linq;
    using GemBox.Spreadsheet;
    using Ionic.Zip;
    using Models;
    using MsSqlDatabase;

    public class ExcelSalesExtractor : MarketData
    {
        public ExcelSalesExtractor(string archivePath, SqlMarketContext context)
        {
            this.ArchivePath = archivePath;
            this.SqlMarketContext = context;
            this.SqlManager = new MsSqlManager();
        }

        public string ArchivePath { get; private set; }

        public SqlMarketContext SqlMarketContext { get; private set; }

        public MsSqlManager SqlManager { get; set; }

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

                var supermarketName = sheet.Rows[1].AllocatedCells[1].Value.ToString();
                supermarketName = this.ReplaceSpecialCharacters(supermarketName);

                var supermarketId =
                    this.SqlManager.GetSupermarketIdByName(supermarketName);

                if (supermarketId == null)
                {
                    Console.WriteLine("Supermarket does not exist in the database!");
                    continue;
                }

                var productsImported = this.ParseRowsData(reportDate, sheet, (int)supermarketId);

                Console.WriteLine("{0}/{1} sales imported.\n", productsImported, sheet.Rows.Count - 4);
            }
        }

        private int ParseRowsData(DateTime reportDate, ExcelWorksheet sheet, int dbSupermarketId)
        {
            var productsImported = 0;

            for (var i = 3; i < sheet.Rows.Count - 1; i++)
            {
                var productName = sheet.Rows[i].AllocatedCells[1].Value.ToString().Normalize();
                productName = this.ReplaceSpecialCharacters(productName);

                var productId = this.SqlManager.GetProductIdByName(productName);

                if (productId != null)
                {
                    this.GenerateSalesReport(reportDate, sheet.Rows[i], dbSupermarketId, (int)productId);
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