﻿using NoEntityFramework.DataManipulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Execute the command than cast the result to a <see cref="List{T}"/>.
    /// </summary>
    public static class ToList
    {
        /// <summary>
        ///     Execute the command than cast the result to a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object that represents the each row of the result of the query.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="List{T}"/> of the query result.</returns>
        public static List<T> AsList<T>(this ISqlServerQueryable query)
            where T : class, new()
        {
            var list = new List<T>();
            var type = typeof(T);
            var objectProperties = ModelCache.GetProperties(type);
            try
            {
                using (var sqlConnection = query.SqlConnection)
                {
                    sqlConnection.Open();
                    query.SqlCommand.Connection = sqlConnection;
                    using (var sqlDataReader = query.SqlCommand.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {
                query.Logger.LogError(query.SqlCommand, ex);
                throw;
            }
            return list;
        }

        /// <summary>
        ///     Execute the command than cast the result to a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object that represents the each row of the result of the query.</typeparam>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>A <see cref="List{T}"/> of the query result.</returns>
        public static async Task<List<T>> AsListAsync<T>(this ISqlServerQueryable query)
            where T : class, new()
        {
            var list = new List<T>();
            var type = typeof(T);
            var objectProperties = ModelCache.GetProperties(type);
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
                }
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