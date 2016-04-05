using AT.Tools;

namespace INAC.Helpers.TL
{
    internal static class BundlesTechlists
    {
        private static void FormEqualString(ref ExcelWorker ex, int index, string cityId, string cityName,
                                            string visible, string effectivePrice, string bundleName)
        {
            ex.SetCell("A" + index, "1");
            ex.SetCell("B" + index, cityId);
            ex.SetCell("C" + index, cityName);
            ex.SetCell("D" + index, visible);
            ex.SetCell("F" + index, effectivePrice);
            ex.SetCell("G" + index, bundleName);
            ex.SetCell("H" + index, "SERVICE_TYPE");
        }

        public static string FromBundlesInsertTechlist(string cityId, string cityName, string visible,
                                                     string effectivePrice, string bundleName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlBundlesInsertPath);

            FormEqualString(ref ex, 2, cityId, cityName, visible, effectivePrice, bundleName);
            FormEqualString(ref ex, 3, cityId, cityName, visible, effectivePrice, bundleName);
            FormEqualString(ref ex, 4, cityId, cityName, visible, effectivePrice, bundleName);
            FormEqualString(ref ex, 5, cityId, cityName, visible, effectivePrice, bundleName);

            ex.SetCell("J2", "VPDN");
            ex.SetCell("J3", "W_STOPPABLE_RENT");
            ex.SetCell("J4", "IPTV");
            ex.SetCell("J5", "STOPPABLE_RENT");

            ex.SetCell("L2", "SWITCH");
            ex.SetCell("L3", "KEEP");
            ex.SetCell("L4", "KEEP");
            ex.SetCell("L5", "KEEP");

            return Techlist.SaveAndClose(ref ex, "bundle_ins");
        }

        public static string FromBundlesUpdateTechlist(string bundleId, string cityId, string cityName, string visible, string bundleName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlBundlesUpdatePath);

            ex.SetCell("A2", bundleId);
            ex.SetCell("B2", cityId);
            ex.SetCell("C2", cityName);
            ex.SetCell("D2", visible);
            ex.SetCell("E2", "Игнорировать");
            ex.SetCell("F2", bundleName);
            ex.SetCell("G2", "Игнорировать");
            ex.SetCell("H2", "Игнорировать");
            ex.SetCell("I2", "Игнорировать");
            ex.SetCell("J2", "Игнорировать");

            return Techlist.SaveAndClose(ref ex, "bundle_upd");
        }
    }
}
