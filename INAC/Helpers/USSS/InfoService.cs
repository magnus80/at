using AT.WebServices.SOAP;

namespace INAC.Helpers.USSS
{
    public static class InfoService
    {
        public static bool UserStopLogin(string login, string days = "1", string date = "")
        {
            var root_id = SoapParamList.AddParam("userStopLogin");

            SoapParamList.AddParam("login", root_id, login);
            SoapParamList.AddParam("days", root_id, days);

            if (date != "") SoapParamList.AddParam("scheduled_date", root_id, date);
            
            return SoapExecutor.Execute(Environment.USSS.Wsdl_InfoService, Environment.USSS.RequestBegin,
                                           Environment.USSS.RequestEnd);
        }
        
        public static bool GetNextBCSum(string login)
        {
            var root_id = SoapParamList.AddParam("getNextBCSum");

            SoapParamList.AddParam("login", root_id, login);

            return SoapExecutor.Execute(Environment.USSS.Wsdl_InfoService, Environment.USSS.RequestBegin,
                                           Environment.USSS.RequestEnd);
        }
    }
}
