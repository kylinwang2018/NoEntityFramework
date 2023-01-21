using System.Data;

namespace NoEntityFramework.MySql
{
    /// <summary>
    ///     Modify the command in the <see cref="IMySqlQueryable"/> that represent the query.
    /// </summary>
    public static class CommandModifier
    {
        /// <summary>
        ///     Change query type to <see cref="CommandType.StoredProcedure"/>.
        /// </summary>
        /// <param name="query">The <see cref="IMySqlQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="IMySqlQueryable"/> that represent the query.</returns>
        public static IMySqlQueryable IsStoredProcedureCommand(this IMySqlQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
