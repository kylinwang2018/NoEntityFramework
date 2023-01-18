using Microsoft.Data.Sqlite;
using System;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    public interface ISqliteLogger
    {
        /// <summary>
        ///     Log message with INFORMATION level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="timeConsumedInMillisecond">The total time consumed for the query.</param>
        /// <param name="message">The additional message.</param>
        void LogInfo(SqliteCommand sqlCommand, long timeConsumedInMillisecond, string message = null);

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogError(SqliteCommand sqlCommand, Exception exception, string message = null);

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogError(Exception exception, string message = null);

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogWaring(SqliteCommand sqlCommand, Exception exception, string message = null);

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogWaring(Exception exception, string message = null);

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogCritical(SqliteCommand sqlCommand, Exception exception, string message = null);

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogCritical(Exception exception, string message = null);
    }

    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context used</typeparam>
    public interface ISqliteLogger<out TDbContext> : ISqliteLogger
        where TDbContext : class, ISqliteDbContext
    {

    }
}
