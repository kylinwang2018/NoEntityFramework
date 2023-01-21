using System;
using System.Data;
using NoEntityFramework.DataAnnotations;
using NoEntityFramework.Npgsql.Models;
using Npgsql;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     The SQL Server query context for NoEntityFramework.Sqlite.
    /// </summary>
    public interface IPostgresQueryable : IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="NpgsqlConnection"/>,
        /// <see cref="NpgsqlCommand"/> and <see cref="NpgsqlDataAdapter"/>.
        /// </summary>
        IPostgresConnectionFactory ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        IPostgresLogger Logger { get; }

        /// <summary>
        ///     The <see cref="NpgsqlCommand"/> that is used for the query,
        ///     the Connection property of this <see cref="NpgsqlCommand"/>
        ///     must be set as the <see cref="NpgsqlConnection"/>
        ///     in this <see cref="IPostgresQueryable"/>.
        /// </summary>
        NpgsqlCommand SqlCommand { get; set; }

        /// <summary>
        ///     The <see cref="NpgsqlConnection"/> that is used for the query.
        /// </summary>
        NpgsqlConnection SqlConnection { get; }

        /// <summary>
        /// <para>
        ///     The object model that can be used as a batch of <see cref="NpgsqlParameter"/> with pre-defined
        ///     <see cref="PostgresDbParameterAttribute"/> for each property of the model.
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
        NpgsqlRetryLogicOption RetryLogicOption { get; set; }
    }
}