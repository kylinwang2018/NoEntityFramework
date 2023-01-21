using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Execute a non-query command.
    /// </summary>
    public static class ExecuteNonQuery
    {
        /// <summary>
        ///     Execute a non-query command.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static int Execute(this IPostgresQueryable query)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                int result;
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.OpenWithRetry(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        result = query.SqlCommand.ExecuteNonQueryWithRetry(query.RetryLogicOption);
                        sqlTransaction.Commit();
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }

        /// <summary>
        ///     Execute a non-query command.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static async Task<int> ExecuteAsync(this IPostgresQueryable query)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                int result;
#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
#else
                    await using (var sqlTransaction = await sqlConnection.BeginTransactionAsync())
#endif
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        result = await query.SqlCommand.ExecuteNonQueryWithRetryAsync(query.RetryLogicOption);
                        await sqlTransaction.CommitAsync();
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }
    }
}