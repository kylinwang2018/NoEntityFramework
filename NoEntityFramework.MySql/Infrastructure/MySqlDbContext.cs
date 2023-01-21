using System.Data;
using MySql.Data.MySqlClient;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     The base MySql database context.
    /// </summary>
    public abstract class MySqlDbContext : IMySqlDbContext
    {
        private readonly IMySqlConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly IMySqlLogger<MySqlDbContext> _logger;
        private readonly MySqlRetryLogicOption _retryOptions;

        /// <summary>
        ///     The constructor of <see cref="MySqlDbContext"/>.
        /// </summary>
        /// <param name="mySqlOptions">The instances which the database context required.</param>
        protected MySqlDbContext(
            IMySqlOptions<MySqlDbContext> mySqlOptions)
        {
            _connectionFactory = mySqlOptions.ConnectionFactory;
            _logger = mySqlOptions.Logger;
            _retryOptions = mySqlOptions.RetryLogicOption;
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
        ///     A <see cref="IMySqlQueryable"/> for following configuration and operation.
        /// </returns>
        public IMySqlQueryable UseCommand(string commandText, CommandType? commandType)
        {
            var queryable = new MySqlQueryable(
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
        ///     A <see cref="IMySqlQueryable"/> for following configuration and operation.
        /// </returns>
        public IMySqlQueryable UseCommand(string commandText)
        {
            var queryable = new MySqlQueryable(
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
        ///     A <see cref="IMySqlQueryable"/> for following configuration and operation.
        /// </returns>
        public IMySqlQueryable UseCommand(MySqlCommand command)
        {
            var queryable = new MySqlQueryable(
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
