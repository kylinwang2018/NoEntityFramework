using System.Data;
using Microsoft.Data.Sqlite;
using NoEntityFramework.Sqlite.Models;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     The base sqlite database context.
    /// </summary>
    public abstract class SqliteDbContext : ISqliteDbContext
    {
        private readonly ISqliteConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly ISqliteLogger<SqliteDbContext> _logger;
        private readonly SqliteRetryLogicOption _retryOptions;

        /// <summary>
        ///     The constructor of <see cref="SqliteDbContext"/>.
        /// </summary>
        /// <param name="sqliteOptions">The instances which the database context required.</param>
        protected SqliteDbContext(
            ISqliteOptions<SqliteDbContext> sqliteOptions)
        {
            _connectionFactory = sqliteOptions.ConnectionFactory;
            _logger = sqliteOptions.Logger;
            _retryOptions = sqliteOptions.RetryLogicOption;
            _retryOptions.Logger = _logger;
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
        ///     A <see cref="ISqliteQueryable"/> for following configuration and operation.
        /// </returns>
        public ISqliteQueryable UseCommand(string commandText, CommandType? commandType)
        {
            var queryable = new SqliteQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(commandText))
            {
                RetryLogicOption = _retryOptions
            };
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
        ///     A <see cref="ISqliteQueryable"/> for following configuration and operation.
        /// </returns>
        public ISqliteQueryable UseCommand(string commandText)
        {
            var queryable = new SqliteQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(commandText))
            {
                RetryLogicOption = _retryOptions
            };
            return queryable;
        }

        /// <summary>
        ///     Create a query with predefined <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="command">
        ///     The <see cref="SqliteCommand"/> that already configured.
        /// </param>
        /// <returns>
        ///     A <see cref="ISqliteQueryable"/> for following configuration and operation.
        /// </returns>
        public ISqliteQueryable UseCommand(SqliteCommand command)
        {
            var queryable = new SqliteQueryable(
                _connectionFactory, _logger,
                _connectionFactory.CreateConnection(),
                _connectionFactory.CreateCommand(command))
            {
                RetryLogicOption = _retryOptions
            };
            return queryable;
        }
    }
}
