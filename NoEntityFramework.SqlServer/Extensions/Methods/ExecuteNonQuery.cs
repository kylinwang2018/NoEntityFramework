using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer.Extensions.Methods
{
    public static class ExecuteNonQuery
    {
        public static int Execute(this ISqlServerQueryable query)
        {
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlTransaction = sqlConnection.BeginTransaction();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = query.SqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                return result;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }

        public static async Task<int> ExecuteAsync(this ISqlServerQueryable query)
        {
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlTransaction = sqlConnection.BeginTransaction();
                query.SqlCommand.Connection = sqlConnection;
                query.SqlCommand.Transaction = sqlTransaction;
                var result = await query.SqlCommand.ExecuteNonQueryAsync();
                sqlTransaction.Commit();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, sqlConnection);

                return result;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }
    }
}
