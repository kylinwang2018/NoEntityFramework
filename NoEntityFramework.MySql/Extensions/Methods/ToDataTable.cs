﻿using System;
using System.Data;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="DataTable"/>.
    /// </summary>
    public static class ToDataTable
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static DataTable AsDataTable(
            this IMySqlQueryable query)
        {
            var dataTable = new DataTable();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.OpenWithRetry(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = query.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }

        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static async Task<DataTable> AsDataTableAsync(
            this IMySqlQueryable query)
        {
            var dataTable = new DataTable();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = query.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }
    }
}