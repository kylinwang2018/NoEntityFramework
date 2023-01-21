using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class ToDictionaryOfObjects
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index number of the key column of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, TObject> AsDictionaryOfObjects<TKey, TObject>(
            this ISqlServerQueryable query, int keyColumnIndex)
        where TKey : struct
        where TObject : class, new()
        {
            var dictionary = new Dictionary<TKey, TObject>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (sqlDataReader.Read())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            dictionary[(TKey)sqlDataReader[keyColumnIndex]] = u;
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, TObject> AsDictionaryOfObjects<TKey, TObject>(
            this ISqlServerQueryable query, string keyColumnName)
            where TKey : struct
            where TObject : class, new()
        {
            var dictionary = new Dictionary<TKey, TObject>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (sqlDataReader.Read())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            dictionary[(TKey)sqlDataReader[keyColumnName]] = u;
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, List<TObject>> AsDictionaryOfListObjects<TKey, TObject>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (sqlDataReader.Read())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            if (dictionary.TryGetValue((TKey)sqlDataReader[keyColumnName], out var list2))
                            {
                                list2.Add(u);
                            }
                            else
                            {
                                list2 = new List<TObject>
                        {
                            u
                        };
                                dictionary.Add((TKey)sqlDataReader[keyColumnName], list2);
                            }
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The key column's index of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, List<TObject>> AsDictionaryOfListObjects<TKey, TObject>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (sqlDataReader.Read())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            if (dictionary.TryGetValue((TKey)sqlDataReader[keyColumnIndex], out var list2))
                            {
                                list2.Add(u);
                            }
                            else
                            {
                                list2 = new List<TObject>
                        {
                            u
                        };
                                dictionary.Add((TKey)sqlDataReader[keyColumnIndex], list2);
                            }
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index number of the key column of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, TObject>> AsDictionaryOfObjectsAsync<TKey, TObject>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TObject>();
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
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (await sqlDataReader.ReadAsync())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            dictionary[(TKey)sqlDataReader[keyColumnIndex]] = u;
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, TObject>> AsDictionaryOfObjectsAsync<TKey, TObject>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, TObject>();
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
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (await sqlDataReader.ReadAsync())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            dictionary[(TKey)sqlDataReader[keyColumnName]] = u;
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, List<TObject>>> AsDictionaryOfListObjectsAsync<TKey, TObject>(
            this ISqlServerQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
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
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (await sqlDataReader.ReadAsync())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            if (dictionary.TryGetValue((TKey)sqlDataReader[keyColumnName], out var list2))
                            {
                                list2.Add(u);
                            }
                            else
                            {
                                list2 = new List<TObject>
                        {
                            u
                        };
                                dictionary.Add((TKey)sqlDataReader[keyColumnName], list2);
                            }
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The key column's index of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, List<TObject>>> AsDictionaryOfListObjectsAsync<TKey, TObject>(
            this ISqlServerQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
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
                        var typeFromHandle = typeof(TObject);
                        var objectProperties = ModelCache.GetProperties(typeFromHandle);

                        while (await sqlDataReader.ReadAsync())
                        {
                            var u = Activator.CreateInstance<TObject>();
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
                            if (dictionary.TryGetValue((TKey)sqlDataReader[keyColumnIndex], out var list2))
                            {
                                list2.Add(u);
                            }
                            else
                            {
                                list2 = new List<TObject>
                        {
                            u
                        };
                                dictionary.Add((TKey)sqlDataReader[keyColumnIndex], list2);
                            }
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
    }
}