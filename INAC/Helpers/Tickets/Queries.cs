using System;
using AT;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Helpers.Tickets
{
    public static class Queries
    {
        /// <summary>
        /// Выборка рандомного контроллера ГП
        /// </summary>
        /// <returns></returns>
        public static string GetAnyGpController()
        {
            var query = @"
            SELECT DISTINCT param_char 
            FROM   helpdesk.tickets_params 
            WHERE  t_id IN (SELECT gp_id 
                            FROM   helpdesk.global_problems) 
                   AND param_name = 'T_CONTROLLER' 
                   AND param_char IS NOT NULL ";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }
    }
}
