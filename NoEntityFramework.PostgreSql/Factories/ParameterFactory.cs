using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace NoEntityFramework.Npgsql
{
    public class ParameterFactory
    {
        /// <summary>
        ///     Create a new <see cref="NpgsqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>A new instance of the <see cref="NpgsqlParameter"/>.</returns>
        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, object value)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType);
            if (value == null) param.Value = DBNull.Value;
            else if (dbType == NpgsqlDbType.Uuid)
            {
                string s = value.ToString();
                if (!string.IsNullOrEmpty(s))
                    param.Value = new SqlGuid(s);
                else
                    param.Value = DBNull.Value;
            }
            else param.Value = value;
            return param;
        }

        /// <summary>
        ///     Create a new <see cref="NpgsqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="value">An <see cref="object"/> that is the value of the <see cref="NpgsqlParameter"/>.</param>
        /// <returns>A new instance of the <see cref="NpgsqlParameter"/>.</returns>
        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, int size, object value)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType, size);
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }

        /// <summary>
        ///     Create a new <see cref="NpgsqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="value">An <see cref="object"/> that is the value of the <see cref="NpgsqlParameter"/>.</param>
        /// <param name="parameterDirection">One of the <see cref="ParameterDirection"/> values.</param>
        /// <returns>A new instance of the <see cref="NpgsqlParameter"/>.</returns>
        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, int size, object value, ParameterDirection parameterDirection)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType, size)
            {
                Direction = parameterDirection
            };
            if (value == null) param.Value = DBNull.Value;
            else param.Value = value;
            return param;
        }

        /// <summary>
        ///     Create a new <see cref="NpgsqlParameter"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="dbType">The type of the parameter</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="parameterDirection">One of the <see cref="ParameterDirection"/> values.</param>
        /// <returns>A new instance of the <see cref="NpgsqlParameter"/>.</returns>
        public static NpgsqlParameter Create(string paramName, NpgsqlDbType dbType, int size, ParameterDirection parameterDirection)
        {
            NpgsqlParameter param = new NpgsqlParameter(paramName, dbType, size)
            {
                Direction = parameterDirection
            };
            return param;
        }
    }
}
