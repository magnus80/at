using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.Tools;

namespace INAC.Helpers.TL
{
    internal static class  NetphoneTechlists
    {
        public static string FormNetphoneServicesInsertTechlist(string cityId, string cityName, string visible, string serviceName, string price, string atlId)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlNetphoneInsertPath);

            ex.SetCell("A2", cityId);
            ex.SetCell("B2", cityName);
            ex.SetCell("C2", serviceName);
            ex.SetCell("D2", atlId);
            ex.SetCell("E2", price);
            ex.SetCell("F2", visible);

            return Techlist.SaveAndClose(ref ex, "netphone_serv_ins");
        }

        public static string FormNetphoneServicesUpdateTechlist(string serviceId, string serviceName, string visible)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlNetphoneUpdatePath);

            ex.SetCell("A2", serviceId);
            ex.SetCell("B2", serviceName);
            ex.SetCell("C2", visible);

            return Techlist.SaveAndClose(ref ex, "netphone_serv_upd");
        }
    }
}
