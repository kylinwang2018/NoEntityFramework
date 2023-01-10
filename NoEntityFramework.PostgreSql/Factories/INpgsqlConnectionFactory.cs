using Npgsql;

namespace NoEntityFramework.Npgsql
{
    public interface INpgsqlConnectionFactory<out TDbContext, TOption>
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {
        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <returns>A new <see cref="NpgsqlConnection"/> object</returns>
        NpgsqlConnection CreateConnection();

        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <param name="connectionString">the connection string that can access the database</param>
        /// <returns>A new <see cref="NpgsqlConnection"/> object</returns>
        NpgsqlConnection CreateConnection(string connectionString);

        /// <summary>
        /// Creates a sql command for a sql connection
        /// </summary>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand();

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="query">the command text</param>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand(string query);

        /// <summary>
        /// Creates a sql data adapter for sql connection
        /// </summary>
        /// <returns>A new <see cref="NpgsqlDataAdapter"/> object</returns>
        NpgsqlDataAdapter CreateDataAdapter();
    }
}
