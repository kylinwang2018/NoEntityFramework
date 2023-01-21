using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace NoEntityFramework.MySql
{
    internal class MySqlLogger<TDbContext> : IMySqlLogger<TDbContext>
        where TDbContext : class, IMySqlDbContext
    {
        protected readonly ILogger<TDbContext> _logger;
        private readonly bool _statisticsEnabled;
        public MySqlLogger(
            IOptionsMonitor<RelationalDbOptions> options,
            ILogger<TDbContext> logger)
        {
            _logger = logger;
            _statisticsEnabled = options
                .Get(typeof(TDbContext).ToString()).EnableStatistics;
        }

        public void LogCritical(MySqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogCritical("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogCritical(Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogCritical("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }

        public void LogError(MySqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogError("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogError(Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogError("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }

        public void LogInfo(MySqlCommand sqlCommand, long timeConsumedInMillisecond,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
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

        public void LogWaring(MySqlCommand sqlCommand, Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogWarning("{customMessage}\n\tCommand:\n\t{Command}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                sqlCommand.CommandText,
                exception.Message,
                exception.StackTrace
                );
        }

        public void LogWaring(Exception exception,
#if NETSTANDARD2_0
            string message = null)
#else
            string? message = null)
#endif
        {
            _logger.LogWarning("{customMessage}\nException Message:\n\t{Message},\nException Stack:\n\t{Stack}",
                message ?? string.Empty,
                exception.Message,
                exception.StackTrace
            );
        }
    }
}
