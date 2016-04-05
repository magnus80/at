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
    class NeoBillBalanceCtnTest: TestBase
    {
        static string testName = "[SOAP API] Метод по необилленным балансам по CTN [getUnbilledBalances]";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ctn = ReaderTestData.ReadExel(testName, "phoneNumber");


        TokenHashSoap ths = new TokenHashSoap();
        string token = String.Empty;

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
            SubscriberService.getUnbilledBalancesRequest getRequest = new getUnbilledBalancesRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;

            try
            {
                Logger.PrintAction("Получение необиллиного баланса", "");
                SubscriberService.getUnbilledBalancesResponse requestResponse = si.getUnbilledBalances(getRequest);
                unbilledBalancesDO s = requestResponse.unbilledBalances;
                Logger.PrintAction("Необилленный баланс получен", "");
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получение необилленного баланса: " + ex.Message, Assert.Fail);
            }
        }
    }
}
