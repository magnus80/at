using AT.Tools;

namespace INAC.Helpers.TL
{
    static internal class FttbTechlists
    {
        /// <summary>
        /// Создание техлиста загрузки новых VDPN тарифов
        /// </summary>
        /// <param name="status">
        /// Статус:
        /// A - активный публичный,
        /// N - публичный только для новых,
        /// R - непубличный, подключения только в HD,
        /// P - непубличный для партнеров, подключения только в HD и IDMS
        /// O - архивный,
        /// S - служебный
        /// </param>
        /// <param name="category">
        /// Категория L- лимитный, S - с понижением скорости, U - безлимитный
        /// </param>
        /// <param name="debtPeriod">
        /// период списания абон. платы:
        /// D - ежедневно,
        /// M - ежемесячно
        ///</param>
        /// <returns></returns>
        public static string FormFttbInsertTechlist(string status, string category, string debtPeriod, string price, string serviceName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlFttbInsertPath);

            ex.SetCell("A2", "12042");
            ex.SetCell("B2", "Москва");
            ex.SetCell("C2", status);
            ex.SetCell("E2", serviceName);
            ex.SetCell("F2", category);
            ex.SetCell("G2", debtPeriod);
            ex.SetCell("H2", price);
            ex.SetCell("K2", "3520");
            ex.SetCell("L2", "3520");

            return Techlist.SaveAndClose(ref ex, "fttb_ins");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">
        /// Статус:
        /// A - активный публичный,
        /// N - публичный только для новых,
        /// R - непубличный, подключения только в HD,
        /// P - непубличный для партнеров, подключения только в HD и IDMS
        /// O - архивный,
        /// S - служебный
        /// </param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static string FormFttbUpdateTechlist(string idService,  string status, string serviceName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlFttbUpdatePath);

            ex.SetCell("D2", idService);
            ex.SetCell("C2", status);
            ex.SetCell("E2", serviceName);

            return Techlist.SaveAndClose(ref ex, "fttb_upd");
        }
    }
}
