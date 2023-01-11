using System;
using Microsoft.Extensions.Options;
using NoEntityFramework.Sqlite.Models;

namespace NoEntityFramework.Sqlite
{
    internal class SqliteOptions<TDbContext> : ISqliteOptions<TDbContext>
        where TDbContext : class, ISqliteDbContext
    {
        public SqliteOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            ISqliteLogger<TDbContext> logger,
            ISqliteConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
            RetryLogicOption = new SqliteRetryLogicOption()
            {
                NumberOfTries = options.Get(typeof(TDbContext).ToString()).NumberOfTries,
                DeltaTime = TimeSpan.FromSeconds(options.Get(typeof(TDbContext).ToString()).DeltaTime)
            };
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public ISqliteLogger<TDbContext> Logger { get; }
        public ISqliteConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        public SqliteRetryLogicOption RetryLogicOption { get; }
    }
}