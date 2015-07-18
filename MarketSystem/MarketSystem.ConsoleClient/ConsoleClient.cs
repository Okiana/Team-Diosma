namespace MarketSystem.ConsoleClient
{
    using System;
    using MsSqlDatabase;
    using OracleDatabase;

    class ConsoleClient
    {
        public static void Main()
        {
            var oracleManager = new OracleManager();
            var data = oracleManager.GetData();

            Console.Write(data);

            MsSqlManager.TransferData(data);

            
        }
    }
}