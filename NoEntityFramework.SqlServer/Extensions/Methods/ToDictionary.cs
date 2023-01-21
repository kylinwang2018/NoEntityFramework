using NoEntityFramework.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(
            this ISqlServerQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(
            this ISqlServerQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(
            this ISqlServerQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenAsync();
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync())
#else
                    await using (var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync())
#endif
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(this ISqlServerQueryable query)
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(
            this ISqlServerQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenAsync();
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync())
#else
                    await using (var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync())
#endif
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }

                        if (query.ParameterModel != null)
                            query.SqlCommand
                                .CopyParameterValueToModels(query.ParameterModel);
                        query.Logger.LogInfo(query.SqlCommand, sqlConnection);
                    }
                }
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
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(this ISqlServerQueryable query)
        {
            return await query.AsDictionaryAsync(0, 1);
        }
    }
}