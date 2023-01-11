using Microsoft.Extensions.Options;
using System.Data;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    internal class PostgresConnectionFactory<TDbContext, TOption> 
        : IPostgresConnectionFactory<TDbContext, TOption>, IPostgresConnectionFactory
        where TOption : class, IDbContextOptions
        where TDbContext : class, IDbContext
    {
        private readonly string _sqlConnectionString;
        private readonly int _commandTimeout;

        public PostgresConnectionFactory(IOptionsMonitor<TOption> options)
        {
            var option = options.Get(typeof(TDbContext).ToString());
            _sqlConnectionString = option.ConnectionString;
            _commandTimeout = option.DbCommandTimeout;
        }

        public NpgsqlCommand CreateCommand()
        {
            return new NpgsqlCommand()
            {
                CommandTimeout = _commandTimeout
            };
        }

        public NpgsqlCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public NpgsqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;
        }

        public NpgsqlCommand CreateCommand(NpgsqlCommand command)
        {
            command.CommandTimeout = _commandTimeout;
            return command;
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_sqlConnectionString);
        }
        
        public NpgsqlDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }
    }
}
