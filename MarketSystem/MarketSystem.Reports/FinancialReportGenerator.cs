namespace MarketSystem.Reports
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using GemBox.Spreadsheet;
    using Models;

    public static class FinancialReportGenerator
    {
        private const string Filename = "financial-report.xlsx";
        private static readonly string[] HeaderValues = { "Vendor", "Incomes", "Expenses", "Total Taxes", "Financial Result"};
        
        public static string GenerateFinancialReport(IEnumerable<FinancialReport> reportData, string folderPath)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("FinancialReport");

            CreateHeader(ws);

            var row = 1;

            foreach (var financialReport in reportData)
            {
                var vendor = financialReport.Vendor;
                var incomes = financialReport.Incomes;
                var expenses = financialReport.Expenses;
                var totalTaxes = financialReport.TotalTaxes;
                var financialResult = incomes - expenses - totalTaxes;
                
                FillRowData(ws, row, vendor, incomes, expenses, totalTaxes, financialResult);
                row++;
            }
            
            ApplyColumnsAutoWidth(ws);

            ef.Save(folderPath + Filename);

            return Path.GetFullPath(folderPath + Filename);
        }

        private static void CreateHeader(ExcelWorksheet ws)
        {
            var headerStyle = CreateHeaderStyle();

            for (int i = 0; i <= 4; i++)
            {
                ws.Cells[0, i].Value = HeaderValues[i];
                ws.Cells[0, i].Style = headerStyle;
            }
        }

        private static void FillRowData(ExcelWorksheet ws, int row, string vendor, decimal incomes, decimal expenses, decimal totalTaxes, decimal financialResult)
        {
            ws.Cells[row, 0].Value = vendor;
            ws.Cells[row, 0].Style = CreateDataStyle(row);
            ws.Cells[row, 1].Value = incomes;
            ws.Cells[row, 1].Style = CreateDataStyle(row, isNumber: true);
            ws.Cells[row, 2].Value = expenses;
            ws.Cells[row, 2].Style = CreateDataStyle(row, isNumber: true);
            ws.Cells[row, 3].Value = totalTaxes;
            ws.Cells[row, 3].Style = CreateDataStyle(row, isNumber: true);
            ws.Cells[row, 4].Value = financialResult;
            ws.Cells[row, 4].Style = CreateDataStyle(row, isBold:true, isNumber: true);
        }

        private static CellStyle CreateDataStyle(int row, bool isBold = false, bool isNumber = false)
        {
            CellStyle style = new CellStyle();
            
            style.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(0, 102, 0), LineStyle.Thin);

            if (row % 2 == 0)
            {
                style.FillPattern.SetSolid(Color.FromArgb(227, 243, 230)); 
            }

            if (isBold)
            {
                style.Font.Weight = ExcelFont.BoldWeight;
            }

            if (isNumber)
            {
                style.NumberFormat = "#,##0.00";
            }

            return style;
        }

        private static CellStyle CreateHeaderStyle()
        {
            CellStyle style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.FillPattern.SetSolid(Color.FromArgb(198, 239, 206));
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Color = Color.FromArgb(0, 97, 0);
            style.WrapText = true;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(0, 102, 0), LineStyle.Thin);

            return style;
        }

        private static void ApplyColumnsAutoWidth(ExcelWorksheet ws)
        {
            int columnCount = ws.CalculateMaxUsedColumns();

            for (int i = 0; i < columnCount; i++)
                ws.Columns[i].AutoFit();
        }
    }
}