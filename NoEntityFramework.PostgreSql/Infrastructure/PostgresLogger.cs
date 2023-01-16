using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Options;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    internal class PostgresLogger<TDbContext> : IPostgresLogger<TDbContext>
        where TDbContext : class, IPostgresDbContext
    {
        protected readonly ILogger<TDbContext> _logger;
        private readonly bool _statisticsEnabled;
        public PostgresLogger(
            IOptionsMonitor<RelationalDbOptions> options,
            ILogger<TDbContext> logger)
        {
            _logger = logger;
            _statisticsEnabled = options
                .Get(typeof(TDbContext).ToString()).EnableStatistics;
        }

        public void LogCritical(NpgsqlCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogCritical("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogCritical(Exception exception, string? message = null)
        {
            _logger.LogCritical("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }

        public void LogError(NpgsqlCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogError("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogError(Exception exception, string? message = null)
        {
            _logger.LogError("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }

        public void LogInfo(NpgsqlCommand sqlCommand, long timeConsumedInMillisecond, string? message = null)
        {
            if (_statisticsEnabled)
            {
                _logger.LogInformation("{customMessage}\n\tCommand:\n\t{Command}\nTime Consumed: {Time}[ms]",
                    message ?? string.Empty,
                    sqlCommand.CommandText,
                    timeConsumedInMillisecond
                    );
            }
            else
                _logger.LogInformation("{customMessage}\n\tCommand:\n\t{Command}",
                    message ?? string.Empty,
                    sqlCommand.CommandText
                    );
        }

        public void LogWaring(NpgsqlCommand sqlCommand, Exception exception, string? message = null)
        {
            _logger.LogWarning("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogWaring(Exception exception, string? message = null)
        {
            _logger.LogWarning("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }
    }
}
