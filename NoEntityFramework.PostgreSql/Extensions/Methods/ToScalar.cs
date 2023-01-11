﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
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
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static T As<T>(this IPostgresQueryable query)
            where T : struct
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using var sqlConnection = query.SqlConnection;
                sqlConnection.OpenWithRetry(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                using var sqlTransaction = sqlConnection.BeginTransaction();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = query.SqlCommand.ExecuteScalarWithRetry(query.RetryLogicOption);
                sqlTransaction.Commit();

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

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
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>The value for the query.</returns>
        public static async Task<T> AsAsync<T>(this IPostgresQueryable query)
            where T : struct
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlTransaction = await sqlConnection.BeginTransactionAsync();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = await query.SqlCommand.ExecuteScalarWithRetryAsync(query.RetryLogicOption);
                await sqlTransaction.CommitAsync();

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

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
