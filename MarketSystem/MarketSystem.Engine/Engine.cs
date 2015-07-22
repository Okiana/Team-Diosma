namespace MarketSystem.Engine
{
    using System;
    using MsSqlDatabase;
    using OracleDatabase;
    using XmlExpensesImport;
    using ZipExtractor;

    public static class Engine
    {
        private const string SalesImportPath = @"..\..\..\..\Import\Sample-Sales-Reports.zip";
        private const string ExpensesImportPath = @"..\..\..\..\Import\Sample-Vendor-Expenses.xml";

        public static void OracleToMsSqlTransfer()
        {
            var oracleManager = new OracleManager();

            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Extracting data from Oracle Database...");

            var data = oracleManager.GetData();

            //Console.Write(data);
            
            Console.WriteLine("Transferring to SQL Server...");
            MsSqlManager.TransferData(data);

            Console.WriteLine("Data transferred.");
            Console.WriteLine(new string('-', 50));
        }

        public static void ZipExcelReportsToMsSql()
        {
            var sqlContext = new SqlMarketContext();
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Extracting data from reports...\n");

            var zipExtractor = new ZipExtractor(SalesImportPath, sqlContext);
            var data = zipExtractor.ExtractData();

            Console.WriteLine("\nImporting to SQL Server...");

            MsSqlManager.TransferData(data);

            Console.WriteLine("Sales reports imported.");
            Console.WriteLine(new string('-', 50));
        }

        public static void XmlExpensesReportToMsSql()
        {
            var sqlContext = new SqlMarketContext();

            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Extracting data from report... ");

            var xmlExpensesReportToMsSql = new XmlVendorExprensesImport(ExpensesImportPath, sqlContext);
            var data = xmlExpensesReportToMsSql.Aaa();

            Console.WriteLine("Importing to SQL Server...");

            MsSqlManager.TransferData(data);

            Console.WriteLine("Vendor expense report imported.");
            Console.WriteLine(new string('-', 50));
        }
    }
}
