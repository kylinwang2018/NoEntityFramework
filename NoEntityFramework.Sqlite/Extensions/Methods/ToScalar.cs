using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Execute command and get a single value from the query.
    /// </summary>
    public static class ToScalar
    {
        /// <summary>
        ///     Execute the command and get a single value from the query.
        /// </summary>
        /// <typeparam name="T">The type of the returned value</typeparam>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static T As<T>(this ISqliteQueryable query)
            where T : struct
        {
            var obj = query.AsScalar();
            if (obj == null)
                return default;
            else
                return (T)obj;
        }

        /// <summary>
        ///     Execute the command and get a single string from the query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static string AsString(this ISqliteQueryable query)
        {
            var obj = query.AsScalar();
            if (obj == null)
                return string.Empty;
            else
                return (string)obj;
        }

        /// <summary>
        ///     Execute the command and get a single value from the query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static object AsScalar(this ISqliteQueryable query)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                object result;
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.OpenWithRetry(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        result = query.SqlCommand.ExecuteScalarWithRetry(query.RetryLogicOption);
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
        ///     Execute the command and get a single value from the query.
        /// </summary>
        /// <typeparam name="T">The type of the returned value</typeparam>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<T> AsAsync<T>(this ISqliteQueryable query)
            where T : struct
        {
            var obj = await query.AsScalarAsync();
            if (obj == null)
                return default;
            else
                return (T)obj;
        }

        /// <summary>
        ///     Execute the command and get a single string from the query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<string> AsStringAsync(this ISqliteQueryable query)
        {
            var obj = await query.AsScalarAsync();
            if (obj == null)
                return string.Empty;
            else
                return (string)obj;
        }

        /// <summary>
        ///     Execute the command and get a single value from the query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<object> AsScalarAsync(this ISqliteQueryable query)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                object result;
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
                    await using (var sqlTransaction = sqlConnection.BeginTransaction())
#endif
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        result = await query.SqlCommand.ExecuteScalarWithRetryAsync(query.RetryLogicOption);
#if NETSTANDARD2_0
                        sqlTransaction.Commit();
#else
                        await sqlTransaction.CommitAsync();
#endif
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