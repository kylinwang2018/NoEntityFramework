using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;
using NoEntityFramework.Npgsql.Models;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Provides command and connection execution with retry logic.
    /// </summary>
    public static class PostgresConnectionExtension
    {
        #region Connection Open
        public static NpgsqlConnection OpenWithRetry(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
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

        public static async Task<NpgsqlConnection> OpenWithRetryAsync(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
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
        public static NpgsqlConnection CloseWithRetry(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
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

        public static async Task<NpgsqlConnection> CloseWithRetryAsync(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
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
        public static NpgsqlDataReader ExecuteReaderWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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

        public static NpgsqlDataReader ExecuteReaderWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
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

        public static async Task<NpgsqlDataReader> ExecuteReaderWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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

        public static async Task<NpgsqlDataReader> ExecuteReaderWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
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
        public static int ExecuteNonQueryWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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

        public static async Task<int> ExecuteNonQueryWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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
        public static object ExecuteScalarWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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

        public static async Task<object> ExecuteScalarWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
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
        private static void HandleNpgsqlException(Exception ex, int retry, NpgsqlRetryLogicOption retryLogicOption)
        {
            if (ex is NpgsqlException &&
                retry < retryLogicOption.NumberOfTries)
            {
                Thread.Sleep(retryLogicOption.DeltaTime);
            }
            else
            {
                throw ex;
            }
        }

        private static async Task HandleNpgsqlExceptionAsync(Exception ex, int retry, NpgsqlRetryLogicOption retryLogicOption)
        {
            if (ex is NpgsqlException &&
                retry < retryLogicOption.NumberOfTries)
            {
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