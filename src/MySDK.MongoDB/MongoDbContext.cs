using MongoDB.Driver;
using MySDK.Configuration;
using MySDK.DependencyInjection;

namespace MySDK.MongoDB
{
    public class MongoDbContext
    {
        protected IMongoDatabase DB { get; private set; }

        public MongoDbContext(string connectionName)
        {
            var connectionString = MyServiceProvider.Configuration.GetConnectionString(connectionName);
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            DB = client.GetDatabase(url.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return DB.GetCollection<T>(typeof(T).Name);
        }
    }
}