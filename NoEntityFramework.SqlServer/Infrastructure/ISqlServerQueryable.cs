using System;
using System.Data;
using Microsoft.Data.SqlClient;
using NoEntityFramework.DataAnnotations;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     The SQL Server query context for NoEntityFramework.SqlServer.
    /// </summary>
    public interface ISqlServerQueryable : IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="Microsoft.Data.SqlClient.SqlConnection"/>,
        /// <see cref="Microsoft.Data.SqlClient.SqlCommand"/> and <see cref="SqlDataAdapter"/>.
        /// </summary>
        ISqlConnectionFactory ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        ISqlServerLogger Logger { get; }

        /// <summary>
        ///     The <see cref="Microsoft.Data.SqlClient.SqlCommand"/> that is used for the query,
        ///     the Connection property of this <see cref="Microsoft.Data.SqlClient.SqlCommand"/>
        ///     must be set as the <see cref="Microsoft.Data.SqlClient.SqlConnection"/>
        ///     in this <see cref="ISqlServerQueryable"/>.
        /// </summary>
        SqlCommand SqlCommand { get; set; }

        /// <summary>
        ///     The <see cref="Microsoft.Data.SqlClient.SqlConnection"/> that is used for the query.
        /// </summary>
        SqlConnection SqlConnection { get; }

        /// <summary>
        /// <para>
        ///     The object model that can be used as a batch of <see cref="SqlParameter"/> with pre-defined
        ///     <see cref="SqlDbParameterAttribute"/> for each property of the model.
        /// </para>
        /// <para>
        ///     The <see cref="ParameterDirection.Output"/> or <see cref="ParameterDirection.InputOutput"/>
        ///     parameters also can be accessed and get returned value after the query is executed.
        /// </para>
        /// <para>
        ///     Only one <see cref="ParameterModel"/> can be used in single query.
        /// </para>
        /// </summary>
#if NETSTANDARD2_0
        object ParameterModel { get; set; }
#else
        object? ParameterModel { get; set; }
#endif
    }
}