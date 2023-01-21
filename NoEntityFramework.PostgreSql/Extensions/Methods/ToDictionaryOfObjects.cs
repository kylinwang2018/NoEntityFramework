using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoEntityFramework.Npgsql
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
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index number of the key column of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, TObject> AsDictionaryOfObjects<TKey, TObject>(
            this IPostgresQueryable query, int keyColumnIndex)
        where TKey : struct
        where TObject : class, new()
        {
            var dictionary = new Dictionary<TKey, TObject>();
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, TObject> AsDictionaryOfObjects<TKey, TObject>(
            this IPostgresQueryable query, string keyColumnName)
            where TKey : struct
            where TObject : class, new()
        {
            var dictionary = new Dictionary<TKey, TObject>();
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, List<TObject>> AsDictionaryOfListObjects<TKey, TObject>(
            this IPostgresQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The key column's index of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static Dictionary<TKey, List<TObject>> AsDictionaryOfListObjects<TKey, TObject>(
            this IPostgresQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The index number of the key column of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, TObject>> AsDictionaryOfObjectsAsync<TKey, TObject>(
            this IPostgresQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, TObject>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
#else
                await using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TValue}"/>.
        /// <para>
        ///     If index column has identical data, the later result will replace the value in the result <see cref="Dictionary{TKey,TValue}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, TObject>> AsDictionaryOfObjectsAsync<TKey, TObject>(
            this IPostgresQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, TObject>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
#else
                await using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnName">The key column name of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, List<TObject>>> AsDictionaryOfListObjectsAsync<TKey, TObject>(
            this IPostgresQueryable query, string keyColumnName)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
#else
                await using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
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
        ///     Execute the command than cast the result to a <see cref="Dictionary{TKey,TObject}"/>.
        /// <para>
        ///     If index column has identical data, the data with same key will be stored into a <see cref="List{TObject}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TKey">The index key of the objects.</typeparam>
        /// <typeparam name="TObject">The object that represents each result row.</typeparam>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="keyColumnIndex">The key column's index of the result.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
        public static async Task<Dictionary<TKey, List<TObject>>> AsDictionaryOfListObjectsAsync<TKey, TObject>(
            this IPostgresQueryable query, int keyColumnIndex)
        {
            var dictionary = new Dictionary<TKey, List<TObject>>();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

#if NETSTANDARD2_0
                using (var sqlConnection = query.SqlConnection)
#else
                await using (var sqlConnection = query.SqlConnection)
#endif
                {
                    await sqlConnection.OpenWithRetryAsync(query.RetryLogicOption);
                    query.SqlCommand.Connection = sqlConnection;
#if NETSTANDARD2_0
                    using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
#else
                await using (var sqlDataReader =
                           await query.SqlCommand.ExecuteReaderWithRetryAsync(query.RetryLogicOption))
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
    }
}