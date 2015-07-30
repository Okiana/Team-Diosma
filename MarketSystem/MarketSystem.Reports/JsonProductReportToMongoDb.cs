namespace MarketSystem.Reports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;

    using MarketSystem.MsSqlDatabase;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class JsonProductReportToMongoDb
    {
        public static void JsonProductSalses(DateTime startDate, DateTime endDate, string path)
        {
            var context = new SqlMarketContext();
            var productSalesInfo = context.Sales
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .Select(s => new
                {
                    s.ProductId,
                    Product = s.Product.Name,
                    Vendor = s.Product.Vendor.Name,
                    TotalQuantitySold = context.Sales
                            .Where(prId => prId.ProductId == s.ProductId)
                            .Sum(prSum => prSum.Quantity),
                    TotalIncomes = context.Sales
                            .Where(prId => prId.ProductId == s.ProductId)
                            .Sum(prSum => prSum.TotalSum)
                });


            foreach (var salesReport in productSalesInfo)
            {
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(salesReport);
                File.WriteAllText(path + salesReport.ProductId + ".json", json);
            }
        }

        public static void ImportJsonToMongoDb()
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("ProductsSales");
            var collection = database.GetCollection<BsonDocument>("ProductsReports");

            var jsonFiles = Directory.GetFiles("../../../../Export");

            foreach (var json in jsonFiles)
            {
                Console.WriteLine();
                var readJsonFile = File.ReadAllText(json);
                var document = BsonDocument.Parse(readJsonFile);
                collection.InsertOneAsync(document).Wait();
            }
        }
    }
}