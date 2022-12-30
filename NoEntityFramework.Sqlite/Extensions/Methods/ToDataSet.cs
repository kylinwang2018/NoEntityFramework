using System;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    public static class ToDataSet
    {
        public static DataSet AsDataSet(
            this ISqliteQueryable query)
        {
            var dataTable = new DataSet();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = query.SqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dataTable;
        }

        public static async Task<DataSet> AsDataSetAsync(
            this ISqliteQueryable query)
        {
            var dataTable = new DataSet();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataAdapter = query.ConnectionFactory.CreateDataAdapter();
                sqlDataAdapter.SelectCommand = query.SqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);
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
