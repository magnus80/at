using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers.Services.IPTV
{
    public static class Queries
    {
        public static string GetIptvGuid()
        {
            var query = @"SELECT ig_id
                      FROM   inac.iptv_guids
                      WHERE  ig_login IS NULL
                           AND ig_rent_login IS NULL
                           AND ig_name = 'VIP1216 EMEA-MS' 
                           AND ig_spl_service is null";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        public static string GetIptvPacket(string city)
        {
            var query = @"SELECT DISTINCT pkt_id
                      FROM   inac.iptv_packets_cap
                           JOIN inac.iptv_packets_param
                             ON pkt_id = id_packet
                      WHERE  pkt_parent IS NULL
                           AND pkt_city_id = " + city + @"
                      ORDER  BY pkt_id desc";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        /// <summary>
        /// выборка иптв пакетов в составе базового
        /// </summary>
        /// <param name="pkt_base_id">ид базового пакета</param>
        /// <returns></returns>
        public static List<string> GetIptvPacketsByBaseMinusEx(string pkt_base_id)
        {
            var query = @"SELECT pkt_id
                      FROM   inac.iptv_packets
                      WHERE  pkt_parent IN(SELECT pkt_id
                                         FROM   inac.iptv_packets
                                         WHERE  pkt_parent IN (SELECT pkt_id
                                                               FROM   inac.iptv_packets
                                                               WHERE  pkt_parent = " + pkt_base_id + @"))
                      MINUS
                      SELECT pkt_id
                      FROM   inac.iptv_packets_excludes
                      WHERE  base_id = " + pkt_base_id + @"
                      MINUS
                      SELECT pkt_id_ex
                      FROM   inac.iptv_packets_excludes
                      WHERE  base_id = " + pkt_base_id;

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0);
            
        }
    }
}
