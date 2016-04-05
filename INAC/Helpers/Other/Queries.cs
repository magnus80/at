using AT.DataBase;

namespace INAC.Helpers.Other
{
    public static class Queries
    {
        public static string GetWifiRouterSerial()
        {
          var  query = @"SELECT WFR_SERIAL 
                      FROM INAC.WIFI_ROUTERS 
                      WHERE WFR_RENT_LOGIN IS NULL";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }
    }
}
