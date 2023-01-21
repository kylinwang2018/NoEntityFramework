using System;
using System.Data;
using MySql.Data.MySqlClient;
using NoEntityFramework.DataAnnotations;
using NoEntityFramework.MySql.Models;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     The SQL Server query context for NoEntityFramework.Sqlite.
    /// </summary>
    public interface IMySqlQueryable : IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="MySqlConnection"/>,
        /// <see cref="MySqlCommand"/> and <see cref="MySqlDataAdapter"/>.
        /// </summary>
        IMySqlConnectionFactory ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        IMySqlLogger Logger { get; }

        /// <summary>
        ///     The <see cref="MySqlCommand"/> that is used for the query,
        ///     the Connection property of this <see cref="MySqlCommand"/>
        ///     must be set as the <see cref="MySqlConnection"/>
        ///     in this <see cref="IMySqlQueryable"/>.
        /// </summary>
        MySqlCommand SqlCommand { get; set; }

        /// <summary>
        ///     The <see cref="MySqlConnection"/> that is used for the query.
        /// </summary>
        MySqlConnection SqlConnection { get; }

        /// <summary>
        /// <para>
        ///     The object model that can be used as a batch of <see cref="MySqlParameter"/> with pre-defined
        ///     <see cref="MySqlDbParameterAttribute"/> for each property of the model.
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

        /// <summary>
        ///     The retry logic for the query.
        /// </summary>
        MySqlRetryLogicOption RetryLogicOption { get; set; }
    }
}