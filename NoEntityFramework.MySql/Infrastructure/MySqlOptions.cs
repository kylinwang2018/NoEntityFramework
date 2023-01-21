using System;
using Microsoft.Extensions.Options;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    internal class MySqlOptions<TDbContext> : IMySqlOptions<TDbContext>
        where TDbContext : class, IMySqlDbContext
    {
        public MySqlOptions(
            IOptionsMonitor<RelationalDbOptions> options,
            IMySqlLogger<TDbContext> logger,
            IMySqlConnectionFactory<TDbContext, RelationalDbOptions> connectionFactory)
        {
            Options = options;
            Logger = logger;
            ConnectionFactory = connectionFactory;
            RetryLogicOption = new MySqlRetryLogicOption()
            {
                NumberOfTries = options.Get(typeof(TDbContext).ToString()).NumberOfTries,
                DeltaTime = TimeSpan.FromSeconds(options.Get(typeof(TDbContext).ToString()).DeltaTime)
            };
        }

        public IOptionsMonitor<RelationalDbOptions> Options { get; }
        public IMySqlLogger<TDbContext> Logger { get; }
        public IMySqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        public MySqlRetryLogicOption RetryLogicOption { get; }
    }
}