using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers
{
    public static class Query
    {
        public static List<string> GetFieldsOfTable(string select, string from, string where)
        {
            var query = @"SELECT " + select + @"
                           FROM " + from;
            if (where.Length > 0)
                query += " where " + where;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);
        }
    }
}
