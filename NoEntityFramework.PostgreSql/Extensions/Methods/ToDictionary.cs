using NoEntityFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class ToDictionary
    {
        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para> 
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(
            this IPostgresQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.OpenWithRetry(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReaderWithRetry(query.RetryLogicOption))
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para> 
        /// <para>
        ///     The first column will be the key and second column will be the value.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this IPostgresQueryable query)
        {
            return query.AsDictionary<TKey, TValue>(0, 1);
        }

        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>, both key and value
        ///     are <see langword="string"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para>
        /// </summary> 
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(
            this IPostgresQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.OpenWithRetry(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReaderWithRetry(query.RetryLogicOption))
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        /// <summary>
        ///  <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>, both key and value
        ///     are <see langword="string"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para>
        /// <para>
        ///     The first column will be the key and second column will be the value.
        /// </para>
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(this IPostgresQueryable query)
        {
            return query.AsDictionary(0, 1);
        }

        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para> 
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(
            this IPostgresQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para> 
        /// <para>
        ///     The first column will be the key and second column will be the value.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(this IPostgresQueryable query)
        {
            return await query.AsDictionaryAsync<TKey, TValue>(0, 1);
        }

        /// <summary>
        /// <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>, both key and value
        ///     are <see langword="string"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para>
        /// </summary> 
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(
            this IPostgresQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                using (var sqlConnection = query.SqlConnection)
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }
                    }
                }

                watch.Stop();

                if (query.ParameterModel != null)
                    query.SqlCommand
                        .CopyParameterValueToModels(query.ParameterModel);
                query.Logger.LogInfo(query.SqlCommand, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        /// <summary>
        ///  <para>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>, both key and value
        ///     are <see langword="string"/>.
        /// </para>
        /// <para>
        ///     The use case of this method for example a table contains key-value pairs for user configuration;
        ///     or only two columns need to be read.
        /// </para>
        /// <para>
        ///     The first column will be the key and second column will be the value.
        /// </para>
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(this IPostgresQueryable query)
        {
            return await query.AsDictionaryAsync(0, 1);
        }
    }
}
