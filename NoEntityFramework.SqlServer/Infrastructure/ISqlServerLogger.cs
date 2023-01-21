using Microsoft.Data.SqlClient;
using System;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    public interface ISqlServerLogger
    {
        /// <summary>
        ///     Log message with INFORMATION level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="connection">The connection that contain the command.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string message = null);
#else
        void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string? message = null);
#endif


        /// <summary>
        ///     Log message with ERROR level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogError(SqlCommand sqlCommand, Exception exception, string message = null);
#else
        void LogError(SqlCommand sqlCommand, Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with WARING level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogWaring(SqlCommand sqlCommand, Exception exception, string message = null);
#else
        void LogWaring(SqlCommand sqlCommand, Exception exception, string? message = null);
#endif

        /// <summary>
        ///     Log message with CRITICAL level.
        /// </summary>
        /// <param name="sqlCommand">The command that executed.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The additional message.</param>
#if NETSTANDARD2_0
        void LogCritical(SqlCommand sqlCommand, Exception exception, string message = null);
#else
        void LogCritical(SqlCommand sqlCommand, Exception exception, string? message = null);
#endif
    }

    /// <summary>
    ///     A logger for logging purpose.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context used</typeparam>
    public interface ISqlServerLogger<out TDbContext> : ISqlServerLogger
        where TDbContext : class, ISqlServerDbContext
    {

    }
}
