using System;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="DataTable"/>.
    /// </summary>
    public static class ToDataTable
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static DataTable AsDataTable(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataTable();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    sqlConnection.Open();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataAdapter = sqlServerQueryable.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = sqlServerQueryable.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }

        /// <summary>
        ///     Execute the command than cast the result to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="DataTable"/> object contains the query result.</returns>
        public static async Task<DataTable> AsDataTableAsync(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataTable();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    await sqlConnection.OpenAsync();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataAdapter = sqlServerQueryable.ConnectionFactory.CreateDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = sqlServerQueryable.SqlCommand;
                        sqlDataAdapter.Fill(dataTable);
                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }
    }
}
