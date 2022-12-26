using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToList
    {
        public static List<T> AsList<T>(this ISqlServerQueryable query)
            where T : class, new()
        {
            var list = new List<T>();
            var type = typeof(T);
            var objectProperties = ModelCache.GetProperties(type);
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var obj = (T)Activator.CreateInstance(type);
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            propertyInfo.SetValue(obj,
                                propertyInfo.PropertyType.IsEnum
                                    ? Enum.ToObject(propertyInfo.PropertyType,
                                        (int)sqlDataReader[propertyInfo.GetColumnName()])
                                    : Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()],
                                        Nullable.GetUnderlyingType(propertyInfo.PropertyType) ??
                                        propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    list.Add(obj);
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
            return list;
        }

        public static async Task<List<T>> AsListAsync<T>(this ISqlServerQueryable query)
            where T : class, new()
        {
            var list = new List<T>();
            var type = typeof(T);
            var objectProperties = ModelCache.GetProperties(type);
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    var obj = (T)Activator.CreateInstance(type);
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            propertyInfo.SetValue(obj,
                                propertyInfo.PropertyType.IsEnum
                                    ? Enum.ToObject(propertyInfo.PropertyType,
                                        (int)sqlDataReader[propertyInfo.GetColumnName()])
                                    : Convert.ChangeType(sqlDataReader[propertyInfo.GetColumnName()],
                                        Nullable.GetUnderlyingType(propertyInfo.PropertyType) ??
                                        propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    list.Add(obj);
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
            return list;
        }
    }
}
