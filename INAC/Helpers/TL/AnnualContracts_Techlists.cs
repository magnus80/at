using AT.Tools;
using NUnit.Framework;

namespace INAC.Helpers.TL
{
    internal static class AnnualContractsTechlists
    {
        public static string FormAnnualContractInsertTechlist(string name, string cityId, string cityName, string visible, string minPrice, int time)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlAnnualInsertPath);

            int i = 1;

            while(i <= time + 1)
            {
                ex.SetCell("A" + (i + 1), "1");
                ex.SetCell("B" + (i + 1), name);
                ex.SetCell("C" + (i + 1), cityId);
                ex.SetCell("D" + (i + 1), cityName);
                ex.SetCell("E" + (i + 1), visible);
                ex.SetCell("F" + (i + 1), "N");
                ex.SetCell("G" + (i + 1), minPrice);
                ex.SetCell("H" + (i + 1), time.ToString());
                if (i != time + 1) ex.SetCell("K" + (i + 1), i.ToString());
                ex.SetCell("L" + (i + 1), (i + 5).ToString());
                i++;
            }
            return Techlist.SaveAndClose(ref ex, "annual_ins");
        }

        public static string FormAnnualContractUpdateTechlist(string serviceId, string cityId, string cityName, string visible, string serviceName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlAnnualUpdatePath);

            ex.SetCell("A2", serviceId);
            ex.SetCell("B2", cityId);
            ex.SetCell("C2", cityName);
            ex.SetCell("D2", visible);
            ex.SetCell("E2", "N");
            ex.SetCell("F2", serviceName);

            return Techlist.SaveAndClose(ref ex, "annual_upd");
        }
    }
}
