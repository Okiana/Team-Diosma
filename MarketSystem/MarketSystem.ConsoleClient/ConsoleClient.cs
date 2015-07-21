using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using MarketSystem.Models;

namespace MarketSystem.ConsoleClient
{
    using System;
    using MsSqlDatabase;
    using OracleDatabase;

    class ConsoleClient
    {
        public static void Main()
        {
            //var oracleManager = new OracleManager();
            //var data = oracleManager.GetData();

            //Console.Write(data);

            //MsSqlManager.TransferData(data);
            var context = new SqlMarketContext();
            Console.WriteLine(context.VendorExpenses.Count());


            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; ;
           

            var xmlDoc = XDocument.Load("../../Sample-Vendor-Expenses.xml");
            var vendorNamesList = xmlDoc.Root.Elements("vendor");

            foreach (var vendorElement in vendorNamesList)
            {
                

                var vendorName = vendorElement.Attribute("name").Value;
                var vendorId = context.Vendors.Where(u => u.Name == vendorName).Select(u => u.Id).FirstOrDefault();



                var monthExpenses = vendorElement.Elements("expenses");
                foreach (var monthExpense in monthExpenses)
                {

                    var dt = monthExpense.Attribute("month").Value;
                    var parsedDatetime = DateTime.Parse(dt);
                   
                    //Console.WriteLine(parsedDate);
                    //System.Console.WriteLine(parsedDate);
                    var expense = monthExpense.Value;
                    //System.Console.WriteLine("month: {0} - {1}", month, expense);
                    //System.Console.WriteLine(month);

                    var vendorExpense = new VendorExpense();
                    vendorExpense.VendorId = vendorId;
                    vendorExpense.Month =parsedDatetime;
                    //System.Console.WriteLine(DateTime.Parse(month));
                    vendorExpense.Expenses = decimal.Parse(expense);

                    context.VendorExpenses.Add(vendorExpense);

                }

            }
            context.SaveChanges();
        }
    }
}