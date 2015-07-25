namespace MarketSystem.ConsoleClient
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Engine;

    public class ConsoleClient
    {
        public static void Main()
        {
            const string Menu = "Menu:\n" +
                    "1. Transfer data from Oracle Database to MsSql Database\n" +
                    "2. Load Zip Excel Reports to MsSql Database\n" +
                    "4. Generate XML Sales by Vendor\n" +
                    "6. Load Xml Vendors Expenses Report to MsSql Database\n" +
                    "0. Exit\n";

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                Console.WriteLine(Menu);
                var menuChoise = int.Parse(Console.ReadLine());

                switch (menuChoise)
                {
                    case 0: 
                        return;
                    case 1: Engine.OracleToMsSqlTransfer();
                        break;
                    case 2: Engine.ZipExcelReportsToMsSql();
                        break;
                    case 4: Engine.XMLExport();
                        break;
                    case 6: Engine.XmlExpensesReportToMsSql();
                        break;
                    default: throw new InvalidOperationException("Invalid operation.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}