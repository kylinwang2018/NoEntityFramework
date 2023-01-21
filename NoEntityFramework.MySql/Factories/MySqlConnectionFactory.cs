using Microsoft.Extensions.Options;
using System.Data;
using MySql.Data.MySqlClient;

namespace NoEntityFramework.MySql
{
    internal class MySqlConnectionFactory<TDbContext, TOption> 
        : IMySqlConnectionFactory<TDbContext, TOption>, IMySqlConnectionFactory
        where TOption : class, IDbContextOptions
        where TDbContext : class, IDbContext
    {
        private readonly string _sqlConnectionString;
        private readonly int _commandTimeout;

        public MySqlConnectionFactory(IOptionsMonitor<TOption> options)
        {
            var option = options.Get(typeof(TDbContext).ToString());
            _sqlConnectionString = option.ConnectionString;
            _commandTimeout = option.DbCommandTimeout;
        }

        public MySqlCommand CreateCommand()
        {
            return new MySqlCommand()
            {
                CommandTimeout = _commandTimeout
            };
        }

        public MySqlCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public MySqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        public MySqlCommand CreateCommand(MySqlCommand command)
        {
            command.CommandTimeout = _commandTimeout;
            return command;
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_sqlConnectionString);
        }
        
        public MySqlDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
