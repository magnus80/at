using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers.Services.Turbo_Button
{
    public static class Queries
    {
        public static string GetTurboSpeedUpService(string city)
        {
            var query = @"SELECT s_id, 
                                   s_name 
                          FROM   inac.services 
                                   join inac.services_param 
                                     ON s_id = id_service 
                          WHERE  s_city = " + city + @" 
                                   AND param_name = 'TURBO_SPEEDUP' 
                                   AND s_f_public = 1";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }
    }
}
