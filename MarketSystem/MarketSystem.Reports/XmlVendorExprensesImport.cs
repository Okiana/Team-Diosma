namespace MarketSystem.Reports
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Models;
    using MsSqlDatabase;

    public class XmlVendorExprensesImport : MarketData
    {
        public XmlVendorExprensesImport(string expensesImportPath, SqlMarketContext context)
        {
            this.ExpensesImportPath = expensesImportPath;
            this.SqlMarketContext = context;
        }

        public string ExpensesImportPath { get; set; }

        public SqlMarketContext SqlMarketContext { get; set; }

        public MarketData GetReportData()
        {
            var xmlDoc = XDocument.Load(this.ExpensesImportPath);
            var vendorNamesList = xmlDoc.Root.Elements("vendor");

            foreach (var vendorElement in vendorNamesList)
            {
                var vendorName = vendorElement.Attribute("name").Value;
                var vendorId = this.SqlMarketContext.Vendors
                    .Where(u => u.Name == vendorName)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                var monthExpenses = vendorElement.Elements("expenses");
                foreach (var monthExpense in monthExpenses)
                {
                    var dt = monthExpense.Attribute("month").Value;
                    var parsedDatetime = DateTime.Parse(dt);
                    var expense = monthExpense.Value;       
         
                    var vendorExpense = new VendorExpense
                    {
                        VendorId = vendorId,
                        Month = parsedDatetime,
                        Expenses = decimal.Parse(expense)
                    };

                    this.VendorExpenses.Add(vendorExpense);
                }
            }

            return this;
        }
    }
}