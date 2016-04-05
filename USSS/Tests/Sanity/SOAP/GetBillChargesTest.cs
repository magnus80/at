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
    class GetBillChargesTest : TestBase
    {
        static string testName = "[SOAP API] Просмотр рез-та запроса отчета по начисл-ям счета для списка CTN";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ctn = ReaderTestData.ReadExel(testName, "phoneNumber");
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
            SubscriberService.getBillChargesRequest getRequest = new getBillChargesRequest();
            getRequest.requestId = requestId;
            getRequest.token = token;
            

            try
            {
                Logger.PrintAction("Получение рез-та запроса отчета по начисл-ям счета", "");
                SubscriberService.getBillChargesResponse requestResponse = si.getBillCharges(getRequest);
                var s = requestResponse.BillChargesList;
                Logger.PrintAction("Результат запроса отчета по начисл-ям счета получин", "");
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получении результата запроса отчета по начисл-ям счета " + ex.Message, Assert.Fail);
            }
        }
    }
}
