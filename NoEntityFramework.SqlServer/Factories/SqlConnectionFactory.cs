using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace NoEntityFramework.SqlServer
{
    internal class SqlConnectionFactory<TDbContext, TOption> 
        : ISqlConnectionFactory<TDbContext, TOption>, ISqlConnectionFactory
        where TOption : class, IDbContextOptions
        where TDbContext : class, IDbContext
    {
        private readonly string _sqlConnectionString;
        private readonly bool _statisticsEnabled;
        private readonly int _commandTimeout;
        private readonly SqlRetryLogicBaseProvider _sqlRetryProvider;

        public SqlConnectionFactory(IOptionsMonitor<TOption> options)
        {
            var _options = options.Get(typeof(TDbContext).ToString());
            _sqlConnectionString = _options.ConnectionString;
            _statisticsEnabled = _options.EnableStatistics;
            _commandTimeout = _options.DbCommandTimeout;
            _sqlRetryProvider = SqlConfigurableRetryFactory
                .CreateExponentialRetryProvider(new SqlRetryLogicOption()
                {
                    NumberOfTries = _options.NumberOfTries,
                    DeltaTime = TimeSpan.FromSeconds(_options.DeltaTime),
                    MaxTimeInterval = TimeSpan.FromSeconds(_options.MaxTimeInterval)
                });
        }

        public SqlCommand CreateCommand()
        {
            return new SqlCommand()
            {
                RetryLogicProvider = _sqlRetryProvider,
                CommandTimeout = _commandTimeout
            };
        }

        public SqlCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public SqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        public SqlCommand CreateCommand(SqlCommand command)
        {
            command.RetryLogicProvider = _sqlRetryProvider;
            command.CommandTimeout = _commandTimeout;
            return command;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_sqlConnectionString)
            {
                RetryLogicProvider = _sqlRetryProvider,
                StatisticsEnabled = _statisticsEnabled
            };
        }

        public SqlDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
