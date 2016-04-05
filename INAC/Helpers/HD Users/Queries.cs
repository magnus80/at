using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.HD_Users
{
    public static class Queries
    {
        public static string GetHdGoUserLogin()
        {
            var query = @"SELECT u_login
                          FROM   helpdesk.users
                          WHERE  u_status = 1 and u_gp_view = 1";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        public static List<string> GetHdUser(int status)
        {
            var query = @"SELECT u_login, u_password
                          FROM   helpdesk.users
                          WHERE  u_gp_view = 1 and u_status = " + status;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();
        }
    }
}
