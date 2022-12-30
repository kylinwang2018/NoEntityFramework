using Microsoft.Extensions.Logging;
using System;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    internal class SqliteLogger<TDbContext> : ISqliteLogger<TDbContext>
        where TDbContext : class, ISqliteDbContext
    {
        protected readonly ILogger<TDbContext> _logger;
        private readonly bool _statisticsEnabled;
        public SqliteLogger(
            ISqliteOptions<TDbContext> sqliteOptions,
            ILogger<TDbContext> logger)
        {
            _logger = logger;
            _statisticsEnabled = sqliteOptions.Options
                .Get(typeof(TDbContext).ToString()).EnableStatistics;
        }

        public void LogCritical(SqliteCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogCritical("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogError(SqliteCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogError("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogInfo(SqliteCommand sqlCommand, SqliteConnection connection, string? message = null)
        {
            if (_statisticsEnabled)
            {
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

        public void LogWaring(SqliteCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogWarning("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }
    }
}
