using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers.Services.VSU
{
    public static class Queries
    {
        public static string GetMinVsuService(string city)
        {
            var query = @"SELECT s_id 
                          FROM   inac.services 
                               join inac.services_param 
                                 ON s_id = id_service 
                          WHERE  s_city = " + city + @"
                               AND s_f_public = 1 
                               AND param_name = 'VPDN_SPEED_UP' 
                          ORDER  BY param_number ";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);
        }
    }
}
