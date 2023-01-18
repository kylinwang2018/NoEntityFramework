﻿using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NoEntityFramework.Sqlite;
using System.Diagnostics;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="DataTable"/>.
    /// </summary>
    public static class ToDataTable
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static DataTable AsDataTable(
            this ISqliteQueryable query)
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
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static async Task<DataTable> AsDataTableAsync(
            this ISqliteQueryable query)
        {
            var dataTable = new DataTable();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
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
