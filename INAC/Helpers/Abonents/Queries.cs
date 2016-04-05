using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static class Queries
    {
        public static string GetAbonentForAAA(string block_type)
        {
            var query = @"SELECT l_login
                          FROM   inac.services_per_login
                                 JOIN inac.logins
                                   ON spl_login = l_login
                                 JOIN inac.contracts
                                   ON c_id = l_contract
                          WHERE  spl_service IN (SELECT s_id
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
                                                          ON s_id = id_service
                                                 WHERE  param_name = 'CONNECT_TYPE'
                                                        AND param_char = 'PPTP')
                                 AND l_block_type = " + block_type + @"
                                 AND l_login LIKE '089%'
                                 AND sysdate - spl_start > 1
                                 AND c_payed - c_uses > 1000 and l_password = 'password1'
                          ORDER  BY dbms_random.value 
                            ";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }


        /// <summary>
        /// Выборка ОТТ абонента
        /// </summary>
        /// <returns>логин</returns>
        public static string GetOTTLogin()
        {
            var query = @"SELECT l_login 
                        FROM   inac.logins 
                               join inac.contract_info 
                                 ON ci_id = l_contract 
                        WHERE  ci_jur_depend = 13 
                               AND ROWNUM = 1";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// Выборка абонентов
        /// </summary>
        /// <param name="fields"> список полей для выборки, например "l_login, l_password" </param>
        /// <param name="other_where">доп. условия, например "l_block_type = 0 and c_cmc_warnto like ('9%')"</param>
        /// <returns>Вовзращает список физ. клиентов из Москвы по указанным критериям </returns>
        public static List<string> GetAnyClient(string fields, string where)
        {
            var query = @"
            SELECT " + fields + @" 
            FROM   inac.contracts c 
                   JOIN inac.addresses0 a  
                     ON c.c_address0 = a.a_id  
                   JOIN inac.houses0 h  
                     ON a.a_house = h.h_id  
                   JOIN inac.areas ar  
                     ON h.h_dealer = ar.ar_id  
                   JOIN inac.cities0 ct  
                     ON ct.ct_id = ar.ar_city  
                   JOIN inac.logins l  
                     ON c.c_id = l.l_contract  
            WHERE  ct.ct_id = 12042  
                    AND (c.c_juridical = 0 OR c.c_juridical IS NULL)  
                   AND l.l_login LIKE ( '%089%' )  
                   AND c.c_address0 is not null ";

            if (!where.Equals("")) query += "and " + where;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();
        }
    }
}
