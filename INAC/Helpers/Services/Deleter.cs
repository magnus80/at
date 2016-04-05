using AT.DataBase;

namespace INAC.Helpers.Services
{
    internal static class Deleter
    {
        public static void DeleteServicesFromTempCity()
        {
            var query =
                @"DELETE FROM inac.services_param
                  WHERE  id_service IN (SELECT s_id
                                        FROM   inac.services
                                        WHERE  s_city = " + Environment.TempCityId + ")"; 
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            query = @"DELETE FROM inac.vpdn
                      WHERE  v_service IN (SELECT s_id
                                      FROM   inac.services
                                      WHERE  s_city = " + Environment.TempCityId + ")";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            query = @"DELETE FROM inac.services
                      WHERE  s_id IN (SELECT s_id
                                      FROM   inac.services
                                      WHERE  s_city = " + Environment.TempCityId + ")";
            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
    }
}
