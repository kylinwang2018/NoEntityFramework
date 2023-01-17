using NoEntityFramework.Exceptions;
using System;
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    sqlConnection.Open();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = sqlServerQueryable.SqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }

                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this ISqlServerQueryable sqlServerQueryable)
        {
            return sqlServerQueryable.AsDictionary<TKey, TValue>(0, 1);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    sqlConnection.Open();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = sqlServerQueryable.SqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (sqlDataReader.Read())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }

                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static Dictionary<string, string> AsDictionary(this ISqlServerQueryable sqlServerQueryable)
        {
            return sqlServerQueryable.AsDictionary(0, 1);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    await sqlConnection.OpenAsync();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = await sqlServerQueryable.SqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add((TKey)sqlDataReader[keyColumnIndex], (TValue)sqlDataReader[valueColumnIndex]);
                        }

                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs.</returns>
        public static async Task<Dictionary<TKey, TValue>> AsDictionaryAsync<TKey, TValue>(this ISqlServerQueryable sqlServerQueryable)
        {
            return await sqlServerQueryable.AsDictionaryAsync<TKey, TValue>(0, 1);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index of the key column, start from <see langword="0"/>.</param>
        /// <param name="valueColumnIndex">The index of the value column, start from <see langword="0"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using (var sqlConnection = sqlServerQueryable.SqlConnection)
                {
                    await sqlConnection.OpenAsync();
                    sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = await sqlServerQueryable.SqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.FieldCount < 2 &&
                            !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                            throw new DatabaseException("Query did not return at least two columns of data.");
                        while (await sqlDataReader.ReadAsync())
                        {
                            dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                        }

                        if (sqlServerQueryable.ParameterModel != null)
                            sqlServerQueryable.SqlCommand
                                .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                        sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
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
        /// <param name="sqlServerQueryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> contains key-value pairs as <see langword="string"/>.</returns>
        public static async Task<Dictionary<string, string>> AsDictionaryAsync(this ISqlServerQueryable sqlServerQueryable)
        {
            return await sqlServerQueryable.AsDictionaryAsync(0, 1);
        }
    }
}
