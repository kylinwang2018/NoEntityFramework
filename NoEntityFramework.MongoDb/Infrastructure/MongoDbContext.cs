using NoEntityFramework.MongoDb;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace NoEntityFramework
{
    public class MongoDbContext : IMongoDbContext
    {
        protected readonly MongoDbContextOptions _options;
        protected readonly IMongoDbConnectionFactory<MongoDbContext, MongoDbContextOptions> _connectionFactory;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(
            IMongoDbOptions<MongoDbContext> mongoDbOptions)
        {
            _options = mongoDbOptions.Options.Get(GetType().ToString());
            _connectionFactory = mongoDbOptions.ConnectionFactory;
            _logger = mongoDbOptions.Logger;
        }

        public MongoDbContextOptions Options => _options;

        public IMongoDatabase Database => _connectionFactory.ConnectDatabase();
    }
}
