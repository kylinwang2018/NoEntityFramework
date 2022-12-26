using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    public static class ToDictionaryOfObjects
    {
        public static Dictionary<T, U> AsDictionaryOfObjects<T, U>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (sqlDataReader.Read())
                {
                    var u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], u);
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

        public static Dictionary<T, U> AsDictionaryOfObjects<T, U>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();
                var typeFromHandle = typeof(U);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (sqlDataReader.Read())
                {
                    var u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    dictionary.Add((T)sqlDataReader[keyColumnName], u);
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

        public static Dictionary<T, List<TU>> AsDictionaryOfListObjects<T, TU>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<T, List<TU>>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();
                var typeFromHandle = typeof(TU);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (sqlDataReader.Read())
                {
                    var u = Activator.CreateInstance<TU>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    if (dictionary.TryGetValue((T)sqlDataReader[keyColumnName], out var list2))
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<TU>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnName], list2);
                    }
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

        public static Dictionary<T, List<TU>> AsDictionaryOfListObjects<T, TU>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<T, List<TU>>();
            try
            {
                using var sqlConnection = query.SqlConnection;
                sqlConnection.Open();
                query.SqlCommand.Connection = sqlConnection;
                using var sqlDataReader = query.SqlCommand.ExecuteReader();
                var typeFromHandle = typeof(TU);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (sqlDataReader.Read())
                {
                    var u = Activator.CreateInstance<TU>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    if (dictionary.TryGetValue((T)sqlDataReader[keyColumnIndex], out var list2))
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<TU>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnIndex], list2);
                    }
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

        public static async Task<Dictionary<T, U>> AsDictionaryOfObjectsAsync<T, U>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (await sqlDataReader.ReadAsync())
                {
                    var u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    dictionary.Add((T)sqlDataReader[keyColumnIndex], u);
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

        public static async Task<Dictionary<T, U>> AsDictionaryOfObjectsAsync<T, U>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<T, U>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();
                var typeFromHandle = typeof(U);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (await sqlDataReader.ReadAsync())
                {
                    var u = Activator.CreateInstance<U>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    dictionary.Add((T)sqlDataReader[keyColumnName], u);
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

        public static async Task<Dictionary<T, List<TU>>> AsDictionaryOfListObjectsAsync<T, TU>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<T, List<TU>>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();
                var typeFromHandle = typeof(TU);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (await sqlDataReader.ReadAsync())
                {
                    var u = Activator.CreateInstance<TU>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    if (dictionary.TryGetValue((T)sqlDataReader[keyColumnName], out var list2))
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<TU>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnName], list2);
                    }
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

        public static async Task<Dictionary<T, List<TU>>> AsDictionaryOfListObjectsAsync<T, TU>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<T, List<TU>>();
            try
            {
                await using var sqlConnection = query.SqlConnection;
                await sqlConnection.OpenAsync();
                query.SqlCommand.Connection = sqlConnection;
                await using var sqlDataReader = await query.SqlCommand.ExecuteReaderAsync();
                var typeFromHandle = typeof(TU);
                var objectProperties = ModelCache.GetProperties(typeFromHandle);

                while (await sqlDataReader.ReadAsync())
                {
                    var u = Activator.CreateInstance<TU>();
                    foreach (var propertyInfo in objectProperties)
                    {
                        try
                        {
                            var isEnum = propertyInfo.PropertyType.IsEnum;
                            propertyInfo.SetValue(u,
                                isEnum
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
                    if (dictionary.TryGetValue((T)sqlDataReader[keyColumnIndex], out var list2))
                    {
                        list2.Add(u);
                    }
                    else
                    {
                        list2 = new List<TU>
                        {
                            u
                        };
                        dictionary.Add((T)sqlDataReader[keyColumnIndex], list2);
                    }
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
    }
}
