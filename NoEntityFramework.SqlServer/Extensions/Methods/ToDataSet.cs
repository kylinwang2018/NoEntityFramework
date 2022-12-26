using System;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToDataSet
    {
        public static DataSet AsDataSet(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataSet();
            try
            {
                using var sqlConnection = sqlServerQueryable.SqlConnection;
                sqlConnection.Open();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                using var sqlDataAdapter = sqlServerQueryable.ConnectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = sqlServerQueryable.SqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (sqlServerQueryable.ParameterModel != null)
                    sqlServerQueryable.SqlCommand
                        .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }

        public static async Task<DataSet> AsDataSetAsync(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataSet();
            try
            {
                await using var sqlConnection = sqlServerQueryable.SqlConnection;
                await sqlConnection.OpenAsync();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                using var sqlDataAdapter = sqlServerQueryable.ConnectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = sqlServerQueryable.SqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (sqlServerQueryable.ParameterModel != null)
                    sqlServerQueryable.SqlCommand
                        .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
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
