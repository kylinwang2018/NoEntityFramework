using System.Data;
using NoEntityFramework.Npgsql.Models;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     The base Postgres database context.
    /// </summary>
    public abstract class PostgresDbContext : IPostgresDbContext
    {
        private readonly IPostgresConnectionFactory<IDbContext, RelationalDbOptions> _connectionFactory;
        private readonly IPostgresLogger<PostgresDbContext> _logger;
        private readonly NpgsqlRetryLogicOption _retryOptions;

        /// <summary>
        ///     The constructor of <see cref="PostgresDbContext"/>.
        /// </summary>
        /// <param name="postgresOptions">The instances which the database context required.</param>
        protected PostgresDbContext(
            IPostgresOptions<PostgresDbContext> postgresOptions)
        {
            _connectionFactory = postgresOptions.ConnectionFactory;
            _logger = postgresOptions.Logger;
            _retryOptions = postgresOptions.RetryLogicOption;
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
        ///     A <see cref="IPostgresQueryable"/> for following configuration and operation.
        /// </returns>
        public IPostgresQueryable UseCommand(string commandText, CommandType? commandType)
        {
            var queryable = new PostgresQueryable(
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
        ///     A <see cref="IPostgresQueryable"/> for following configuration and operation.
        /// </returns>
        public IPostgresQueryable UseCommand(string commandText)
        {
            var queryable = new PostgresQueryable(
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
        ///     A <see cref="IPostgresQueryable"/> for following configuration and operation.
        /// </returns>
        public IPostgresQueryable UseCommand(NpgsqlCommand command)
        {
            var queryable = new PostgresQueryable(
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
