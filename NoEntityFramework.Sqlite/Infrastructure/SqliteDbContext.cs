using System.Data;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    public class SqliteDbContext : ISqliteDbContext
    {
        private readonly ISqlConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly ISqliteLogger<SqliteDbContext> _logger;

        public SqliteDbContext(
            ISqliteOptions<SqliteDbContext> sqlServerOptions)
        {
            _connectionFactory = sqlServerOptions.ConnectionFactory;
            _logger = sqlServerOptions.Logger;
        }

        public ISqliteQueryable UseCommand(string commandText, CommandType? commandType)
        {
            var queryable = new SqliteQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(commandText));
            if (commandType != null)
                queryable.SqlCommand.CommandType = (CommandType)commandType;

            return queryable;
        }

        public ISqliteQueryable UseCommand(SqliteCommand command)
        {
            var queryable = new SqliteQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(command));

            return queryable;
        }
    }
}
