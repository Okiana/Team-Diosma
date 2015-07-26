namespace MarketSystem.Engine
{
    using System;
    using System.Globalization;
    using System.IO;
    using MySqlDatabase;
    using MarketSystem.MsSqlDatabase;
    using MarketSystem.OracleDatabase;
    using MarketSystem.XmlExpensesImport;
    using MarketSystem.XMLReportByVedor;
    using Reports;
    using ZipExtractor;

    public static class Engine
    {
        private const string SalesImportPath = @"..\..\..\..\Import\Sample-Sales-Reports.zip";
        private const string ExpensesImportPath = @"..\..\..\..\Import\Sample-Vendor-Expenses.xml";
        private const string XmlResultFileName = @"..\..\..\..\Export\";

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
            var zipExtractor = new ZipExtractor(SalesImportPath, sqlContext);
            var data = zipExtractor.ExtractData();

            Console.WriteLine("\nSending data to SQL Server...");
            MsSqlManager.TransferData(data);

            Console.WriteLine("Sales reports imported.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void XMLExport()
        {
            /*DateTime startDate = new DateTime(2014, 07, 20);
            DateTime endDate = new DateTime(2014, 07, 22);*/

            if (!Directory.Exists(XmlResultFileName))
            {
                Directory.CreateDirectory(XmlResultFileName);
            }
            
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Enter start date in format [yyyy.mm.dd]:");
            DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy.MM.dd", CultureInfo.InvariantCulture);
            Console.WriteLine("Enter end date in format [yyyy.mm.dd]: ");
            DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy.MM.dd", CultureInfo.InvariantCulture);
            Console.WriteLine(SeparatorLiner);

            Console.WriteLine("Generating report from sales to xml...");
            var path = XmlReportGenerator.GenerateXmlReport(startDate, endDate, XmlResultFileName);
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

        public static void SqlServerToMySqlTransfer()
        {
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Extracting data from SQL server...");
            var sqlManager = new MsSqlManager();

            Console.WriteLine("Sending data from MySQL server...");
            var data = sqlManager.GetData();
            MySqlManager.TransferData(data);

            Console.WriteLine("Data transferred.");
            Console.WriteLine(SeparatorLiner);
        }

        public static void GenerateFinancialReport()
        {
            DateTime[] timePeriod = TakeUserInput();
            DateTime startDate = timePeriod[0];
            DateTime endDate = timePeriod[1];
            
            Console.WriteLine(SeparatorLiner);
            Console.WriteLine("Generating Report...");
            var reportPath = SalesReportGenerator.ReportSalesByDate(startDate, endDate);
            Console.WriteLine("Report path: {0}", Path.GetFullPath(reportPath));
            Console.WriteLine(SeparatorLiner);
        }

        private static DateTime[] TakeUserInput()
        {
            while (true)
            {
                Console.WriteLine("Please enter start and end date in format dd.mm.yyyy" +
                                  " separated by space:");

                string[] userInput = Console.ReadLine().Split(' ');

                DateTime[] dates = new DateTime[2];

                if (userInput.Length == 2 &&
                    DateTime.TryParseExact(
                        userInput[0],
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dates[0]) &&
                    DateTime.TryParseExact(
                        userInput[1],
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dates[1]))
                {
                    return dates;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
}