using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NoEntityFramework.SqlServer
{
    public static class ToReader
    {
        public static SqlDataReader AsDataReader(
            this ISqlServerQueryable query, CommandBehavior? commandBehavior)
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

        public static async Task<SqlDataReader> AsDataReaderAsync(
            this ISqlServerQueryable query, CommandBehavior? commandBehavior)
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
