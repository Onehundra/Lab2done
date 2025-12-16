using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace Lab2done
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:DatabaseName"];

            var db = new MongoDbService(connectionString, databaseName);



            ShopManager shop = new ShopManager(db);
            shop.RunStartMenu();


        }
    }
}
