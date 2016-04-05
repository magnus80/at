using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.Adresses
{
    public static class Queries
    {
        public static string GetConnectedStreet(string city)
        {
            var query = @"SELECT DISTINCT s_id 
            FROM   inac.streets0 s 
                   JOIN inac.houses0 h 
                     ON s.s_id = h.h_street 
            WHERE  h_status = 'connected' 
            AND s_city = " + city;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        public static string GetConnectedHouse(string street)
        {
            var query = @"SELECT DISTINCT h_id, 
                            s_id, 
                            h_id 
            FROM   inac.houses0 
                   JOIN inac.streets0 
                     ON s_id = h_street 
            WHERE  h_status = 'connected' and s_id = " + street;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        public static string GetStreetName(string s_id)
        {
            var query = @" SELECT DISTINCT s_street 
            FROM   inac.streets0  
            WHERE  s_id = " + s_id;

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        /// <summary>
        /// выбирает любой район (ид) в указанном городе
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetArea(string city)
        {
            var query = @" SELECT DISTINCT ar_id 
            FROM   inac.areas 
            WHERE  ar_city = " + city;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }
    }
}
