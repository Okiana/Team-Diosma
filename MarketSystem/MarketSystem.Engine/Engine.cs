namespace MarketSystem.Engine
{
    using System;
    using System.Globalization;
    using System.IO;

    using MarketSystem.MsSqlDatabase;
    using MarketSystem.MySqlDatabase;
    using MarketSystem.OracleDatabase;
    using MarketSystem.Reports;

    public static class Engine
    {
        private const string SalesImportPath = @"..\..\..\..\Import\Sample-Sales-Reports.zip";
        private const string ExpensesImportPath = @"..\..\..\..\Import\Sample-Vendor-Expenses.xml";
        private const string ExportDirectory = @"..\..\..\..\Export\";

        private const char SeparatorSymbol = '-';
        private const int SeparatorLength = 50;

        private static readonly string SeparatorLiner = new string(SeparatorSymbol, SeparatorLength);

        public static void OracleToMsSqlTransfer()
        {
            var oracleManager = new OracleManager();

            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Extracting data from Oracle Database...");
            var data = oracleManager.GetData();
            
            Console.WriteLine("Sending data to SQL Server...");
            MsSqlManager.TransferData(data);

            Console.WriteLine("Data transferred.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void ZipExcelReportsToMsSql()
        {
            var sqlContext = new SqlMarketContext();

            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Extracting data from reports...\n");
            var zipExtractor = new ExcelSalesExtractor(SalesImportPath, sqlContext);
            var data = zipExtractor.ExtractData();

            Console.WriteLine("\nSending data to SQL Server...");
            MsSqlManager.TransferData(data);

            Console.WriteLine("Sales reports imported.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void XMLExport()
        {
            CreateNonExistingDirectory(ExportDirectory);
            
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Enter start date in format [yyyy.mm.dd]:");
            DateTime startDate = ReadUserInputDate();
            Console.WriteLine("Enter end date in format [yyyy.mm.dd]:");
            DateTime endDate = ReadUserInputDate();
            Console.WriteLine(SeparatorLiner);

            Console.WriteLine("Generating report from sales to xml...");
            var path = XmlReportGenerator.GenerateXmlReport(startDate, endDate, ExportDirectory);
            Console.WriteLine("The report is done!\n Path: {0}", Path.GetFullPath(path));
            Console.WriteLine(SeparatorLiner);
        }

        public static void XmlExpensesReportToMsSql()
        {
            var sqlContext = new SqlMarketContext();

            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Extracting data from report... ");
            var xmlExpensesReportToMsSql = new XmlVendorExprensesImport(ExpensesImportPath, sqlContext);
            var data = xmlExpensesReportToMsSql.GetReportData();

            Console.WriteLine("Sending data to SQL Server...");
            MsSqlManager.TransferData(data);

            Console.WriteLine("Vendor expense report imported.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void JsonReportsToMongoDb()
        {
            CreateNonExistingDirectory(ExportDirectory);

            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Enter start date in format [yyyy.mm.dd]:");
            DateTime startDate = ReadUserInputDate();
            Console.WriteLine("Enter end date in format [yyyy.mm.dd]:");
            DateTime endDate = ReadUserInputDate();
            Console.WriteLine(SeparatorLiner);

            JsonProductReportToMongoDb.JsonProductSalses(startDate, endDate, ExportDirectory);

            Console.WriteLine("Generating report for products sales to json...");
            Console.WriteLine("The report is done!\n Path: {0}", Path.GetFullPath(ExportDirectory));
            
            Console.WriteLine("Importing reports into mongo database...");
            JsonProductReportToMongoDb.ImportJsonToMongoDb();
            //Console.WriteLine(Path.GetFullPath(@"..\..\..\..\Mongodb\data"));
            Console.WriteLine("Reports are imported in collection \"ProductsReports\" in database \"ProductsSales\"");
            Console.WriteLine(SeparatorLiner);
        }

        public static void SqlServerToMySqlTransfer()
        {
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Extracting data from SQL server...");
            var sqlManager = new MsSqlManager();

            Console.WriteLine("Sending data to MySQL server...");
            var data = sqlManager.GetData();
            MySqlManager.TransferData(data);

            Console.WriteLine("Data transferred.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void GeneratePdfSalesReport()
        {
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Enter start date in format [yyyy.mm.dd]:");
            DateTime startDate = ReadUserInputDate();
            Console.WriteLine("Enter end date in format [yyyy.mm.dd]: ");
            DateTime endDate = ReadUserInputDate();
            
            Console.WriteLine("Generating Report...");
            var reportPath = SalesReportGenerator.ReportSalesByDate(startDate, endDate);
            Console.WriteLine("Report path: {0}", Path.GetFullPath(reportPath));
            Console.WriteLine(SeparatorLiner);
        }

        public static void GenerateFinancialReport()
        {
            CreateNonExistingDirectory(ExportDirectory);

            Console.WriteLine(SeparatorLiner);

            Console.WriteLine("Extracting Report data...");
            var mysqlManager = new MySqlManager();
            var data = mysqlManager.GetVendorResults();

            Console.WriteLine("Generating Report...");
            var reportPath = FinancialReportGenerator.GenerateFinancialReport(data, ExportDirectory);

            Console.WriteLine("Report path: {0}", reportPath);
            Console.WriteLine(SeparatorLiner);
        }

        public static void PrintGoodByeMessage()
        {
            Console.WriteLine("Good bye!");
        }

        private static DateTime ReadUserInputDate()
        {
            var date = DateTime.ParseExact(Console.ReadLine().Trim(), "yyyy.MM.dd", CultureInfo.InvariantCulture);

            return date;
        }

        private static void CreateNonExistingDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}