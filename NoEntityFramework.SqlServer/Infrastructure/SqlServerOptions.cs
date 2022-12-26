using NoEntityFramework.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoEntityFramework.SqlServer.Infrastructure;

namespace NoEntityFramework
{
    internal class SqlServerOptions<TDbContext> : ISqlServerOptions<TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        public SqlServerOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            ISqlServerLogger<TDbContext> logger,
            ISqlConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public ISqlServerLogger<TDbContext> Logger { get; }
        public ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }
    }
}