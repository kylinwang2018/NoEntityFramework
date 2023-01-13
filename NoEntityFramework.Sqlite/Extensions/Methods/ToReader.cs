using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Return a <see cref="SqliteDataReader"/>.
    /// </summary>
    public static class ToReader
    {
        /// <summary>
        ///     Create a <see cref="SqliteDataReader"/> for calling method with selected query.
        /// <para>
        ///     If a non-CloseConnection <see cref="CommandBehavior"/> is selected,
        ///     the <see cref="SqliteConnection"/> in the <see cref="ISqliteQueryable"/> must be closed manually.
        /// </para>
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqliteDataReader"/>.</returns>
        public static SqliteDataReader AsDataReader(
            this ISqliteQueryable query, CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var sqlConnection = query.SqlConnection;
                sqlConnection.OpenWithRetry(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                var reader = 
                    query.SqlCommand.ExecuteReaderWithRetry(query.RetryLogicOption, commandBehavior);

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

                return reader;
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
        }

        /// <summary>
        ///     Create a <see cref="SqliteDataReader"/> for calling method with selected query.
        /// <para>
        ///     If a non-CloseConnection <see cref="CommandBehavior"/> is selected,
        ///     the <see cref="SqliteConnection"/> in the <see cref="ISqliteQueryable"/> must be closed manually.
        /// </para>
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="commandBehavior">A description of the results of the query and its effect on the database.</param>
        /// <returns>A <see cref="SqliteDataReader"/>.</returns>
        public static async Task<SqliteDataReader> AsDataReaderAsync(
            this ISqliteQueryable query, CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                query.SqlCommand.Connection = sqlConnection;
                var reader = 
                    await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption, commandBehavior);

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);

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
