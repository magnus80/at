using AT.DataBase;

namespace INAC.Helpers.Methods
{
    public static class HD
    {
        public static void ClearGlobalProblems()
        {
            var query = @"UPDATE helpdesk.tickets
                          SET    t_status = 15
                          WHERE  t_id IN (SELECT gp_id
                                        FROM   helpdesk.global_problems)";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }

        public static void ClearVipClients()
        {
            var query = @"UPDATE helpdesk.advanced_adr_info
                          SET    vip_client = 0";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
    }
}
