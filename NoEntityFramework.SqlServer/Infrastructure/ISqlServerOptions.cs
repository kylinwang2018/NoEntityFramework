using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     The instances which the database context required.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    public interface ISqlServerOptions<out TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="SqlConnection"/>,
        /// <see cref="SqlCommand"/> and <see cref="SqlDataAdapter"/>.
        /// </summary>
        ISqlConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        ISqlServerLogger<TDbContext> Logger { get; }

        /// <summary>
        ///     Provide options for this database context.
        /// </summary>
        IOptionsMonitor<RelationalDbOptions> Options { get; }
    }
}