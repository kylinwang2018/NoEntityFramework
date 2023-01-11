﻿using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Return a <see cref="SqliteDataReader"/>.
    /// </summary>
    public static class ToReader
    {
        /// <summary>
        ///     Create a <see cref="SqliteDataReader"/> for calling method with selected query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqliteDataReader"/>.</returns>
        public static SqliteDataReader AsDataReader(
            this ISqliteQueryable query, CommandBehavior? commandBehavior)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using var sqlConnection = query.SqlConnection;
                sqlConnection.OpenWithRetry(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                var reader = commandBehavior == null? 
                    query.SqlCommand.ExecuteReaderWithRetry(query.RetryLogicOption) :
                    query.SqlCommand.ExecuteReaderWithRetry(query.RetryLogicOption, (CommandBehavior)commandBehavior);

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

                return reader;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }

        /// <summary>
        ///     Create a <see cref="SqliteDataReader"/> for calling method with selected query.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqliteDataReader"/>.</returns>
        public static async Task<SqliteDataReader> AsDataReaderAsync(
            this ISqliteQueryable query, CommandBehavior? commandBehavior)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                var reader = commandBehavior == null ?
                    await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption) :
                    await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption, (CommandBehavior)commandBehavior);

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

                return reader;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }
    }
}
