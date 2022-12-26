using NoEntityFramework.PostgresSQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NoEntityFramework
{
    internal class NpgsqlDbOptions<TDbContext> : INpgsqlDbOptions<TDbContext> where TDbContext : class, INpgsqlDbContext
    {
        public NpgsqlDbOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            ILogger<TDbContext> logger,
            INpgsqlConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public ILogger<TDbContext> Logger { get; }
        public INpgsqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

    }
}