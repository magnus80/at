using AT.DataBase;

namespace INAC.Helpers.AAA
{
    public static class Queries
    {
        public static bool IsActiveSession(string login)
        {
            var query = @"select * from vpdn.radacct t where t.ac_username = '" + login + "'";
            var list = Executor.ExecuteSelect(query, Environment.StormDb);

            return list.Count > 0;
        }
    }
}
