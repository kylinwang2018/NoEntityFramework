﻿using System;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute a non-query command.
    /// </summary>
    public static class ExecuteNonQuery
    {
        /// <summary>
        ///     Execute a non-query command.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static int Execute(this ISqlServerQueryable query)
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
                        var result = query.SqlCommand.ExecuteNonQuery();
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
        ///     Execute a non-query command.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The number of the row has been affected.</returns>
        public static async Task<int> ExecuteAsync(this ISqlServerQueryable query)
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
                        var result = await query.SqlCommand.ExecuteNonQueryAsync();
#if NETSTANDARD2_0
                        sqlTransaction.Commit();
#else
                        await sqlTransaction.CommitAsync();
#endif
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