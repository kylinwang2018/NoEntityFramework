using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="DataSet"/>.
    /// </summary>
    public static class ToDataSet
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataSet"/> object contains the query result.</returns>
        public static DataSet AsDataSet(
            this ISqliteQueryable query)
        {
            var dataTable = new DataSet();
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
        ///     Execute the command than cast the result to a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataSet"/> object contains the query result.</returns>
        public static async Task<DataSet> AsDataSetAsync(
            this ISqliteQueryable query)
        {
            var dataTable = new DataSet();
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