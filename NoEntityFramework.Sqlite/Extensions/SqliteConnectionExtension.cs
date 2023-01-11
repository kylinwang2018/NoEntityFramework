using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Data.Sqlite;
using NoEntityFramework.Sqlite.Models;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Provides command and connection execution with retry logic.
    /// </summary>
    public static class SqliteConnectionExtension
    {
        #region Connection Open
        public static SqliteConnection OpenWithRetry(this SqliteConnection connection, SqliteRetryLogicOption retryLogicOption)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<SqliteConnection> OpenWithRetryAsync(this SqliteConnection connection, SqliteRetryLogicOption retryLogicOption)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Connection Close
        public static SqliteConnection CloseWithRetry(this SqliteConnection connection, SqliteRetryLogicOption retryLogicOption)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<SqliteConnection> CloseWithRetryAsync(this SqliteConnection connection, SqliteRetryLogicOption retryLogicOption)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteReader
        public static SqliteDataReader ExecuteReaderWithRetry(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static SqliteDataReader ExecuteReaderWithRetry(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<SqliteDataReader> ExecuteReaderWithRetryAsync(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<SqliteDataReader> ExecuteReaderWithRetryAsync(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteNonQuery
        public static int ExecuteNonQueryWithRetry(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<int> ExecuteNonQueryWithRetryAsync(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteScalar
        public static object ExecuteScalarWithRetry(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    HandleSqliteException(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<object> ExecuteScalarWithRetryAsync(this SqliteCommand command, SqliteRetryLogicOption retryLogicOption)
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
                    await HandleSqliteExceptionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Exception Handler
        private static void HandleSqliteException(Exception ex, int retry, SqliteRetryLogicOption retryLogicOption)
        {
            if (ex is SqliteException &&
                retry < retryLogicOption.NumberOfTries)
            {
                Thread.Sleep(retryLogicOption.DeltaTime);
            }
            else
            {
                throw ex;
            }
        }

        private static async Task HandleSqliteExceptionAsync(Exception ex, int retry, SqliteRetryLogicOption retryLogicOption)
        {
            if (ex is SqliteException &&
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