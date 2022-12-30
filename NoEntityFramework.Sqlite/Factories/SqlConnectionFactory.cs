using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    internal class SqlConnectionFactory<TDbContext, TOption> 
        : ISqlConnectionFactory<TDbContext, TOption>, ISqlConnectionFactory
        where TOption : class, IDbContextOptions
        where TDbContext : class, IDbContext
    {
        private readonly string _sqlConnectionString;
        private readonly int _commandTimeout;

        public SqlConnectionFactory(IOptionsMonitor<TOption> options)
        {
            var _options = options.Get(typeof(TDbContext).ToString());
            _sqlConnectionString = _options.ConnectionString;
            _commandTimeout = _options.DbCommandTimeout;
        }

        public SqliteCommand CreateCommand()
        {
            return new SqliteCommand()
            {
                CommandTimeout = _commandTimeout
            };
        }

        public SqliteCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public SqliteCommand CreateCommand(string commandText, CommandType commandType)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        public SqliteCommand CreateCommand(SqliteCommand command)
        {
            command.CommandTimeout = _commandTimeout;
            return command;
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_sqlConnectionString);
        }
        
        public SqliteDataAdapter CreateDataAdapter()
        {
            return new SqliteDataAdapter();
        }
    }
}
