using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace NoEntityFramework.DataAnnotations
{
    /// <summary>
    ///     Annotate a property in the model as a <see cref="NpgsqlParameter"/>.
    /// </summary>
    public class PostgresDbParameterAttribute : Attribute
    {
        /// <summary>
        ///     The name of the parameter to map.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     One of the <see cref="NpgsqlDbType"/> values.
        /// </summary>
        public NpgsqlDbType Type { get; set; } = NpgsqlDbType.Unknown;

        /// <summary>
        ///     The length of the parameter.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        ///     The name of the source column.
        /// </summary>
        public string? SourceColumn { get; set; }

        /// <summary>
        ///     One of the <see cref="ParameterDirection"/> values.
        /// </summary>
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        /// <summary>
        ///     <see langword="true"/> if the value of the field can be <see langword="null"/>, otherwise <see langword="false"/>.
        /// </summary>
        public bool Nullable { get; set; } = false;

        /// <summary>
        ///     The total number of digits to the left and right of the decimal point to which the value is resolved.
        /// </summary>
        public byte? Precision { get; set; }

        /// <summary>
        ///     The total number of decimal places to which teh value is resolved.
        /// </summary>
        public byte? Scale { get; set; }

        /// <summary>
        ///     The total number of decimal places to which the value is resolved.
        /// </summary>
        public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Default;
    }
}