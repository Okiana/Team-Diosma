namespace MarketSystem.XmlExpensesImport
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Models;
    using MsSqlDatabase;

    public class XmlVendorExprensesImport: MarketData
    {
        public XmlVendorExprensesImport(string expensesImportPath, SqlMarketContext context)
        {
            this.ExpensesImportPath = expensesImportPath;
            this.SqlMarketContext = context;
        }

        public string ExpensesImportPath { get; set; }

        public SqlMarketContext SqlMarketContext { get; set; }

        public MarketData Aaa()
        {
            var xmlDoc = XDocument.Load(ExpensesImportPath);
            var vendorNamesList = xmlDoc.Root.Elements("vendor");

            foreach (var vendorElement in vendorNamesList)
            {
                var vendorName = vendorElement.Attribute("name").Value;
                var vendorId = this.SqlMarketContext.Vendors.Where(u => u.Name == vendorName).Select(u => u.Id).FirstOrDefault();

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

                    var vendorExpense = new VendorExpense
                    {
                        VendorId = vendorId,
                        Month = parsedDatetime,
                        Expenses = decimal.Parse(expense)
                    };

                    this.VendorExpenses.Add(vendorExpense);

                    //var vendorExpense = new VendorExpense();
                    //vendorExpense.VendorId = vendorId;
                    //vendorExpense.Month = parsedDatetime;
                    //System.Console.WriteLine(DateTime.Parse(month));
                    //vendorExpense.Expenses = decimal.Parse(expense);

                    //sqlContext.VendorExpenses.Add(vendorExpense);

                }

            }

            return this;
        }
    }
}
