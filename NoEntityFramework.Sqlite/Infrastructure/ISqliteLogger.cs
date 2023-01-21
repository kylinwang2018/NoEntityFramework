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
#if NETSTANDARD2_0
        void LogInfo(SqliteCommand sqlCommand, long timeConsumedInMillisecond, string message = null);
#else
        void LogInfo(SqliteCommand sqlCommand, long timeConsumedInMillisecond, string? message = null);
#endif

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogError(SqliteCommand sqlCommand, Exception exception, string message = null);
#else
        void LogError(SqliteCommand sqlCommand, Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogError(Exception exception, string message = null);
#else
        void LogError(Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogWaring(SqliteCommand sqlCommand, Exception exception, string message = null);
#else
        void LogWaring(SqliteCommand sqlCommand, Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogWaring(Exception exception, string message = null);
#else
        void LogWaring(Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogCritical(SqliteCommand sqlCommand, Exception exception, string message = null);
#else
        void LogCritical(SqliteCommand sqlCommand, Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogCritical(Exception exception, string message = null);
#else
        void LogCritical(Exception exception, string? message = null);
#endif
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
