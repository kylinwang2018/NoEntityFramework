using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    public static class ToReader
    {
        public static SqliteDataReader AsDataReader(
            this ISqliteQueryable query, CommandBehavior? commandBehavior)
        {
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                var reader = commandBehavior == null? 
                    query.SqlCommand.ExecuteReader() :
                    query.SqlCommand.ExecuteReader((CommandBehavior)commandBehavior);

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                return reader;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }

        public static async Task<SqliteDataReader> AsDataReaderAsync(
            this ISqliteQueryable query, CommandBehavior? commandBehavior)
        {
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                var reader = commandBehavior == null ?
                    await query.SqlCommand.ExecuteReaderAsync() :
                    await query.SqlCommand.ExecuteReaderAsync((CommandBehavior)commandBehavior);

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

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
