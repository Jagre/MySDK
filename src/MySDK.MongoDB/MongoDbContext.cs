using MongoDB.Driver;

namespace MySDK.MongoDB
{
    public abstract class MongoDbContext
    {
        protected IMongoDatabase DB { get; private set; }

        public MongoDbContext(string connectionString)
        {
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