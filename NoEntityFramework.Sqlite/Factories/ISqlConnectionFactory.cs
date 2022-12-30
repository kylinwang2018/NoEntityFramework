using System.Data;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    public interface ISqlConnectionFactory<out TDbContext, TOption> : ISqlConnectionFactory
        where TDbContext : class, IDbContext
        where TOption : class, IDbContextOptions
    {

    }

    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates a database connection to the specified database.
        /// </summary>
        /// <returns>A new <see cref="SqliteConnection"/> object</returns>
        SqliteConnection CreateConnection();

        /// <summary>
        /// Creates a sql command for a sqlite connection
        /// </summary>
        /// <returns>A new <see cref="SqliteCommand"/> object</returns>
        SqliteCommand CreateCommand();

        /// <summary>
        /// Creates a sql command with specified command text for a sqlite connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="SqliteCommand"/> object</returns>
        SqliteCommand CreateCommand(string commandText);

        /// <summary>
        ///     Creates a sql command with specified <see cref="SqliteCommand"/> for a sqlite connection
        /// </summary>
        /// <param name="command">
        ///     A <see cref="SqliteCommand"/> object
        /// </param>
        /// <returns>A new <see cref="SqliteCommand"/> object</returns>
        SqliteCommand CreateCommand(SqliteCommand command);

        /// <summary>
        /// Creates a sql command with specified command text for a sql connection
        /// </summary>
        /// <param name="commandText">the command text</param>
        /// <returns>A new <see cref="SqliteCommand"/> object</returns>
        SqliteCommand CreateCommand(string commandText, CommandType commandType);
        
        /// <summary>
        /// Creates a sql data adapter for sqlite connection
        /// </summary>
        /// <returns>A new <see cref="SqliteDataAdapter"/> object</returns>
        SqliteDataAdapter CreateDataAdapter();
    }
}
