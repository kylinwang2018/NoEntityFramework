using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace NoEntityFramework.DataAnnotations
{
    /// <summary>
    ///     Annotate a property in the model as a <see cref="SqliteParameter"/>.
    /// </summary>
    public class SqliteDbParameterAttribute : Attribute
    {
        /// <summary>
        ///     The name of the parameter to map.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     One of the <see cref="SqlDbType"/> values.
        /// </summary>
        public SqliteType Type
        {
            get => _type;
            set { 
                _type = value;
                TypeDefined = true;
            }
        }

        private SqliteType _type = SqliteType.Text;

        public bool TypeDefined { get; private set; }

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
        ///     The length of the parameter.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        ///     The name of the source column.
        /// </summary>
        public string? SourceColumn { get; set; }

        /// <summary>
        ///     The total number of decimal places to which the value is resolved.
        /// </summary>
        public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Original;
    }
}