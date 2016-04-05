using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers.Services.FTTB
{
    public static class Queries
    {
        /// <summary>
        /// Возвращает сервис для ААА (безлимитный, публичный, Москва, PPTP)
        /// </summary>
        /// <returns></returns>
        public static string GetFttbServiceFoAAA()
        {
            var query = @"SELECT s_id
                          FROM   inac.services
                                 JOIN inac.services_param spp1
                                   ON ( s_id = spp1.id_service
                                        AND spp1.param_name = 'BILL_TYPE'
                                        AND spp1.param_number = 1 )
                                 JOIN inac.services_param spp2
                                   ON ( s_id = spp2.id_service
                                        AND spp2.param_name = 'RESERV_TYPE'
                                        AND spp2.param_number = 0 )
                          WHERE  s_city = 12042 
                                 AND s_f_public = 1
                                 AND s_f_vpdn = 1
                          INTERSECT
                          SELECT s_id
                          FROM   inac.services
                                 JOIN inac.services_param
                                   ON  s_id = id_service 
                                        where param_name = 'CONNECT_TYPE'
                                        AND param_char = 'PPTP' 
                          ";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// Возвращает ид публичного сервиса ШПД (с резерированием)
        /// </summary>
        /// <param name="city_id">ид города</param>
        /// <returns>ид сервиса</returns>
        public static string GetFttbServiceUnlim(string city_id)
        {
            var query =
                @"SELECT s_id
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                  WHERE  param_name = 'RESERV_TYPE'
                         AND param_number = '0'
                         AND s_f_vpdn = 1
                         AND s_f_public = 1
                         AND s_city = " + city_id + @"
                  INTERSECT                          
                  SELECT s_id
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                  WHERE  param_name = 'CONNECT_TYPE'
                         AND param_char = 'PPTP'";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// Возвращает ид публичного лимитного сервиса ШПД (без резервирования)
        /// </summary>
        /// <param name="city_id">ид города</param>
        /// <returns>ид сервиса</returns>
        public static string GetFttbServiceLimit(string city_id)
        {
            var query =
                @"SELECT s_id
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                  WHERE  param_name = 'RESERV_TYPE'
                         AND param_number = '1'
                         AND s_f_vpdn = 1
                         AND s_f_public = 1
                         AND s_city = " + city_id + @"
                  INTERSECT                          
                  SELECT s_id
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                  WHERE  param_name = 'CONNECT_TYPE'
                         AND param_char = 'PPTP'";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// Возвращает ID сервиса, для которого доступна опция "выбор скорости"
        /// </summary>
        /// <param name="city_id"></param>
        /// <returns></returns>
        public static string GetFttbServiceForVsu(string city_id)
        {
            var query = @"SELECT s_id
                          FROM   inac.services
                                 JOIN inac.services_param spp1
                                   ON ( s_id = spp1.id_service
                                        AND spp1.param_name = 'BILL_TYPE'
                                        AND spp1.param_number = 1 )
                                 JOIN inac.services_param spp2
                                   ON ( s_id = spp2.id_service
                                        AND spp2.param_name = 'RESERV_TYPE'
                                        AND spp2.param_number = 0 )
                          WHERE  s_city = " + city_id + @"
                                 AND s_f_public = 1
                                 AND s_f_vpdn = 1
                          MINUS
                          SELECT s_id
                          FROM   inac.services
                                 JOIN inac.services_param spp1
                                   ON ( s_id = spp1.id_service
                                        AND spp1.param_name = 'SERVICE_TYPE'
                                        AND spp1.param_char = 'SHAPED' )
                          INTERSECT                          
                          SELECT s_id
                          FROM   inac.services
                              JOIN inac.services_param
                                ON s_id = id_service
                          WHERE  param_name = 'CONNECT_TYPE'
                              AND param_char = 'PPTP'";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }


        public static string GetFttbServiceShaped(string city)
        {
            var query = @"SELECT s_id 
                          FROM   inac.services 
                               join inac.services_param 
                                 ON s_id = id_service 
                          WHERE  param_name = 'CONNECT_TYPE' 
                               AND param_char = 'PPTP' 
                               AND s_city = " + city + @" 
                               AND s_f_public = 1 
                               AND s_f_vpdn = 1 
                          INTERSECT 
                          SELECT id_service 
                          FROM   inac.services_param 
                          WHERE  param_char = 'SHAPED' 
                          INTERSECT                          
                          SELECT s_id
                          FROM   inac.services
                              JOIN inac.services_param
                                ON s_id = id_service
                          WHERE  param_name = 'CONNECT_TYPE'
                              AND param_char = 'PPTP'";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// Возвращает значение параметра из таблицы inac.vpdn
        /// </summary>
        /// <param name="service">сервис</param>
        /// <param name="param">название столбца</param>
        /// <returns></returns>
        public static string GetFromVpdn(string service, string param)
        {
            return Query.GetFieldsOfTable(param, "inac.vpdn", "v_service = '" + service + "'")[0];
          /*  var query =
                @"SELECT " + param + @"
                  FROM   inac.vpdn
                  WHERE  v_service = '" + service + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0,0]; */
        }
    }
}
