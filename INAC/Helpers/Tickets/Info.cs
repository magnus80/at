using System;
using AT;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.Tickets
{
    public static class Info
    {
        public static string GetTypeText(string t_id)
        {
            var query = @"SELECT tt_type
                          FROM   helpdesk.tickets
                                 JOIN helpdesk.ticket_types
                                   ON t_type = tt_id
                          WHERE  t_id = " + t_id;

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        public static string GetStatusText(string t_id)
        {
            var query = @"SELECT ts_status
                          FROM   helpdesk.tickets
                                 JOIN helpdesk.ticket_statuses
                                   ON t_status = ts_id
                          WHERE  t_id = " + t_id;

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }
    }
}
