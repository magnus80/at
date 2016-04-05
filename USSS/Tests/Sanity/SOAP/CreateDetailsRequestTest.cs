using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.SubscriberService;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    class CreateDetailsRequestTest : TestBase
    {
        static string testName = "[SOAP] Формирование детализации формата  PDF по запросу из  GlassFish";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ctn = ReaderTestData.ReadExel(testName, "phoneNumber");
        long ban = Convert.ToInt64(ReaderTestData.ReadExel(testName, "ban"));
        TokenHashSoap ths = new TokenHashSoap();
        string token = String.Empty;
        DateTime startDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "StartDate"));
        DateTime endDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "EndDate"));

        [Test]
        public void step_01()
        {

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "");


            try
            {

                token = ths.GetToken(login, password);
                Logger.PrintAction("Токен получен", token);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_02()
        {

            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.createDetailsRequestRequest getRequest = new createDetailsRequestRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.channel = "?";
            getRequest.email = "kpodberezin@bellintegrator.ru";
            getRequest.format = "PDF";
            getRequest.periodEnd = endDate;
            getRequest.periodStart =startDate;
            
            try
            {
                Logger.PrintAction("Формирование детализации формата  PDF по запросу из  GlassFish", "");
                SubscriberService.createDetailsRequestResponse requestResponse = si.createDetailsRequest(getRequest);
                var s = requestResponse.requestId;
                Logger.PrintAction("Заявка создана", s.ToString());
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при формировании детализации формата  PDF по запросу из  GlassFish" + ex.Message, Assert.Fail);
            }
        }
    }
}
