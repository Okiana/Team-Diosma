﻿namespace ZipExtractor
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

                        for (var i = 3; i < sheet.Rows.Count - 1; i++)
                        {
                            var product = sheet.Rows[i].AllocatedCells[1].Value.ToString();
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
                    }
                }
            }

            this.DeleteTempFolder();

            return this;
        }

        public void DeleteTempFolder()
        {
            if (Directory.Exists(TargetDirectory))
            {
                Directory.Delete(TargetDirectory, true);
            }
        } 
    }
}