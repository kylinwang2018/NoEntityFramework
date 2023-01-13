using Microsoft.Data.Sqlite;
using NoEntityFramework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Add to Parameter to selected <see cref="SqliteCommand"/>.
    /// </summary>
    public static class ParameterAttacher
    {
        /// <summary>
        ///     Add one <see cref="SqliteParameter"/> to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="parameter">The <see cref="SqliteParameter"/> that want to add to.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqliteQueryable WithParameter(
            this ISqliteQueryable queryable, SqliteParameter parameter)
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
        ///     Add multiple <see cref="SqliteParameter"/>s to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="parameters">The <see cref="SqliteParameter"/>s that want to add to.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqliteQueryable WithParameters(
            this ISqliteQueryable queryable, params SqliteParameter[] parameters)
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
        ///     Add parameters in a object model to <see cref="SqliteCommand"/>. The parameters must be defined with <see cref="SqliteDbParameterAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="parameterModel">The model object.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqliteQueryable WithParameter<T>(
            this ISqliteQueryable queryable, T parameterModel)
            where T : class, new()
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var properties = typeof(T).GetProperties()
                .Where(s => s.CanRead && s.GetCustomAttributes<SqliteDbParameterAttribute>().Any())
                .ToList();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes<SqliteDbParameterAttribute>().First();
                var propertyType = property.PropertyType;
                if (string.IsNullOrEmpty(attr.Name))
                    throw new ArgumentNullException(nameof(attr.Name));

                var sqlParameter = new SqliteParameter()
                {
                    SqliteType = attr.TypeDefined ? attr.Type : TypeMap[propertyType],
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
        /// <param name="sqlCommand">The <see cref="SqliteCommand"/> that is used to execute the query.</param>
        /// <param name="parameterModel">The parameter model that is used to execute the query.</param>
        /// <returns>The parameter model that is used to execute the query.</returns>
        public static object CopyParameterValueToModels(
            this SqliteCommand sqlCommand, object parameterModel)
        {
            var properties = parameterModel.GetType().GetProperties()
            .Where(s => s.CanWrite &&
                s.GetCustomAttributes<SqliteDbParameterAttribute>().Any() &&
                s.GetCustomAttributes<SqliteDbParameterAttribute>().First().Direction != ParameterDirection.Input)
            .ToList();

            foreach (var property in properties)
            {
                property.SetValue(parameterModel, sqlCommand.Parameters[property.Name].Value);
            }

            return parameterModel;
        }

        /// <summary>
        ///     Create and add one <see cref="SqliteParameter"/> to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqliteQueryable WithParameter(
            this ISqliteQueryable queryable,
            string paramName, SqliteType dbType, ParameterDirection parameterDirection, object? value, int? size)
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var param = size == null ?
                new SqliteParameter(paramName, dbType) : new SqliteParameter(paramName, dbType, (int)size);

            param.Direction = parameterDirection;

            param.Value = value ?? DBNull.Value;

            queryable.SqlCommand.Parameters.Add(param);
            return queryable;
        }

        /// <summary>
        ///     Create and add one input only <see cref="SqliteParameter"/> to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        public static ISqliteQueryable WithInputParameter(
            this ISqliteQueryable queryable, string paramName, SqliteType dbType, object value)
        {
            return WithInputParameter(queryable, paramName, dbType, value, null);
        }

        /// <summary>
        ///     Create and add one input only <see cref="SqliteParameter"/> to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        public static ISqliteQueryable WithInputParameter(
            this ISqliteQueryable queryable, string paramName, SqliteType dbType, object value, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Input, value, size);
        }

        /// <summary>
        ///     Create and add one output only <see cref="SqliteParameter"/> to <see cref="SqliteCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        public static ISqliteQueryable WithOutputParameter(
            this ISqliteQueryable queryable, string paramName, SqliteType dbType, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Output, null, size);
        }

        private static readonly Dictionary<Type, SqliteType> TypeMap = new Dictionary<Type, SqliteType>
        {
            [typeof(byte)] = SqliteType.Integer,
            [typeof(sbyte)] = SqliteType.Integer,
            [typeof(short)] = SqliteType.Integer,
            [typeof(ushort)] = SqliteType.Integer,
            [typeof(int)] = SqliteType.Integer,
            [typeof(uint)] = SqliteType.Integer,
            [typeof(long)] = SqliteType.Integer,
            [typeof(ulong)] = SqliteType.Integer,
            [typeof(float)] = SqliteType.Real,
            [typeof(double)] = SqliteType.Real,
            [typeof(decimal)] = SqliteType.Real,
            [typeof(bool)] = SqliteType.Integer,
            [typeof(string)] = SqliteType.Text,
            [typeof(char)] = SqliteType.Text,
            [typeof(Guid)] = SqliteType.Text,
            [typeof(DateTime)] = SqliteType.Text,
            [typeof(DateTimeOffset)] = SqliteType.Text,
            [typeof(byte[])] = SqliteType.Integer,
            [typeof(byte?)] = SqliteType.Integer,
            [typeof(sbyte?)] = SqliteType.Integer,
            [typeof(short?)] = SqliteType.Integer,
            [typeof(ushort?)] = SqliteType.Integer,
            [typeof(int?)] = SqliteType.Integer,
            [typeof(uint?)] = SqliteType.Integer,
            [typeof(long?)] = SqliteType.Integer,
            [typeof(ulong?)] = SqliteType.Integer,
            [typeof(float?)] = SqliteType.Real,
            [typeof(double?)] = SqliteType.Real,
            [typeof(decimal?)] = SqliteType.Real,
            [typeof(bool?)] = SqliteType.Integer,
            [typeof(char?)] = SqliteType.Text,
            [typeof(Guid?)] = SqliteType.Text,
            [typeof(DateTime?)] = SqliteType.Text,
            [typeof(DateTimeOffset?)] = SqliteType.Text,
        };
    }
}
