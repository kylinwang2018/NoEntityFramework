using System.Data;
using MySql.Data.MySqlClient;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     Provide a connection factory for create <see cref="MySqlConnection"/>,
    /// <see cref="MySqlCommand"/> and <see cref="MySqlDataAdapter"/>. 
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    /// <typeparam name="TOption">
    ///     The instances which the database context required.
    /// </typeparam>
    public interface IMySqlConnectionFactory<out TDbContext, TOption> : IMySqlConnectionFactory
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {

    }

    /// <summary>
    ///     Provide a connection factory for create <see cref="MySqlConnection"/>,
    /// <see cref="MySqlCommand"/> and <see cref="MySqlDataAdapter"/>.
    /// </summary>
    public interface IMySqlConnectionFactory
    {
        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <returns>A new <see cref="MySqlConnection"/> object</returns>
        MySqlConnection CreateConnection();

        /// <summary>
        /// Creates a sql command for a MySql connection
        /// </summary>
        /// <returns>A new <see cref="MySqlCommand"/> object</returns>
        MySqlCommand CreateCommand();

        /// <summary>
        /// Creates a sql command with specified command text for a MySql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="MySqlCommand"/> object</returns>
        MySqlCommand CreateCommand(string commandText);

        /// <summary>
        ///     Creates a sql command with specified <see cref="MySqlCommand"/> for a MySql connection
        /// </summary>
        /// <param name="command">
        ///     A <see cref="MySqlCommand"/> object
        /// </param>
        /// <returns>A new <see cref="MySqlCommand"/> object</returns>
        MySqlCommand CreateCommand(MySqlCommand command);

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="MySqlCommand"/> object</returns>
        MySqlCommand CreateCommand(string commandText, CommandType commandType);
        
        /// <summary>
        /// Creates a sql data adapter for MySql connection
        /// </summary>
        /// <returns>A new <see cref="MySqlDataAdapter"/> object</returns>
        MySqlDataAdapter CreateDataAdapter();
    }
}
