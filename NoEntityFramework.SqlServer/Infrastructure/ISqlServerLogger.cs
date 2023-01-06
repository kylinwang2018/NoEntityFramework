using Microsoft.Data.SqlClient;
using System;

namespace NoEntityFramework.SqlServer
{
    public interface ISqlServerLogger
    {
        void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string? message = null);
        void LogError(SqlCommand sqlCommand, Exception exception, string? message = null);
        void LogWaring(SqlCommand sqlCommand, Exception exception, string? message = null);
        void LogCritical(SqlCommand sqlCommand, Exception exception, string? message = null);
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
