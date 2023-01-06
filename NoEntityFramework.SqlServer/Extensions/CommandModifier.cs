using System.Data;

namespace NoEntityFramework.SqlServer
{
    /// <summary>
    ///     Modify the command in the <see cref="ISqlServerQueryable"/> that represent the query.
    /// </summary>
    public static class CommandModifier
    {
        /// <summary>
        ///     Change query type to <see cref="CommandType.StoredProcedure"/>.
        /// </summary>
        /// <param name="query">The <see cref="ISqlServerQueryable"/> that represent the query.</param>
        /// <returns>The <see cref="ISqlServerQueryable"/> that represent the query.</returns>
        public static ISqlServerQueryable IsStoredProcedureCommand(this ISqlServerQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
