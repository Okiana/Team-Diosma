namespace MarketSystem.Reports
{
    using System;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Models;
    using MsSqlDatabase;

    public static class SalesReportGenerator
    {
        private const Font.FontFamily DefaultFont = Font.FontFamily.HELVETICA;
        private const int DefaultCellPadding = 5;
        private const string DefaultDateFormat = "dd-MMM-yyyy";
        private const string ReportDirectoryPath = "..\\..\\..\\..\\Export\\";
        private const int WidthOfTable = 5;

        public static string ReportSalesByDate(DateTime startDate, DateTime endDate)
        {
            var sales = MsSqlManager.FindSalesByDateRange(startDate, endDate);
            var fileName = string.Format("SalesReport_{0}_to_{1}.pdf", startDate.ToString(DefaultDateFormat), endDate.ToString(DefaultDateFormat));

            CreatePdfReport(sales, fileName, startDate, endDate);

            return ReportDirectoryPath + fileName;
        }

        private static void CreatePdfReport(IQueryable<IGrouping<DateTime, SaleReport>> salesByDate, string fileName, DateTime startDate, DateTime endDate)
        {
            if (!Directory.Exists(ReportDirectoryPath))
            {
                Directory.CreateDirectory(ReportDirectoryPath);
            }

            Document doc = new Document(iTextSharp.text.PageSize.A4, -30, -30, 10, 10);

            PdfWriter writer = PdfWriter.GetInstance(
                doc,
                new FileStream(ReportDirectoryPath + fileName, FileMode.Create));

            doc.Open();

            PdfPTable table = new PdfPTable(5);

            table.SetWidths(new int[] { 70, 25, 25, 80, 25 });

            table.AddCell(CreateTitleCell(
                string.Format("Sales Report:  {0}  /  {1}", 
                startDate.ToString(DefaultDateFormat),
                endDate.ToString(DefaultDateFormat))));

            decimal totalSum = 0m;

            foreach (var saleByDate in salesByDate)
            {
                table.AddCell(CreateDateCell(saleByDate.Key.ToString(DefaultDateFormat)));
                table.AddCell(CreateHeaderCell("Product"));
                table.AddCell(CreateHeaderCell("Quantity"));
                table.AddCell(CreateHeaderCell("Unit Price"));
                table.AddCell(CreateHeaderCell("Location"));
                table.AddCell(CreateHeaderCell("Sum"));

                foreach (var sale in saleByDate)
                {
                    table.AddCell(CreateDataCell(sale.Product));
                    table.AddCell(CreateDataCell(string.Empty + sale.Quantity + " " + sale.ProductType, 1));
                    table.AddCell(CreateDataCell(sale.UnitPrice.ToString("n2"), 2));
                    table.AddCell(CreateDataCell(sale.Location));
                    table.AddCell(CreateDataCell(sale.Sum.ToString("n2"), 2));
                }

                var totalSumCellValue = string.Format("Total sum for {0}: ", saleByDate.Key.ToString(DefaultDateFormat));
                decimal dateTotal = saleByDate.Sum(s => s.Sum);

                table.AddCell(CreateFooterCell(totalSumCellValue, colspan: WidthOfTable - 1));
                table.AddCell(CreateFooterCell(dateTotal.ToString("n2"), isBold: true));

                totalSum += dateTotal;
            }

            table.AddCell(CreateFooterCell(
                "Grand Total: ", 
                colspan: WidthOfTable - 1, 
                backgroundColor: new BaseColor(180, 198, 231)));

            table.AddCell(CreateFooterCell(
                totalSum.ToString("n2"), 
                isBold: true, 
                backgroundColor: new BaseColor(180, 198, 231)));

            doc.Add(table);
            doc.Close();
        }

        private static PdfPCell CreateTitleCell(string cellValue)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellValue, new Font(DefaultFont, 15f, Font.BOLD)));
            cell.Colspan = WidthOfTable;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.Padding = DefaultCellPadding;

            return cell;
        }

        private static PdfPCell CreateDateCell(string cellValue)
        {
            PdfPCell cell = new PdfPCell(new Phrase("Date: " + cellValue, new Font(DefaultFont, 10f)));
            cell.Colspan = WidthOfTable;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = DefaultCellPadding;
            cell.BackgroundColor = new BaseColor(242, 242, 242);

            return cell;
        }

        private static PdfPCell CreateHeaderCell(string cellValue)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellValue, new Font(DefaultFont, 10f, Font.BOLD)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.PaddingTop = DefaultCellPadding;
            cell.PaddingBottom = DefaultCellPadding;
            cell.BackgroundColor = new BaseColor(217, 217, 217);
            
            return cell;
        }

        private static PdfPCell CreateDataCell(string cellValue, int horizontalAlignment = Element.ALIGN_LEFT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellValue, new Font(DefaultFont, 9f)));
            cell.HorizontalAlignment = horizontalAlignment;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.Padding = DefaultCellPadding;
            return cell;
        }

        private static PdfPCell CreateFooterCell(string cellValue, int colspan = 1, bool isBold = false, BaseColor backgroundColor = null)
        {
            PdfPCell cell = new PdfPCell(new Phrase(cellValue, new Font(DefaultFont, 10f, isBold ? Font.BOLD : Font.NORMAL)));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Padding = DefaultCellPadding;
            cell.BackgroundColor = backgroundColor;

            return cell;
        }
    }
}
