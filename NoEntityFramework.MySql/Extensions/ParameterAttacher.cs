using NoEntityFramework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     Add to Parameter to selected <see cref="MySqlCommand"/>.
    /// </summary>
    public static class ParameterAttacher
    {
        /// <summary>
        ///     Add one <see cref="MySqlParameter"/> to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="parameter">The <see cref="MySqlParameter"/> that want to add to.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMySqlQueryable WithParameter(
            this IMySqlQueryable queryable, MySqlParameter parameter)
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
        ///     Add multiple <see cref="MySqlParameter"/>s to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="parameters">The <see cref="MySqlParameter"/>s that want to add to.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMySqlQueryable WithParameters(
            this IMySqlQueryable queryable, params MySqlParameter[] parameters)
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
        ///     Add parameters in a object model to <see cref="MySqlCommand"/>. The parameters must be defined with <see cref="PostgresDbParameterAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="parameterModel">The model object.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMySqlQueryable WithParameter<T>(
            this IMySqlQueryable queryable, T parameterModel)
            where T : class, new()
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var properties = typeof(T).GetProperties()
                .Where(s => s.CanRead && s.GetCustomAttributes<MySqlDbParameterAttribute>().Any())
                .ToList();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes<MySqlDbParameterAttribute>().First();
                var propertyType = property.PropertyType;
                if (string.IsNullOrEmpty(attr.Name))
                    throw new ArgumentNullException(nameof(attr.Name));

                var sqlParameter = new MySqlParameter()
                {
                    MySqlDbType = attr.TypeDefined ? TypeMap[propertyType] : attr.Type,
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
        /// <param name="sqlCommand">The <see cref="MySqlCommand"/> that is used to execute the query.</param>
        /// <param name="parameterModel">The parameter model that is used to execute the query.</param>
        /// <returns>The parameter model that is used to execute the query.</returns>
        public static object CopyParameterValueToModels(
            this MySqlCommand sqlCommand, object parameterModel)
        {
            var properties = parameterModel.GetType().GetProperties()
            .Where(s => s.CanWrite &&
                s.GetCustomAttributes<MySqlDbParameterAttribute>().Any() &&
                s.GetCustomAttributes<MySqlDbParameterAttribute>().First().Direction != ParameterDirection.Input)
            .ToList();

            foreach (var property in properties)
            {
                property.SetValue(parameterModel, sqlCommand.Parameters[property.Name].Value);
            }

            return parameterModel;
        }

        /// <summary>
        ///     Create and add one <see cref="MySqlParameter"/> to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="MySqlDbType"/> of the parameter.</param>
        /// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IMySqlQueryable WithParameter(
            this IMySqlQueryable queryable,
#if NETSTANDARD2_0
            string paramName, MySqlDbType dbType, ParameterDirection parameterDirection, object value, int? size)
#else
            string paramName, MySqlDbType dbType, ParameterDirection parameterDirection, object? value, int? size)
#endif
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var param = size == null ?
                new MySqlParameter(paramName, dbType) : new MySqlParameter(paramName, dbType, (int)size);

            param.Direction = parameterDirection;

            param.Value = value ?? DBNull.Value;

            queryable.SqlCommand.Parameters.Add(param);
            return queryable;
        }

        /// <summary>
        ///     Create and add one input only <see cref="MySqlParameter"/> to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="MySqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        public static IMySqlQueryable WithInputParameter(
            this IMySqlQueryable queryable, string paramName, MySqlDbType dbType, object value)
        {
            return WithInputParameter(queryable, paramName, dbType, value, null);
        }

        /// <summary>
        ///     Create and add one input only <see cref="MySqlParameter"/> to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="MySqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        public static IMySqlQueryable WithInputParameter(
            this IMySqlQueryable queryable, string paramName, MySqlDbType dbType, object value, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Input, value, size);
        }

        /// <summary>
        ///     Create and add one output only <see cref="MySqlParameter"/> to <see cref="MySqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="MySqlDbType"/> of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        public static IMySqlQueryable WithOutputParameter(
            this IMySqlQueryable queryable, string paramName, MySqlDbType dbType, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Output, null, size);
        }

        private static readonly Dictionary<Type, MySqlDbType> TypeMap = new Dictionary<Type, MySqlDbType>
        {
            [typeof(byte)] = MySqlDbType.Byte,
            [typeof(sbyte)] = MySqlDbType.Byte,
            [typeof(short)] = MySqlDbType.Int16,
            [typeof(ushort)] = MySqlDbType.UInt16,
            [typeof(int)] = MySqlDbType.Int32,
            [typeof(uint)] = MySqlDbType.UInt32,
            [typeof(long)] = MySqlDbType.Int64,
            [typeof(ulong)] = MySqlDbType.UInt64,
            [typeof(float)] = MySqlDbType.Float,
            [typeof(double)] = MySqlDbType.Double,
            [typeof(decimal)] = MySqlDbType.Decimal,
            [typeof(bool)] = MySqlDbType.Bit,
            [typeof(string)] = MySqlDbType.VarString,
            [typeof(char)] = MySqlDbType.Text,
            [typeof(Guid)] = MySqlDbType.Guid,
            [typeof(DateTime)] = MySqlDbType.DateTime,
            [typeof(DateTimeOffset)] = MySqlDbType.DateTime,
            [typeof(byte[])] = MySqlDbType.Byte,
            [typeof(byte?)] = MySqlDbType.Byte,
            [typeof(sbyte?)] = MySqlDbType.Byte,
            [typeof(short?)] = MySqlDbType.Int16,
            [typeof(ushort?)] = MySqlDbType.UInt16,
            [typeof(int?)] = MySqlDbType.Int32,
            [typeof(uint?)] = MySqlDbType.UInt32,
            [typeof(long?)] = MySqlDbType.Int64,
            [typeof(ulong?)] = MySqlDbType.UInt64,
            [typeof(float?)] = MySqlDbType.Float,
            [typeof(double?)] = MySqlDbType.Double,
            [typeof(decimal?)] = MySqlDbType.Decimal,
            [typeof(bool?)] = MySqlDbType.Bit,
            [typeof(char?)] = MySqlDbType.Text,
            [typeof(Guid?)] = MySqlDbType.Guid,
            [typeof(DateTime?)] = MySqlDbType.DateTime,
            [typeof(DateTimeOffset?)] = MySqlDbType.DateTime,
        };
    }
}
