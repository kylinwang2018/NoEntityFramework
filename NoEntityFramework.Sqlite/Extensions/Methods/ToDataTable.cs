using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NoEntityFramework.Sqlite;

namespace NoEntityFramework.Sqlite
{
    public static class ToDataTable
    {
        public static DataTable AsDataTable(
            this ISqliteQueryable query)
        {
            var dataTable = new DataTable();
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

        public static async Task<DataTable> AsDataTableAsync(
            this ISqliteQueryable query)
        {
            var dataTable = new DataTable();
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

        public static List<T> AsDataTable<T>(
            this ISqliteQueryable query) where T : class, new()
        {
            var dataTable = query.AsDataTable();
            return DataTableHelper.DataTableToList<T>(dataTable);
        }

        public static async Task<List<T>> AsDataTableAsync<T>(
            this ISqliteQueryable query) where T : class, new()
        {
            var dataTable = await query.AsDataTableAsync();
            return DataTableHelper.DataTableToList<T>(dataTable);
        }
    }
}
