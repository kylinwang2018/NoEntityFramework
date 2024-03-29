﻿using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using NoEntityFramework.Sqlite.Models;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     The instances which the database context required.
    /// </summary>
    /// <typeparam name="TDbContext">
    ///     The type of the database context used.
    /// </typeparam>
    public interface ISqliteOptions<out TDbContext>
        where TDbContext : class, ISqliteDbContext
    {
        /// <summary>
        ///     Provide a connection factory for create <see cref="SqliteConnection"/>,
        /// <see cref="SqliteCommand"/> and <see cref="SqliteDataAdapter"/>.
        /// </summary>
        ISqliteConnectionFactory<TDbContext, RelationalDbOptions> ConnectionFactory { get; }

        /// <summary>
        ///     Provide a logger for logging purpose.
        /// </summary>
        ISqliteLogger<TDbContext> Logger { get; }

        /// <summary>
        ///     Provide options for this database context.
        /// </summary>
        IOptionsMonitor<RelationalDbOptions> Options { get; }

        /// <summary>
        ///     Provide a retry logic for the query.
        /// </summary>
        SqliteRetryLogicOption RetryLogicOption { get; }
    }
}