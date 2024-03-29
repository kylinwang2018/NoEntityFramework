﻿using NoEntityFramework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Add to Parameter to selected <see cref="NpgsqlCommand"/>.
    /// </summary>
    public static class ParameterAttacher
    {
        /// <summary>
        ///     Add one <see cref="NpgsqlParameter"/> to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="parameter">The <see cref="NpgsqlParameter"/> that want to add to.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPostgresQueryable WithParameter(
            this IPostgresQueryable queryable, NpgsqlParameter parameter)
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            // Check for derived output value with no value assigned
            if ((parameter.Direction == ParameterDirection.InputOutput ||
                parameter.Direction == ParameterDirection.Input) &&
                (parameter.Value == null))
            {
                parameter.Value = DBNull.Value;
            }

            queryable.SqlCommand.Parameters.Add(parameter);

            return queryable;
        }

        /// <summary>
        ///     Add multiple <see cref="NpgsqlParameter"/>s to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="parameters">The <see cref="NpgsqlParameter"/>s that want to add to.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPostgresQueryable WithParameters(
            this IPostgresQueryable queryable, params NpgsqlParameter[] parameters)
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            if (parameters.Length == 0)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var p in parameters)
            {
                // Check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput ||
                     p.Direction == ParameterDirection.Input) &&
                    (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                queryable.SqlCommand.Parameters.Add(p);
            }
            return queryable;
        }

        /// <summary>
        ///     Add parameters in a object model to <see cref="NpgsqlCommand"/>. The parameters must be defined with <see cref="PostgresDbParameterAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="parameterModel">The model object.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPostgresQueryable WithParameter<T>(
            this IPostgresQueryable queryable, T parameterModel)
            where T : class, new()
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var properties = typeof(T).GetProperties()
                .Where(s => s.CanRead && s.GetCustomAttributes<PostgresDbParameterAttribute>().Any())
                .ToList();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes<PostgresDbParameterAttribute>().First();
                var propertyType = property.PropertyType;
                if (string.IsNullOrEmpty(attr.Name))
                    throw new ArgumentNullException(nameof(attr.Name));

                var sqlParameter = new NpgsqlParameter()
                {
                    NpgsqlDbType = attr.Type == NpgsqlDbType.Unknown ? TypeMap[propertyType] : attr.Type,
                    Direction = attr.Direction,
                    Value = property.GetValue(parameterModel) ?? DBNull.Value,
                    ParameterName = attr.Name,
                    IsNullable = attr.Nullable,
                    Scale = attr.Scale ?? 0,
                    SourceColumn = attr.SourceColumn,
                    Precision = attr.Precision ?? 0,
                    SourceVersion = attr.SourceVersion,
                };

                if (attr.Size != null)
                    sqlParameter.Size = attr.Size.Value;

                queryable.SqlCommand.Parameters.Add(sqlParameter);
            }

            queryable.ParameterModel = parameterModel;

            return queryable;
        }

        /// <summary>
        ///     After executed the query, some parameters may need to be returned with a value, call this member can copy these kind
        ///     of parameters' value return to the model.
        /// </summary>
        /// <param name="sqlCommand">The <see cref="NpgsqlCommand"/> that is used to execute the query.</param>
        /// <param name="parameterModel">The parameter model that is used to execute the query.</param>
        /// <returns>The parameter model that is used to execute the query.</returns>
        public static object CopyParameterValueToModels(
            this NpgsqlCommand sqlCommand, object parameterModel)
        {
            var properties = parameterModel.GetType().GetProperties()
            .Where(s => s.CanWrite &&
                s.GetCustomAttributes<PostgresDbParameterAttribute>().Any() &&
                s.GetCustomAttributes<PostgresDbParameterAttribute>().First().Direction != ParameterDirection.Input)
            .ToList();

            foreach (var property in properties)
            {
                property.SetValue(parameterModel, sqlCommand.Parameters[property.Name].Value);
            }

            return parameterModel;
        }

        /// <summary>
        ///     Create and add one <see cref="NpgsqlParameter"/> to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="NpgsqlDbType"/> of the parameter.</param>
        /// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IPostgresQueryable WithParameter(
            this IPostgresQueryable queryable,
#if NETSTANDARD2_0
            string paramName, NpgsqlDbType dbType, ParameterDirection parameterDirection, object value, int? size)
#else
            string paramName, NpgsqlDbType dbType, ParameterDirection parameterDirection, object? value, int? size)
#endif
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var param = size == null ?
                new NpgsqlParameter(paramName, dbType) : new NpgsqlParameter(paramName, dbType, (int)size);

            param.Direction = parameterDirection;

            param.Value = value ?? DBNull.Value;

            queryable.SqlCommand.Parameters.Add(param);
            return queryable;
        }

        /// <summary>
        ///     Create and add one input only <see cref="NpgsqlParameter"/> to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="NpgsqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static IPostgresQueryable WithInputParameter(
            this IPostgresQueryable queryable, string paramName, NpgsqlDbType dbType, object value)
        {
            return WithInputParameter(queryable, paramName, dbType, value, null);
        }

        /// <summary>
        ///     Create and add one input only <see cref="NpgsqlParameter"/> to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="NpgsqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static IPostgresQueryable WithInputParameter(
            this IPostgresQueryable queryable, string paramName, NpgsqlDbType dbType, object value, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Input, value, size);
        }

        /// <summary>
        ///     Create and add one output only <see cref="NpgsqlParameter"/> to <see cref="NpgsqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="NpgsqlDbType"/> of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static IPostgresQueryable WithOutputParameter(
            this IPostgresQueryable queryable, string paramName, NpgsqlDbType dbType, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Output, null, size);
        }

        private static readonly Dictionary<Type, NpgsqlDbType> TypeMap = new Dictionary<Type, NpgsqlDbType>
        {
            [typeof(byte)] = NpgsqlDbType.Integer,
            [typeof(sbyte)] = NpgsqlDbType.Integer,
            [typeof(short)] = NpgsqlDbType.Smallint,
            [typeof(ushort)] = NpgsqlDbType.Smallint,
            [typeof(int)] = NpgsqlDbType.Integer,
            [typeof(uint)] = NpgsqlDbType.Integer,
            [typeof(long)] = NpgsqlDbType.Bigint,
            [typeof(ulong)] = NpgsqlDbType.Bigint,
            [typeof(float)] = NpgsqlDbType.Real,
            [typeof(double)] = NpgsqlDbType.Double,
            [typeof(decimal)] = NpgsqlDbType.Numeric,
            [typeof(bool)] = NpgsqlDbType.Boolean,
            [typeof(string)] = NpgsqlDbType.Varchar,
            [typeof(char)] = NpgsqlDbType.Text,
            [typeof(Guid)] = NpgsqlDbType.Uuid,
            [typeof(DateTime)] = NpgsqlDbType.Timestamp,
            [typeof(DateTimeOffset)] = NpgsqlDbType.TimestampTz,
            [typeof(byte[])] = NpgsqlDbType.Integer,
            [typeof(byte?)] = NpgsqlDbType.Integer,
            [typeof(sbyte?)] = NpgsqlDbType.Integer,
            [typeof(short?)] = NpgsqlDbType.Smallint,
            [typeof(ushort?)] = NpgsqlDbType.Smallint,
            [typeof(int?)] = NpgsqlDbType.Integer,
            [typeof(uint?)] = NpgsqlDbType.Integer,
            [typeof(long?)] = NpgsqlDbType.Bigint,
            [typeof(ulong?)] = NpgsqlDbType.Bigint,
            [typeof(float?)] = NpgsqlDbType.Real,
            [typeof(double?)] = NpgsqlDbType.Double,
            [typeof(decimal?)] = NpgsqlDbType.Numeric,
            [typeof(bool?)] = NpgsqlDbType.Boolean,
            [typeof(char?)] = NpgsqlDbType.Text,
            [typeof(Guid?)] = NpgsqlDbType.Uuid,
            [typeof(DateTime?)] = NpgsqlDbType.Timestamp,
            [typeof(DateTimeOffset?)] = NpgsqlDbType.TimestampTz,
        };
    }
}
