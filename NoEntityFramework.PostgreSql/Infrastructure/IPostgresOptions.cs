using Microsoft.Extensions.Options;
using NoEntityFramework.Npgsql.Models;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     The instances which the database context required.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    public interface IPostgresOptions<out TDbContext>
        where TDbContext : class, IPostgresDbContext
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="NpgsqlConnection"/>,
        /// <see cref="NpgsqlCommand"/> and <see cref="NpgsqlDataAdapter"/>.
        /// </summary>
        IPostgresConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        IPostgresLogger<TDbContext> Logger { get; }

        /// <summary>
        ///     Provide options for this database context.
        /// </summary>
        IOptionsMonitor<RelationalDbOptions> Options { get; }

        /// <summary>
        ///     Provide a retry logic for the query.
        /// </summary>
        NpgsqlRetryLogicOption RetryLogicOption { get; }
    }
}