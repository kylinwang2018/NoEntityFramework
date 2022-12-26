using NoEntityFramework.MongoDb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NoEntityFramework
{
    internal class MongoDbOptions<TDbContext> : IMongoDbOptions<TDbContext> where TDbContext : class, IMongoDbContext
    {
        public MongoDbOptions(
            IOptionsMonitor<MongoDbContextOptions> options,
            IMongoDbConnectionFactory<TDbContext, MongoDbContextOptions> connectionFactory,
            ILogger<TDbContext> logger
            )
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
        }

        public IOptionsMonitor<MongoDbContextOptions> Options { get; }
        public ILogger<TDbContext> Logger { get; }
        public IMongoDbConnectionFactory<TDbContext, MongoDbContextOptions> ConnectionFactory { get; }
    }
}