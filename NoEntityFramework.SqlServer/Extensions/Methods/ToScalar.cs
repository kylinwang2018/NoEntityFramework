using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static T As<T>(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static string AsString(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static object AsScalar(this ISqlServerQueryable query)
        {
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        var result = query.SqlCommand.ExecuteScalar();
                        sqlTransaction.Commit();

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                        return result;
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<T> AsAsync<T>(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<string> AsStringAsync(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<object> AsScalarAsync(this ISqlServerQueryable query)
        {
            try
            {
#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenAsync();
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
#else
                    await using (var sqlTransaction = sqlConnection.BeginTransaction())
#endif
                    {
                        query.SqlCommand.Connection = sqlConnection;
                        query.SqlCommand.Transaction = sqlTransaction;
                        var result = await query.SqlCommand.ExecuteScalarAsync();
                        sqlTransaction.Commit();

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }
    }
}