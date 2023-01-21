using System;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    public interface IPostgresLogger
    {
        /// <summary>
        ///     Log message with INFORMATION level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="timeConsumedInMillisecond">The total time consumed for the query.</param>
        /// <param name="message">The additional message.</param>
        void LogInfo(NpgsqlCommand sqlCommand, long timeConsumedInMillisecond,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogError(NpgsqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogError(Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogWaring(NpgsqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        void LogWaring(Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogCritical(NpgsqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogCritical(Exception exception,
#if NETSTANDARD2_0
            string message = null);
#else
            string? message = null);
#endif
    }

    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context used</typeparam>
    public interface IPostgresLogger<out TDbContext> : IPostgresLogger
        where TDbContext : class, IPostgresDbContext
    {

    }
}
