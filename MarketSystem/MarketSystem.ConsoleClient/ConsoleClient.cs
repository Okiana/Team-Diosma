namespace MarketSystem.ConsoleClient
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Engine;

    class ConsoleClient
    {
        public static void Main()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                const string menu = "Menu:\n" +
                    "1. Transfer data from Oracle Database to MsSql Database\n" +
                    "2. Load Zip Excel Reports to MsSql Database\n" +
                    "4. Generate XML Sales by Vendor\n" +
                    "6. Load Xml Vendors Expenses Report to MsSql Database\n" +
                    "0. Exit\n";

                Console.WriteLine(menu);
                var menuChoise = int.Parse(Console.ReadLine());

                switch (menuChoise)
                {
                    case 0: break;
                    case 1: Engine.OracleToMsSqlTransfer();
                        break;
                    case 2: Engine.ZipExcelReportsToMsSql();
                        break;
                    case 4: Engine.XMLExport();
                        break;
                    case 6: Engine.XmlExpensesReportToMsSql();
                        break;
                    default: throw new InvalidOperationException("Invalid operation.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}