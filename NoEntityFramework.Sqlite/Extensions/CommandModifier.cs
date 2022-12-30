using System.Data;

namespace NoEntityFramework.Sqlite
{
    public static class CommandModifier
    {
        public static ISqliteQueryable IsStoredProcedureCommand(this ISqliteQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
