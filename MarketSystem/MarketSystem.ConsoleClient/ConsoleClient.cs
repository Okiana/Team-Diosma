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
                    "1. Replicate data from Oracle Database into MsSql Database\n" +
                    "2. Load Zip Excel Reports to MsSql Database\n" +
                    "3. Generate Pdf Sales report\n" +
                    "4. Generate XML Sales by Vendor from given date range\n" +
                    "6. Load Xml Vendors Expenses Report into MsSql Database\n" +
                    "7. Load data from MsSql to MySql\n" +
                    "8. Generate financial report\n" +
                    "9. Exit\n";
            
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                var isFinished = false;

                while (!isFinished)
                {
                    Console.WriteLine(Menu);
                    var menuChoise = int.Parse(Console.ReadLine());

                    switch (menuChoise)
                    {
                        case 1: Engine.OracleToMsSqlTransfer();
                            break;
                        case 2: Engine.ZipExcelReportsToMsSql();
                            break;
                        case 3: Engine.GeneratePdfSalesReport();
                            break;
                        case 4: Engine.XMLExport();
                            break;
                        case 6: Engine.XmlExpensesReportToMsSql();
                            break;
                        case 7: Engine.SqlServerToMySqlTransfer();
                            break;
                        case 8: Engine.GenerateFinancialReport();
                            break;
                        case 9: isFinished = true;
                            break;
                        default: throw new InvalidOperationException("Invalid operation.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}