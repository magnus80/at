using System.IO;

namespace INAC
{
    static internal class Environment
    {
        public static string InacDb = "billft3";
        public static string StormDb = "stormft3";

        #region techlists path

        public static string TlFttbInsertPath = Directory.GetCurrentDirectory() + "\\TL\\fttb_insert.xls";
        public static string TlFttbUpdatePath = Directory.GetCurrentDirectory() + "\\TL\\fttb_update.xls";
        public static string TlIptvServicesInsertPath = Directory.GetCurrentDirectory() + "\\TL\\iptv_services_insert.xls";
        public static string TlIptvPacketsInsertPath = Directory.GetCurrentDirectory() + "\\TL\\iptv_packets_insert.xls";
        public static string TlIptvPacketsUpdatePath = Directory.GetCurrentDirectory() + "\\TL\\iptv_packets_update.xls";
        public static string TlBundlesInsertPath = Directory.GetCurrentDirectory() + "\\TL\\bundles_insert.xls";
        public static string TlBundlesUpdatePath = Directory.GetCurrentDirectory() + "\\TL\\bundles_update.xls";
        public static string TlAnnualInsertPath = Directory.GetCurrentDirectory() + "\\TL\\annual_insert.xls";
        public static string TlAnnualUpdatePath = Directory.GetCurrentDirectory() + "\\TL\\annual_update.xls";
        public static string TlNetphoneInsertPath = Directory.GetCurrentDirectory() + "\\TL\\netphone_insert.xls";
        public static string TlNetphoneUpdatePath = Directory.GetCurrentDirectory() + "\\TL\\netphone_update.xls";
        
        public static string TlOutputPath = Directory.GetCurrentDirectory() + "\\TL" + "\\TL_LOAD\\";

        #endregion 

        public static string TempCityId = "99999";
        public static string TempCityName = "Город для удаленных сервисов (автотесты)";


        public static string NewAbonPassword = "password_AT";

        #region USSS

        internal static class USSS
        {

            public static string Wsdl_InfoService = "http://uss.caprica2.ftst3.corbina.net/info_service/?wsdl";
            public static string Wsdl_BundleServices = "http://uss.caprica2.ftst3.corbina.net/bundle_service/?wsdl";

            public static string RequestBegin =
                @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns0='http://inac.corbina.net/ns0'>
                                               <soapenv:Header/>
                                               <soapenv:Body>";

            public static string RequestEnd = @"</soapenv:Body>
                                            </soapenv:Envelope>";
        }

        #endregion

        #region AAA

        public static string BrasCisco = "78.107.1.242";
        public static string BrasEricson = "78.107.1.245";
        public static string Path = @"Z:\Shared\For_AT";
        public static string ConnectionCisco = "CISCO_AAA";
        public static string ConnectionEricson = "ERICSON_AAA";

        public static string RadiusIp = "172.20.3.154";
        public static string RadiusUser = "testuser";
        public static string RadiusPassword = "123";
        public static int RadiusPort = 22;
        public static string RadiusOk = "auth:true;svc:true;login:true";

        #endregion
    }
}
