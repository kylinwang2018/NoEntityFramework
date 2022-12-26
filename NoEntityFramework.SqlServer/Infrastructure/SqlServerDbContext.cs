using Microsoft.Data.SqlClient;
using NoEntityFramework.SqlServer;
using NoEntityFramework.SqlServer.Infrastructure;
using System.Data;

namespace NoEntityFramework
{
    public class SqlServerDbContext : ISqlServerDbContext
    {
        private readonly ISqlConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly ISqlServerLogger<SqlServerDbContext> _logger;

        public SqlServerDbContext(
            ISqlServerOptions<SqlServerDbContext> sqlServerOptions)
        {
            _connectionFactory = sqlServerOptions.ConnectionFactory;
            _logger = sqlServerOptions.Logger;
        }

        public ISqlServerQueryable UseCommand(string commandText, CommandType? commandType)
        {
            var queryable = new SqlServerQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(commandText));
            if (commandType != null)
                queryable.SqlCommand.CommandType = (CommandType)commandType;

            return queryable;
        }

        public ISqlServerQueryable UseCommand(SqlCommand command)
        {
            var queryable = new SqlServerQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(command));

            return queryable;
        }
    }
}
