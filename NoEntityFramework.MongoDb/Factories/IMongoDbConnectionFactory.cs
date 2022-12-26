using MongoDB.Driver;

namespace NoEntityFramework.MongoDb
{
    public interface IMongoDbConnectionFactory<out TDbContext, TOption>
        where TDbContext : class, IMongoDbContext
        where TOption : class, IMongoDbContextOptions
    {
        IMongoDatabase ConnectDatabase();
        IMongoDatabase ConnectDatabase(IMongoClient mongoClient);
        IMongoClient CreateClient();
    }
}