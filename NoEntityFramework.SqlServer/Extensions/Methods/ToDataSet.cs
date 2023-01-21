using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="DataSet"/>.
    /// </summary>
    public static class ToDataSet
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataSet"/> object contains the query result.</returns>
        public static DataSet AsDataSet(
            this ISqlServerQueryable query)
        {
            var dataTable = new DataSet();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = query.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataSet"/> object contains the query result.</returns>
        public static async Task<DataSet> AsDataSetAsync(
            this ISqlServerQueryable query)
        {
            var dataTable = new DataSet();
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
                    using (var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = query.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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