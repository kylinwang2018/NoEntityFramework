using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NoEntityFramework.PostgresSQL
{
    public static class NpgsqlConnectionExtension
    {
        #region Connection Open
        public static NpgsqlConnection OpenWithRetry(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    connection.Open();
                    return connection;
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<NpgsqlConnection> OpenWithRetryAsync(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    await connection.OpenAsync();
                    return connection;
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Connection Close
        public static NpgsqlConnection CloseWithRetry(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    connection.Close();
                    return connection;
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<NpgsqlConnection> CloseWithRetryAsync(this NpgsqlConnection connection, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    await connection.CloseAsync();
                    return connection;
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteReader
        public static NpgsqlDataReader ExecuteReaderWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static NpgsqlDataReader ExecuteReaderWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteReader(commandBehavior);
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<NpgsqlDataReader> ExecuteReaderWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteReaderAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<NpgsqlDataReader> ExecuteReaderWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption, CommandBehavior commandBehavior)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteReaderAsync(commandBehavior);
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteNonQuery
        public static int ExecuteNonQueryWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<int> ExecuteNonQueryWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Command ExecuteScalar
        public static object ExecuteScalarWithRetry(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    HandleNpgsqlExcpetion(ex, retry++, retryLogicOption);
                }
            }
        }

        public static async Task<object> ExecuteScalarWithRetryAsync(this NpgsqlCommand command, NpgsqlRetryLogicOption retryLogicOption)
        {
            int retry = 0;
            while (true)
            {
                try
                {
                    return await command.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    await HandleNpgsqlExcpetionAsync(ex, retry++, retryLogicOption);
                }
            }
        }
        #endregion

        #region Exception Handler
        private static void HandleNpgsqlExcpetion(Exception ex, int retry, NpgsqlRetryLogicOption retryLogicOption)
        {
            if ((ex is NpgsqlException ||
                ex is InvalidOperationException) &&
                (ex.Message.Contains("A network-related or instance-specific error occurred")
                    || ex.Message.Contains("A transport-level error has occurred")
                    || ex.Message.Contains("The timeout period elapsed prior to obtaining a connection from the pool")
                    || ex.Message.Contains("an error occurred during the pre-login handshake")) &&
                retry < retryLogicOption.NumberOfTries) 
            {
                Thread.Sleep(retryLogicOption.DeltaTime);
            }
            else
            {
                throw ex;
            }
        }

        private static async Task HandleNpgsqlExcpetionAsync(Exception ex, int retry, NpgsqlRetryLogicOption retryLogicOption)
        {
            if ((ex is NpgsqlException ||
                ex is InvalidOperationException) &&
                (ex.Message.Contains("A network-related or instance-specific error occurred")
                    || ex.Message.Contains("A transport-level error has occurred")
                    || ex.Message.Contains("The timeout period elapsed prior to obtaining a connection from the pool")
                    || ex.Message.Contains("an error occurred during the pre-login handshake")) &&
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
