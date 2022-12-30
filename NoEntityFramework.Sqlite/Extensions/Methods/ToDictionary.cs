using NoEntityFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.Sqlite
{
    public static class ToDictionary
    {
        public static Dictionary<T, U>? AsDictionary<T, U>(
            this ISqliteQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }

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
            return dictionary; 
        }

        public static Dictionary<T, U>? AsDictionary<T, U>(this ISqliteQueryable query)
        {
            return query.AsDictionary<T, U>(0, 1);
        }

        public static Dictionary<string, string>? AsDictionary(
            this ISqliteQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();

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
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        public static Dictionary<string, string>? AsDictionary(this ISqliteQueryable query)
        {
            return query.AsDictionary(0, 1);
        }

        public static async Task<Dictionary<T, U>?> AsDictionaryAsync<T, U>(
            this ISqliteQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }

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
            return dictionary;
        }

        public static async Task<Dictionary<T, U>?> AsDictionaryAsync<T, U>(this ISqliteQueryable query)
        {
            return await query.AsDictionaryAsync<T, U>(0, 1);
        }

        public static async Task<Dictionary<string, string>?> AsDictionaryAsync(
            this ISqliteQueryable query, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();

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
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        public static async Task<Dictionary<string, string>?> AsDictionaryAsync(this ISqliteQueryable query)
        {
            return await query.AsDictionaryAsync(0, 1);
        }
    }
}
