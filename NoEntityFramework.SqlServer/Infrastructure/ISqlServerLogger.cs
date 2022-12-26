using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoEntityFramework.SqlServer.Infrastructure
{
    public interface ISqlServerLogger
    {
        void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string? message = null);
        void LogError(SqlCommand sqlCommand, Exception exception, string? message = null);
        void LogWaring(SqlCommand sqlCommand, Exception exception, string? message = null);
        void LogCritical(SqlCommand sqlCommand, Exception exception, string? message = null);
    }

    public interface ISqlServerLogger<out TDbContext> : ISqlServerLogger
        where TDbContext : class, ISqlServerDbContext
    {

    }
}
