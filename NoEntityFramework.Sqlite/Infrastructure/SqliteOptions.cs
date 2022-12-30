using Microsoft.Extensions.Options;

namespace NoEntityFramework.Sqlite
{
    internal class SqliteOptions<TDbContext> : ISqliteOptions<TDbContext>
        where TDbContext : class, ISqliteDbContext
    {
        public SqliteOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            ISqliteLogger<TDbContext> logger,
            ISqlConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public ISqliteLogger<TDbContext> Logger { get; }
        public ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
    }
}