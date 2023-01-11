using System.Data;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Provide a connection factory for create <see cref="NpgsqlConnection"/>,
    /// <see cref="NpgsqlCommand"/> and <see cref="NpgsqlDataAdapter"/>. 
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    /// <typeparam name="TOption">
    ///     The instances which the database context required.
    /// </typeparam>
    public interface IPostgresConnectionFactory<out TDbContext, TOption> : IPostgresConnectionFactory
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {

    }

    /// <summary>
    ///     Provide a connection factory for create <see cref="NpgsqlConnection"/>,
    /// <see cref="NpgsqlCommand"/> and <see cref="NpgsqlDataAdapter"/>.
    /// </summary>
    public interface IPostgresConnectionFactory
    {
        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <returns>A new <see cref="NpgsqlConnection"/> object</returns>
        NpgsqlConnection CreateConnection();

        /// <summary>
        /// Creates a sql command for a Postgres connection
        /// </summary>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand();

        /// <summary>
        /// Creates a sql command with specified command text for a Postgres connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand(string commandText);

        /// <summary>
        ///     Creates a sql command with specified <see cref="NpgsqlCommand"/> for a Postgres connection
        /// </summary>
        /// <param name="command">
        ///     A <see cref="NpgsqlCommand"/> object
        /// </param>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand(NpgsqlCommand command);

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="NpgsqlCommand"/> object</returns>
        NpgsqlCommand CreateCommand(string commandText, CommandType commandType);
        
        /// <summary>
        /// Creates a sql data adapter for Postgres connection
        /// </summary>
        /// <returns>A new <see cref="NpgsqlDataAdapter"/> object</returns>
        NpgsqlDataAdapter CreateDataAdapter();
    }
}
