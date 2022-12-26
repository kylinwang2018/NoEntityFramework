using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToDataTable
    {
        public static DataTable AsDataTable(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataTable();
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

        public static async Task<DataTable> AsDataTableAsync(
            this ISqlServerQueryable sqlServerQueryable)
        {
            var dataTable = new DataTable();
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

        public static List<T> AsDataTable<T>(
            this ISqlServerQueryable sqlServerQueryable) where T : class, new()
        {
            var dataTable = sqlServerQueryable.AsDataTable();
            return DataTableHelper.DataTableToList<T>(dataTable);
        }

        public static async Task<List<T>> AsDataTableAsync<T>(
            this ISqlServerQueryable sqlServerQueryable) where T : class, new()
        {
            var dataTable = await sqlServerQueryable.AsDataTableAsync();
            return DataTableHelper.DataTableToList<T>(dataTable);
        }
    }
}
