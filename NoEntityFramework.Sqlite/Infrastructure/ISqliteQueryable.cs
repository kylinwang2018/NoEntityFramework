using System;
using System.Data;
using Microsoft.Data.Sqlite;
using NoEntityFramework.DataAnnotations;
using NoEntityFramework.Sqlite.Models;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     The SQL Server query context for NoEntityFramework.Sqlite.
    /// </summary>
    public interface ISqliteQueryable : IDisposable, IAsyncDisposable
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="SqliteConnection"/>,
        /// <see cref="SqliteCommand"/> and <see cref="SqliteDataAdapter"/>.
        /// </summary>
        ISqliteConnectionFactory ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        ISqliteLogger Logger { get; }

        /// <summary>
        ///     The <see cref="SqliteCommand"/> that is used for the query,
        ///     the Connection property of this <see cref="SqliteCommand"/>
        ///     must be set as the <see cref="SqliteConnection"/>
        ///     in this <see cref="ISqliteQueryable"/>.
        /// </summary>
        SqliteCommand SqlCommand { get; set; }

        /// <summary>
        ///     The <see cref="SqliteConnection"/> that is used for the query.
        /// </summary>
        SqliteConnection SqlConnection { get; }

        /// <summary>
        /// <para>
        ///     The object model that can be used as a batch of <see cref="SqliteParameter"/> with pre-defined
        ///     <see cref="SqliteDbParameterAttribute"/> for each property of the model.
        /// </para>
        /// <para>
        ///     The <see cref="ParameterDirection.Output"/> or <see cref="ParameterDirection.InputOutput"/>
        ///     parameters also can be accessed and get returned value after the query is executed.
        /// </para>
        /// <para>
        ///     Only one <see cref="ParameterModel"/> can be used in single query.
        /// </para>
        /// </summary>
        object? ParameterModel { get; set; }

        /// <summary>
        ///     The retry logic for the query.
        /// </summary>
        SqliteRetryLogicOption RetryLogicOption { get; set; }
    }
}