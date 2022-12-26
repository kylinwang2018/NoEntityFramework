using NoEntityFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToDictionary
    {
        public static Dictionary<T, U>? AsDictionary<T, U>(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = sqlServerQueryable.SqlConnection;
                sqlConnection?.Open();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = sqlServerQueryable.SqlCommand.ExecuteReader();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }

                sqlServerQueryable.SqlCommand
                    .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dictionary; 
        }

        public static Dictionary<T, U>? AsDictionary<T, U>(this ISqlServerQueryable sqlServerQueryable)
        {
            return sqlServerQueryable.AsDictionary<T, U>(0, 1);
        }

        public static Dictionary<string, string>? AsDictionary(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using var sqlConnection = sqlServerQueryable.SqlConnection;
                sqlConnection?.Open();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = sqlServerQueryable.SqlCommand.ExecuteReader();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (sqlDataReader.Read())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }

                sqlServerQueryable.SqlCommand
                    .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        public static Dictionary<string, string>? AsDictionary(this ISqlServerQueryable sqlServerQueryable)
        {
            return sqlServerQueryable.AsDictionary(0, 1);
        }

        public static async Task<Dictionary<T, U>?> AsDictionaryAsync<T, U>(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                await using var sqlConnection = sqlServerQueryable.SqlConnection;
                await sqlConnection.OpenAsync();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await sqlServerQueryable.SqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], (U)sqlDataReader[valueColumnIndex]);
                }

                sqlServerQueryable.SqlCommand
                    .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        public static async Task<Dictionary<T, U>?> AsDictionaryAsync<T, U>(this ISqlServerQueryable sqlServerQueryable)
        {
            return await sqlServerQueryable.AsDictionaryAsync<T, U>(0, 1);
        }

        public static async Task<Dictionary<string, string>?> AsDictionaryAsync(
            this ISqlServerQueryable sqlServerQueryable, int keyColumnIndex, int valueColumnIndex)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                await using var sqlConnection = sqlServerQueryable.SqlConnection;
                await sqlConnection.OpenAsync();
                sqlServerQueryable.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await sqlServerQueryable.SqlCommand.ExecuteReaderAsync();

                if (sqlDataReader.FieldCount < 2 &&
                    !(keyColumnIndex == valueColumnIndex && sqlDataReader.FieldCount == 1))
                    throw new DatabaseException("Query did not return at least two columns of data.");
                while (await sqlDataReader.ReadAsync())
                {
                    dictionary.Add(sqlDataReader[keyColumnIndex].ToString(), sqlDataReader[valueColumnIndex].ToString());
                }

                sqlServerQueryable.SqlCommand
                    .CopyParameterValueToModels(sqlServerQueryable.ParameterModel);
                sqlServerQueryable.Logger.LogInfo(sqlServerQueryable.SqlCommand, sqlConnection);
            }
            catch (Exception ex)
            {
                sqlServerQueryable.Logger.LogError(sqlServerQueryable.SqlCommand, ex);
                throw;
            }
            return dictionary;
        }

        public static async Task<Dictionary<string, string>?> AsDictionaryAsync(this ISqlServerQueryable sqlServerQueryable)
        {
            return await sqlServerQueryable.AsDictionaryAsync(0, 1);
        }
    }
}
