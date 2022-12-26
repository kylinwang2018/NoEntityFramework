using System.Data;

namespace NoEntityFramework.SqlServer.Extensions
{
    public static class CommandModifier
    {
        public static ISqlServerQueryable IsStoredProcedureCommand(this ISqlServerQueryable query)
        {
            query.SqlCommand.CommandType = CommandType.StoredProcedure;
            return query;
        }
    }
}
