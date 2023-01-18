using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Execute a non-query command.
    /// </summary>
    public static class ExecuteNonQuery
    {
        /// <summary>
        ///     Execute a non-query command.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static int Execute(this ISqliteQueryable query)
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
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static async Task<int> ExecuteAsync(this ISqliteQueryable query)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                int result;

                using (var sqlConnection = query.SqlConnection)
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        result = await query.SqlCommand.ExecuteNonQueryWithRetryAsync(query.RetryLogicOption);
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
    }
}
