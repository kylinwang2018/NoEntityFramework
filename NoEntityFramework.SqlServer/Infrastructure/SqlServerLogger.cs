﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace NoEntityFramework.SqlServer
{
    internal class SqlServerLogger<TDbContext> : ISqlServerLogger<TDbContext>
        where TDbContext : class, ISqlServerDbContext
    {
        protected readonly ILogger<TDbContext> _logger;
        private readonly bool _statisticsEnabled;
        public SqlServerLogger(
            IOptionsMonitor<RelationalDbOptions> options,
            ILogger<TDbContext> logger)
        {
            _logger = logger;
            _statisticsEnabled = options
                .Get(typeof(TDbContext).ToString()).EnableStatistics;
        }


#if NETSTANDARD2_0
        public void LogCritical(SqlCommand sqlCommand, Exception exception, string message = null)
#else
        public void LogCritical(SqlCommand sqlCommand, Exception exception, string? message = null)
#endif
        {
            _logger.LogCritical(exception, "{customMessage}\n\tCommand:\n\t{Command}",
                message?? string.Empty,
                sqlCommand.CommandText
                );
        }

#if NETSTANDARD2_0
        public void LogError(SqlCommand sqlCommand, Exception exception, string message = null)
#else
        public void LogError(SqlCommand sqlCommand, Exception exception, string? message = null)
#endif
        {
            _logger.LogError(exception, "{customMessage}\n\tCommand:\n\t{Command}",
                message ?? string.Empty,
                sqlCommand.CommandText
            );
        }

#if NETSTANDARD2_0
        public void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string message = null)
#else
        public void LogInfo(SqlCommand sqlCommand, SqlConnection connection, string? message = null)
#endif
        {
            if (_statisticsEnabled)
            {
                var stats = connection.RetrieveStatistics();
                var executionTime = (long)stats["ExecutionTime"];
                var commandNetworkServerTimeInMs = (long)stats["NetworkServerTime"];
                _logger.LogInformation("{customMessage}\n\tCommand:\n\t{Command}\nExecution Time: {Time}[ms]\nNetwork Time: {NetworkTime}[ms]",
                    message ?? string.Empty,
                    sqlCommand.CommandText,
                    executionTime,
                    commandNetworkServerTimeInMs
                    );
            }
            else
                _logger.LogInformation("{customMessage}\n\tCommand:\n\t{Command}",
                    message ?? string.Empty,
                    sqlCommand.CommandText
                    );
        }
#if NETSTANDARD2_0
        public void LogWaring(SqlCommand sqlCommand, Exception exception, string message = null)
#else
        public void LogWaring(SqlCommand sqlCommand, Exception exception, string? message = null)
#endif
        {
            _logger.LogWarning(exception, "{customMessage}\n\tCommand:\n\t{Command}", 
                message ?? string.Empty,
                sqlCommand.CommandText);
        }
    }
}
