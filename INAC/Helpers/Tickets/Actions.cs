using AT.DataBase;

namespace INAC.Helpers.Tickets
{
    public static class Actions
    {
        public static void LintToGlobalProblem(string t_id, string gp_id)
        {
            var query = @"
            UPDATE helpdesk.tickets 
            SET    t_global = '" + gp_id + @"'  
            WHERE  t_id = " + t_id;

            Executor.ExecuteSelect(query, Environment.InacDb);
        }
    }
}
