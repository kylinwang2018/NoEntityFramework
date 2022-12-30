using Microsoft.Data.Sqlite;
using System;

namespace NoEntityFramework.Sqlite
{
    public interface ISqliteLogger
    {
        void LogInfo(SqliteCommand sqlCommand, SqliteConnection connection, string? message = null);
        void LogError(SqliteCommand sqlCommand, Exception exception, string? message = null);
        void LogWaring(SqliteCommand sqlCommand, Exception exception, string? message = null);
        void LogCritical(SqliteCommand sqlCommand, Exception exception, string? message = null);
    }

    public interface ISqliteLogger<out TDbContext> : ISqliteLogger
        where TDbContext : class, ISqliteDbContext
    {

    }
}
