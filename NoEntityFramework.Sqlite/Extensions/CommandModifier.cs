using System.Data;

namespace NoEntityFramework.Sqlite
{
    /// <summary>
    ///     Modify the command in the <see cref="ISqliteQueryable"/> that represent the query.
    /// </summary>
    public static class CommandModifier
    {
        /// <summary>
        ///     Change query type to <see cref="CommandType.StoredProcedure"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqliteQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="ISqliteQueryable"/> that represent the query.</returns>
        public static ISqliteQueryable IsStoredProcedureCommand(this ISqliteQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
