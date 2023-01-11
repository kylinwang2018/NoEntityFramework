using Microsoft.Data.SqlClient;
using NoEntityFramework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Add to Parameter to selected <see cref="SqlCommand"/>.
    /// </summary>
    public static class ParameterAttacher
    {
        /// <summary>
        ///     Add one <see cref="SqlParameter"/> to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="parameter">The <see cref="SqlParameter"/> that want to add to.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqlServerQueryable WithParameter(
            this ISqlServerQueryable queryable, SqlParameter parameter)
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
        ///     Add multiple <see cref="SqlParameter"/>s to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="parameters">The <see cref="SqlParameter"/>s that want to add to.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqlServerQueryable WithParameters(
            this ISqlServerQueryable queryable, params SqlParameter[] parameters)
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
        ///     Add parameters in a object model to <see cref="SqlCommand"/>. The parameters must be defined with <see cref="SqlDbParameterAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="parameterModel">The model object.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqlServerQueryable WithParameter<T>(
            this ISqlServerQueryable queryable, T parameterModel)
            where T : class, new()
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var properties = typeof(T).GetProperties()
                .Where(s => s.CanRead && s.GetCustomAttributes<SqlDbParameterAttribute>().Any())
                .ToList();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes<SqlDbParameterAttribute>().First();
                var propertyType = property.PropertyType;
                if (string.IsNullOrEmpty(attr.Name))
                    throw new ArgumentNullException(nameof(attr.Name));

                var sqlParameter = new SqlParameter()
                {
                    SqlDbType = (attr.Type == SqlDbType.Variant) ? TypeMap[propertyType] : attr.Type,
                    Direction = attr.Direction,
                    Value = property.GetValue(parameterModel) ?? DBNull.Value,
                    ParameterName = attr.Name,
                    IsNullable = attr.Nullable,
                    Scale = attr.Scale ?? 0,
                    SourceColumn = attr.SourceColumn,
                    Offset = attr.Offset ?? 0,
                    ForceColumnEncryption = attr.ForceColumnEncryption ?? false,
                    Precision = attr.Precision ?? 0,
                    SourceVersion = attr.SourceVersion,
                    CompareInfo = attr.CompareInfo
                };

                if (attr.Size != null)
                    sqlParameter.Size = attr.Size.Value;
                if (attr.LocaleId != null)
                    sqlParameter.LocaleId = attr.LocaleId.Value;

                queryable.SqlCommand.Parameters.Add(sqlParameter);
            }

            queryable.ParameterModel = parameterModel;

            return queryable;
        }

        /// <summary>
        ///     After executed the query, some parameters may need to be returned with a value, call this member can copy these kind
        ///     of parameters' value return to the model.
        /// </summary>
        /// <param name="sqlCommand">The <see cref="SqlCommand"/> that is used to execute the query.</param>
        /// <param name="parameterModel">The parameter model that is used to execute the query.</param>
        /// <returns>The parameter model that is used to execute the query.</returns>
        public static object CopyParameterValueToModels(
            this SqlCommand sqlCommand, object parameterModel)
        {
            var properties = parameterModel.GetType().GetProperties()
            .Where(s => s.CanWrite &&
                s.GetCustomAttributes<SqlDbParameterAttribute>().Any() &&
                s.GetCustomAttributes<SqlDbParameterAttribute>().First().Direction != ParameterDirection.Input)
            .ToList();

            foreach (var property in properties)
            {
                property.SetValue(parameterModel, sqlCommand.Parameters[property.Name].Value);
            }

            return parameterModel;
        }

        /// <summary>
        ///     Create and add one <see cref="SqlParameter"/> to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="parameterDirection">The <see cref="ParameterDirection"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ISqlServerQueryable WithParameter(
            this ISqlServerQueryable queryable,
            string paramName, SqlDbType dbType, ParameterDirection parameterDirection, object? value, int? size)
        {
            if (queryable.SqlCommand == null)
                throw new ArgumentNullException(nameof(queryable.SqlCommand));

            var param = size == null ?
                new SqlParameter(paramName, dbType) : new SqlParameter(paramName, dbType, (int)size);

            param.Direction = parameterDirection;

            if (value == null) param.Value = DBNull.Value;
            else if (dbType == SqlDbType.UniqueIdentifier)
            {
                var s = value.ToString();
                if (!string.IsNullOrEmpty(s))
                    param.Value = new SqlGuid(s);
                else
                    param.Value = DBNull.Value;
            }
            else
                param.Value = value;

            queryable.SqlCommand.Parameters.Add(param);
            return queryable;
        }

        /// <summary>
        ///     Create and add one input only <see cref="SqlParameter"/> to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static ISqlServerQueryable WithInputParameter(
            this ISqlServerQueryable queryable, string paramName, SqlDbType dbType, object value)
        {
            return WithInputParameter(queryable, paramName, dbType, value, null);
        }

        /// <summary>
        ///     Create and add one input only <see cref="SqlParameter"/> to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static ISqlServerQueryable WithInputParameter(
            this ISqlServerQueryable queryable, string paramName, SqlDbType dbType, object value, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Input, value, size);
        }

        /// <summary>
        ///     Create and add one output only <see cref="SqlParameter"/> to <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="dbType">The <see cref="SqlDbType"/> of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static ISqlServerQueryable WithOutputParameter(
            this ISqlServerQueryable queryable, string paramName, SqlDbType dbType, int? size)
        {
            return WithParameter(queryable, paramName, dbType, ParameterDirection.Output, null, size);
        }

        private static readonly Dictionary<Type, SqlDbType> TypeMap = new Dictionary<Type, SqlDbType>
        {
            [typeof(byte)] = SqlDbType.TinyInt,
            [typeof(sbyte)] = SqlDbType.TinyInt,
            [typeof(short)] = SqlDbType.SmallInt,
            [typeof(ushort)] = SqlDbType.SmallInt,
            [typeof(int)] = SqlDbType.Int,
            [typeof(uint)] = SqlDbType.Int,
            [typeof(long)] = SqlDbType.BigInt,
            [typeof(ulong)] = SqlDbType.BigInt,
            [typeof(float)] = SqlDbType.Real,
            [typeof(double)] = SqlDbType.Float,
            [typeof(decimal)] = SqlDbType.Decimal,
            [typeof(bool)] = SqlDbType.Bit,
            [typeof(string)] = SqlDbType.NVarChar,
            [typeof(char)] = SqlDbType.Char,
            [typeof(Guid)] = SqlDbType.UniqueIdentifier,
            [typeof(DateTime)] = SqlDbType.DateTime,
            [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
            [typeof(byte[])] = SqlDbType.Binary,
            [typeof(byte?)] = SqlDbType.TinyInt,
            [typeof(sbyte?)] = SqlDbType.TinyInt,
            [typeof(short?)] = SqlDbType.SmallInt,
            [typeof(ushort?)] = SqlDbType.SmallInt,
            [typeof(int?)] = SqlDbType.Int,
            [typeof(uint?)] = SqlDbType.Int,
            [typeof(long?)] = SqlDbType.BigInt,
            [typeof(ulong?)] = SqlDbType.BigInt,
            [typeof(float?)] = SqlDbType.Real,
            [typeof(double?)] = SqlDbType.Float,
            [typeof(decimal?)] = SqlDbType.Decimal,
            [typeof(bool?)] = SqlDbType.Bit,
            [typeof(char?)] = SqlDbType.Char,
            [typeof(Guid?)] = SqlDbType.UniqueIdentifier,
            [typeof(DateTime?)] = SqlDbType.DateTime,
            [typeof(DateTimeOffset?)] = SqlDbType.DateTimeOffset,
        };
    }
}
