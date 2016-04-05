using AT;
using AT.WebServices.SOAP;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    public class test : TestBase
    {
        [Test]
        public void step_01()
        {
            var root_id = SoapParamList.AddParam("addBundle");
            var bs_id = SoapParamList.AddParam("bundle_structure", root_id);
            var abst_1 = SoapParamList.AddParam("AddBundleServiceType", bs_id);
            var abst_2 = SoapParamList.AddParam("AddBundleServiceType", bs_id);

            SoapParamList.AddParam("login", root_id, "0894966719");
            SoapParamList.AddParam("bundle_id", root_id, "BS01633");
            SoapParamList.AddParam("antivirus_email", abst_1, "test@test.ru");
            SoapParamList.AddParam("service_type", abst_1, "ANTIVIRUS");
            SoapParamList.AddParam("service_id", abst_1, "ESET0");
            SoapParamList.AddParam("service_type", abst_2, "FTTB");
            SoapParamList.AddParam("service_id", abst_2, "M106309");
            
            SoapExecutor.Execute(Environment.USSS.Wsdl_BundleServices, Environment.USSS.RequestBegin,
                                           Environment.USSS.RequestEnd);

            var res = SoapExecutor.Results;

        }
    }
}