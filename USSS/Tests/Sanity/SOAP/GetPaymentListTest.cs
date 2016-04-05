using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.SubscriberService;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    internal class GetPaymentListTest : TestBase
    {
        private static string testName = "[SOAP API] Получение списка платежей";
        private string login = ReaderTestData.ReadExel(testName, "Login");
        private string password = ReaderTestData.ReadExel(testName, "Password");
        private string ctn = ReaderTestData.ReadExel(testName, "ctn");
        private long ban = Convert.ToInt64(ReaderTestData.ReadExel(testName, "BAN"));
        private DateTime startDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "StartDate"));
        private DateTime endDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "EndDate"));

        private string loginPrepaid = ReaderTestData.ReadExel(testName, "Login Prepaid");
        //private string passwordPrepaid = ReaderTestData.ReadExel(testName, "Password");
        private string ctnPrepaid = ReaderTestData.ReadExel(testName, "ctn Prepaid");
        private long banPrepaid = Convert.ToInt64(ReaderTestData.ReadExel(testName, "BAN Prepaid"));
        private DateTime startDatePrepaid = DateTime.Parse(ReaderTestData.ReadExel(testName, "StartDate Prepaid"));
        private DateTime endDatePrepaid = DateTime.Parse(ReaderTestData.ReadExel(testName, "EndDate Prepaid"));

        private TokenHashSoap ths = new TokenHashSoap();
        private string token = String.Empty;

        private bool globalR = true;

        [Test]
        //аутентификация
        public void step_01()
        {
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "");


            try
            {
                token = ths.GetToken(login, password);
                Logger.PrintAction("Токен получен", token);
                Logger.PrintRezult(true,"");
            }
            catch (Exception ex)
            {
                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "");
            }
        }

        [Test]
        //корректные параметры
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            getRequest.ctn = ctn;
            getRequest.token = ths.GetToken(login, password);
            getRequest.ban = ban;
            getRequest.startDate = startDate;
            getRequest.endDate = endDate;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b post, получение платежей, все корректные параметры ", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
          //      Logger.PrintRezult(true, "");
                globalR = true;
                if (s[0] != null)
                {
                    Logger.PrintRezult(true, "Метод отрабатывает");
                    globalR = true;
                }
                else
                {
                    Logger.PrintRezult(true, "Метод отрабатывает, платежи не найдены");
                    globalR = true;
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        //все параметры, ctn не привязан к передаваемому бану
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            getRequest.ctn = "9030337081";
            getRequest.token = ths.GetToken(login, password);
            getRequest.ban = ban;
            getRequest.startDate = startDate;
            getRequest.endDate = endDate;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b post, получение платежей, ctn не привязан к передаваемому бану", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
                Logger.PrintRezult(true, "");
                globalR = true;
            }

            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true,
                    "Ошибка при получении списка платежей: " + faultException.Detail.errorCode + " " +
                    faultException.Detail.errorDescription);

                if (faultException.Detail.errorCode == 20006 &&
                    faultException.Detail.errorDescription.Substring(0, 9) == "FORBIDDEN")
                {
                    Logger.PrintRezult(true, "Код ошибки корректен");
                }
                else
                {
                    Logger.PrintRezult(false, "Код ошибки некорректен");
                    globalR = false;
                }
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        //все параметры, ctn не передается
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            // getRequest.ctn = "9030337081";
            getRequest.token = ths.GetToken(login, password);
            getRequest.ban = ban;
            getRequest.startDate = startDate;
            getRequest.endDate = endDate;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b post, получение платежей, ctn не передается", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
                Logger.PrintRezult(true, "");
                globalR = true;
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        //аутентификация
        public void step_05()
        {           
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("подключению к сервису", "");

            try
            {
                token = ths.GetToken(login, password);
                Logger.PrintAction("Токен получен", token);
                Logger.PrintRezult(true, "");
            }
            catch (Exception ex)
            {
                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "");
            }
        }

        [Test]
        //корректные параметры b2b prepaid
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            getRequest.ctn = ctnPrepaid;
            getRequest.token = ths.GetToken(loginPrepaid, password);
            getRequest.ban = banPrepaid;
            getRequest.startDate = startDatePrepaid;
            getRequest.endDate = endDatePrepaid;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b prepaid, получение платежей, все корректные параметры ", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
               // Logger.PrintRezult(true, "");
                globalR = true;
                if (s[0] != null)
                {
                    Logger.PrintRezult(true, "Метод отрабатывает");
                    globalR = true;
                }
                else
                {
                    Logger.PrintRezult(true, "Метод отрабатывает, платежи не найдены");
                    globalR = true;
                }


            }

            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей: " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей: " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        //все параметры, ctn не привязан к передаваемому бану
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            getRequest.ctn = "9030337081";
            getRequest.token = ths.GetToken(loginPrepaid, password);
            getRequest.ban = banPrepaid;
            getRequest.startDate = startDatePrepaid;
            getRequest.endDate = endDatePrepaid;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b prepaid, получение платежей, ctn не привязан к передаваемому бану", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
                Logger.PrintRezult(false, "");
                globalR = true;
            }

            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true,
                    "Ошибка при получении списка платежей: " + faultException.Detail.errorCode + " " +
                    faultException.Detail.errorDescription);

                if (faultException.Detail.errorCode == 20006 &&
                    faultException.Detail.errorDescription.Substring(0, 9) == "FORBIDDEN")
                {
                    Logger.PrintRezult(true, "Код ошибки корректен");
                }
                else
                {
                    Logger.PrintRezult(false, "Код ошибки некорректен");
                    globalR = false;
                }
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        //все параметры, ctn не передается
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getPaymentListRequest getRequest = new getPaymentListRequest();
            // getRequest.ctn = "9030337081";
            getRequest.token = ths.GetToken(loginPrepaid, password);
            getRequest.ban = banPrepaid;
            getRequest.startDate = startDatePrepaid;
            getRequest.endDate = endDatePrepaid;
            getRequest.hash = "?";

            try
            {
                Logger.PrintAction("b2b prepaid, получение платежей, ctn не передается", "");
                SubscriberService.getPaymentListResponse requestResponse = si.getPaymentList(getRequest);
                var s = requestResponse.PaymentList;

                Logger.PrintAction("Список платежей получен", "");
                Logger.PrintRezult(true, "");
                globalR = true;
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при получении списка платежей: " + ex.Message, Assert.Fail);
                Logger.PrintRezult(false, "Ошибка при получении списка платежей: " + ex.Message);
                globalR = false;
            }
        }

       [Test]
        public void step_09()
        {
            Logger.PrintAction("Завершение тестирования", "");
            Logger.PrintRezultTest(globalR);
        }

    }
}