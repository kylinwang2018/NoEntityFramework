using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     The base sql server database context.
    /// </summary>
    public abstract class SqlServerDbContext : ISqlServerDbContext
    {
        private readonly ISqlConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly ISqlServerLogger<SqlServerDbContext> _logger;

        /// <summary>
        ///     The constructor of <see cref="SqlServerDbContext"/>.
        /// </summary>
        /// <param name="sqlServerOptions">The instances which the database context required.</param>
        protected SqlServerDbContext(
            ISqlServerOptions<SqlServerDbContext> sqlServerOptions)
        {
            _connectionFactory = sqlServerOptions.ConnectionFactory;
            _logger = sqlServerOptions.Logger;
        }

        /// <summary>
        ///     Create a query with specific query text. May also specify the command type.
        /// </summary>
        /// <param name="commandText">
        ///     The text for this query.
        /// </param>
        /// <param name="commandType">
        ///     The <see cref="CommandType"/> for this query, specify this can speed up the query operation.
        /// </param>
        /// <returns>
        ///     A <see cref="ISqlServerQueryable"/> for following configuration and operation.
        /// </returns>
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

        /// <summary>
        ///     Create a query with specific query text. May also specify the command type.
        /// </summary>
        /// <param name="commandText">
        ///     The text for this query.
        /// </param>
        /// <returns>
        ///     A <see cref="ISqlServerQueryable"/> for following configuration and operation.
        /// </returns>
        public ISqlServerQueryable UseCommand(string commandText)
        {
            var queryable = new SqlServerQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(commandText));

            return queryable;
        }

        /// <summary>
        ///     Create a query with predefined <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="command">
        ///     The <see cref="SqlCommand"/> that already configured.
        /// </param>
        /// <returns>
        ///     A <see cref="ISqlServerQueryable"/> for following configuration and operation.
        /// </returns>
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
