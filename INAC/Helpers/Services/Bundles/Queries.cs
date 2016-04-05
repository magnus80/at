using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;
using AT.Service;
using NUnit.Framework;

namespace INAC.Helpers.Services.Bundles
{
    public static class Queries
    {
        /// <summary>
        /// Выборка бандла с ШПД и wifi арендой (BUNDLE_ONLY_FOR_AUTOTEST_VPDN-WIFI)
        /// </summary>
        /// <returns></returns>
        public static string GetBundleVpdnAndWifiRent()
        {
            var query = @"SELECT s_id
                          FROM   inac.services
                          WHERE  s_name = 'BUNDLE_ONLY_FOR_AUTOTEST_VPDN-WIFI' 
                          ";

            return Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);
        }
    }
}
