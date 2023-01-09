using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Return a <see cref="SqlDataReader"/>.
    /// </summary>
    public static class ToReader
    {
        /// <summary>
        ///     Create a <see cref="SqlDataReader"/> for calling method with selected query.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
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

        /// <summary>
        ///     Create a <see cref="SqlDataReader"/> for calling method with selected query.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqlDataReader"/>.</returns>
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
