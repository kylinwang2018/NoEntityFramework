using System.Data;

namespace NoEntityFramework.Npgsql
{
    /// <summary>
    ///     Modify the command in the <see cref="IPostgresQueryable"/> that represent the query.
    /// </summary>
    public static class CommandModifier
    {
        /// <summary>
        ///     Change query type to <see cref="CommandType.StoredProcedure"/>.
        /// </summary>
        /// <param name="query">The <see cref="IPostgresQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="IPostgresQueryable"/> that represent the query.</returns>
        public static IPostgresQueryable IsStoredProcedureCommand(this IPostgresQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
