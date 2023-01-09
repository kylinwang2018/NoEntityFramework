using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace NoEntityFramework.DataAnnotations
{
    /// <summary>
    ///     Annotate a property in the model as a <see cref="SqlParameter"/>.
    /// </summary>
    public class SqlDbParameterAttribute : Attribute
    {
        /// <summary>
        ///     The name of the parameter to map.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     One of the <see cref="SqlDbType"/> values.
        /// </summary>
        public SqlDbType DbType { get; set; } = SqlDbType.Variant;

        /// <summary>
        ///     Defines how string comparisons should be performed for this parameter.
        /// </summary>
        public SqlCompareOptions CompareInfo { get; set; } = SqlCompareOptions.None;

        /// <summary>
        ///     One of the <see cref="ParameterDirection"/> values.
        /// </summary>
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        /// <summary>
        ///     <para>
        ///     Enforces encryption of a parameter when using Always Encrypted. If SQL Server
        ///     informs the driver that the parameter does not need to be encrypted, the query
        ///     using the parameter will fail. This property provides additional protection against
        ///     security attacks that involve a compromised SQL Server providing incorrect encryption
        ///     metadata to the client, which may lead to data disclosure.
        ///     </para>
        ///     <para>
        ///     <see langword="true"/> if the parameter has a force column encryption; otherwise, <see langword="false"/>.
        ///     </para>
        /// </summary>
        public bool? ForceColumnEncryption { get; set; }

        /// <summary>
        ///     <see langword="true"/> if the value of the field can be <see langword="null"/>, otherwise <see langword="false"/>.
        /// </summary>
        public bool Nullable { get; set; } = false;

        /// <summary>
        ///     Determines conventions and language for a particular region.
        /// </summary>
        public int? LocaleId { get; set; }

        /// <summary>
        ///    The offset to the value. 
        /// </summary>
        public int? Offset { get; set; }

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
        public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Default;
    }
}