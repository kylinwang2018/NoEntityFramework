using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     The instances which the database context required.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    public interface IMySqlOptions<out TDbContext>
        where TDbContext : class, IMySqlDbContext
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="MySqlConnection"/>,
        /// <see cref="MySqlCommand"/> and <see cref="MySqlDataAdapter"/>.
        /// </summary>
        IMySqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        IMySqlLogger<TDbContext> Logger { get; }

        /// <summary>
        ///     Provide options for this database context.
        /// </summary>
        IOptionsMonitor<RelationalDbOptions> Options { get; }

        /// <summary>
        ///     Provide a retry logic for the query.
        /// </summary>
        MySqlRetryLogicOption RetryLogicOption { get; }
    }
}