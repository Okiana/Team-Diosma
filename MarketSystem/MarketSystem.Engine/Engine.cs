namespace MarketSystem.Engine
{
    using System;
    using System.IO;

    using MarketSystem.MsSqlDatabase;
    using MarketSystem.OracleDatabase;
    using MarketSystem.XmlExpensesImport;
    using MarketSystem.XMLReportByVedor;

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

            //Console.Write(data);
            
            Console.WriteLine("Transferring to SQL Server...");
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

            Console.WriteLine("\nImporting to SQL Server...");

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
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter end date in format [yyyy.mm.dd]: ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());
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
            var data = xmlExpensesReportToMsSql.Aaa();

            Console.WriteLine("Importing to SQL Server...");

            MsSqlManager.TransferData(data);

            Console.WriteLine("Vendor expense report imported.");
            Console.WriteLine(SeparatorLiner);
        }
    }
}
