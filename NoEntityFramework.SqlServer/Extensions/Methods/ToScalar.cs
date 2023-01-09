using System;
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
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlTransaction = sqlConnection.BeginTransaction();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = query.SqlCommand.ExecuteScalar();
                sqlTransaction.Commit();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                if (result == null)
                    return default;
                return (T)result;
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
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlTransaction = sqlConnection.BeginTransaction();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = await query.SqlCommand.ExecuteScalarAsync();
                sqlTransaction.Commit();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                if (result == null)
                    return default;
                return (T)result;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }
    }
}
