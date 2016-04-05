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
    class CreateBillCallsRequestTest : TestBase
    {
        static string testName = "[SOAP API] Создание запроса для создания отчета по детализации счета для списка CTN";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ctn = ReaderTestData.ReadExel(testName, "phoneNumber");
        long ban = Convert.ToInt64(ReaderTestData.ReadExel(testName, "ban"));
        long requestId = Convert.ToInt64(ReaderTestData.ReadExel(testName, "requestId"));
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
            SubscriberService.createBillCallsRequestRequest getRequest = new createBillCallsRequestRequest();
            getRequest.CTNList = new string[]{ctn};
            getRequest.ban = ban;
            getRequest.billDate = startDate;
            getRequest.token = token;


            try
            {
                Logger.PrintAction("Создание запроса для создания отчета по детализации счета для списка CTN", "");
                SubscriberService.createBillCallsRequestResponse requestResponse = si.createBillCallsRequest(getRequest);
                var s = requestResponse.requestId;
                Logger.PrintAction("Заявка создана", s.ToString());
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при создании запроса для создания отчета по детализации счета для списка CTN " + ex.Message, Assert.Fail);
            }
        }
    }
}
