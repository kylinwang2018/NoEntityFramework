using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoEntityFramework.Exceptions;

namespace NoEntityFramework.MongoDb
{
    internal class MongoDbConnectionFactory<TDbContext, TOption> : IMongoDbConnectionFactory<TDbContext, TOption>
        where TDbContext : class, IMongoDbContext
        where TOption : class, IMongoDbContextOptions
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public MongoDbConnectionFactory(IOptionsMonitor<TOption> options)
        {
            _connectionString = options.Get(typeof(TDbContext).ToString()).ConnectionString;
            _databaseName = options.Get(typeof(TDbContext).ToString()).DatabaseName;
        }

        public IMongoClient CreateClient()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new DatabaseException("Connection string cannot be empty.");

            return new MongoClient(_connectionString);
        }

        public IMongoDatabase ConnectDatabase(IMongoClient mongoClient)
        {
            if (string.IsNullOrWhiteSpace(_databaseName))
                throw new DatabaseException("Database Name cannot be empty.");

            return mongoClient.GetDatabase(_databaseName);
        }

        public IMongoDatabase ConnectDatabase()
        {
            var client = CreateClient();
            return ConnectDatabase(client);
        }
    }
}
