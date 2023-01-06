using Microsoft.Data.SqlClient;
using System.Data;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Provide a connection factory for create <see cref="SqlConnection"/>,
    /// <see cref="SqlCommand"/> and <see cref="SqlDataAdapter"/>. 
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    /// <typeparam name="TOption">
    ///     The instances which the database context required.
    /// </typeparam>
    public interface ISqlConnectionFactory<out TDbContext, TOption> : ISqlConnectionFactory
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {

    }

    /// <summary>
    ///     Provide a connection factory for create <see cref="SqlConnection"/>,
    /// <see cref="SqlCommand"/> and <see cref="SqlDataAdapter"/>.
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <returns>A new <see cref="SqlConnection"/> object</returns>
        SqlConnection CreateConnection();

        /// <summary>
        /// Creates a sql command for a sql connection
        /// </summary>
        /// <returns>A new <see cref="SqlCommand"/> object</returns>
        SqlCommand CreateCommand();

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="SqlCommand"/> object</returns>
        SqlCommand CreateCommand(string commandText);

        /// <summary>
        ///     Creates a sql command with specified <see cref="SqlCommand"/> for a sql connection
        /// </summary>
        /// <param name="command">
        ///     A <see cref="SqlCommand"/> object
        /// </param>
        /// <returns>A new <see cref="SqlCommand"/> object</returns>
        SqlCommand CreateCommand(SqlCommand command);

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="SqlCommand"/> object</returns>
        SqlCommand CreateCommand(string commandText, CommandType commandType);

        /// <summary>
        /// Creates a sql data adapter for sql connection
        /// </summary>
        /// <returns>A new <see cref="SqlDataAdapter"/> object</returns>
        SqlDataAdapter CreateDataAdapter();
    }
}
