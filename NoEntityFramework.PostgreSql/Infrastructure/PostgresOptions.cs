using System;
using Microsoft.Extensions.Options;
using NoEntityFramework.Npgsql.Models;

namespace NoEntityFramework.Npgsql
{
    internal class PostgresOptions<TDbContext> : IPostgresOptions<TDbContext>
        where TDbContext : class, IPostgresDbContext
    {
        public PostgresOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            IPostgresLogger<TDbContext> logger,
            IPostgresConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
            RetryLogicOption = new NpgsqlRetryLogicOption()
            {
                NumberOfTries = options.Get(typeof(TDbContext).ToString()).NumberOfTries,
                DeltaTime = TimeSpan.FromSeconds(options.Get(typeof(TDbContext).ToString()).DeltaTime)
            };
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public IPostgresLogger<TDbContext> Logger { get; }
        public IPostgresConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        public NpgsqlRetryLogicOption RetryLogicOption { get; }
    }
}