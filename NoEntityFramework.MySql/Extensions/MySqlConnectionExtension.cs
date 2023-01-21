using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     Provides command and connection execution with retry logic.
    /// </summary>
    public static class MySqlConnectionExtension
    {
        #region Connection Open
        public static MySqlConnection OpenWithRetry(this MySqlConnection connection, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    connection.Open();
                    return connection;
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<MySqlConnection> OpenWithRetryAsync(this MySqlConnection connection, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    await connection.OpenAsync();
                    return connection;
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Connection Close
        public static MySqlConnection CloseWithRetry(this MySqlConnection connection, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    connection.Close();
                    return connection;
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<MySqlConnection> CloseWithRetryAsync(this MySqlConnection connection, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    await connection.CloseAsync();
                    return connection;
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteReader
        public static MySqlDataReader ExecuteReaderWithRetry(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static MySqlDataReader ExecuteReaderWithRetry(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteReader(commandBehavior);
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<DbDataReader> ExecuteReaderWithRetryAsync(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteReaderAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<DbDataReader> ExecuteReaderWithRetryAsync(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteReaderAsync(commandBehavior);
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteNonQuery
        public static int ExecuteNonQueryWithRetry(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<int> ExecuteNonQueryWithRetryAsync(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteScalar
        public static object ExecuteScalarWithRetry(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<object> ExecuteScalarWithRetryAsync(this MySqlCommand command, MySqlRetryLogicOption retryLogicOption)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Exception Handler
        private static void HandleNpgsqlException(Exception ex, int retry, MySqlRetryLogicOption retryLogicOption)
        {
            if (ex is MySqlException &&
                retry < retryLogicOption.NumberOfTries)
            {
                retryLogicOption.Logger.LogError(ex, $"Retry {retry}/{retryLogicOption.NumberOfTries}");
                Thread.Sleep(retryLogicOption.DeltaTime);
            }
            else
            {
                throw ex;
            }
        }

        private static async Task HandleNpgsqlExceptionAsync(Exception ex, int retry, MySqlRetryLogicOption retryLogicOption)
        {
            if (ex is MySqlException &&
                retry < retryLogicOption.NumberOfTries)
            {
                retryLogicOption.Logger.LogError(ex, $"Retry {retry}/{retryLogicOption.NumberOfTries}");
                await Task.Delay(retryLogicOption.DeltaTime);
            }
            else
            {
                throw ex;
            }
        }
        #endregion
    }
}