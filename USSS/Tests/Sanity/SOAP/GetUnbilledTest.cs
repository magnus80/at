using System.ServiceModel;
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
    class GetUnbilledTest : TestBase
    {
        static string testName = "[SOAP API] Получение unbilled (postpaid)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ctn = ReaderTestData.ReadExel(testName, "ctn");
        private string ctn_x = ReaderTestData.ReadExel(testName, "ctn_x");

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
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;

            try
            {
                Logger.PrintAction("Получение unbilled", "");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(true,"Список получен "+s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false,"Список не получен");
                }
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
            
            getRequest.token = token;

            try
            {
                Logger.PrintAction("Получение unbilled", "без ctn ");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }
           
                 catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20001 && exception.Detail.errorDescription == "INVALID_QUERY_PARAM")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }          
            
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
            getRequest.ctn = ctn;
            getRequest.token = null;

            try
            {
                Logger.PrintAction("Получение unbilled", "без токена");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }

            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20001 && exception.Detail.errorDescription == "INVALID_QUERY_PARAM")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();

            getRequest.token = token;
            getRequest.ctn = ctn + 123;
            try
            {
                Logger.PrintAction("Получение unbilled", "несуществующий ctn ");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }

            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20005 && exception.Detail.errorDescription == "CTN_NOT_FOUND")
                {
                    Logger.PrintRezult(true,
                                       "Код ошибки корректный " + exception.Detail.errorDescription + " " +
                                       exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false,
                                       "Некорректный код ошибки " + exception.Detail.errorDescription + " " +
                                       exception.Detail.errorCode);
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

            [Test]
            public void step_05()
            {
                Logger.PrintStepName("Step 5");
                SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
                SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
                getRequest.ctn = ctn;
                getRequest.token = token + "AA";

                try
                {
                    Logger.PrintAction("Получение unbilled", "несуществующий токен");
                    SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                    var s = requestResponse.UnbilledCallsList;
                    if (s != null)
                    {
                        Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Список не получен");
                    }
                }

                catch (FaultException<UssWsApiException> exception)
                {
                    if (exception.Detail.errorCode == 20003 && exception.Detail.errorDescription == "TOKEN_NOT_FOUND")
                    {
                        Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                }

                catch (Exception ex)
                {
                    Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
                }
            }

        [Test]
        public void step_s_01()
        {

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "");


            try
            {

                token = ths.GetSystemToken();
                Logger.PrintAction("Токен получен", token);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_s_02()
        {

            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.hash = ths.GetHashAPI(ctn);
            try
            {
                Logger.PrintAction("Получение unbilled", "");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(true,"Список получен "+s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false,"Список не получен");
                }
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_s_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
            
            getRequest.token = token;
            getRequest.hash = ths.GetHashAPI("");
            try
            {
                Logger.PrintAction("Получение unbilled", "без ctn ");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }
           
                 catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20001 && exception.Detail.errorDescription == "INVALID_QUERY_PARAM")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }          
            
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
            getRequest.ctn = ctn;
            getRequest.token = null;
            getRequest.hash = ths.GetHashAPI(ctn);
            try
            {
                Logger.PrintAction("Получение unbilled", "без токена");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }

            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20001 && exception.Detail.errorDescription == "INVALID_QUERY_PARAM")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_s_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();

            getRequest.token = token;
            getRequest.ctn = ctn + 123;
            getRequest.hash = ths.GetHashAPI(ctn + "123");
            try
            {
                Logger.PrintAction("Получение unbilled", "несуществующий ctn ");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }

            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20005 && exception.Detail.errorDescription == "CTN_NOT_FOUND")
                {
                    Logger.PrintRezult(true,
                                       "Код ошибки корректный " + exception.Detail.errorDescription + " " +
                                       exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false,
                                       "Некорректный код ошибки " + exception.Detail.errorDescription + " " +
                                       exception.Detail.errorCode);
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

            getRequest.token = token;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ctn)+"123";
            try
            {
                Logger.PrintAction("Получение unbilled", "несуществующий hash ");
                SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                var s = requestResponse.UnbilledCallsList;
                if (s != null)
                {
                    Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                }
                else
                {
                    Logger.PrintRezult(false, "Список не получен");
                }
            }

            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20007 && exception.Detail.errorDescription == "INVALID_SYSTEM_HASH")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }

            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }
        }

            [Test]
            public void step_s_05()
            {
                Logger.PrintStepName("Step 5");
                SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
                SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
                getRequest.ctn = ctn;
                getRequest.token = token + "AA";
                getRequest.hash = ths.GetHashAPI(ctn);

                try
                {
                    Logger.PrintAction("Получение unbilled", "несуществующий токен");
                    SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                    var s = requestResponse.UnbilledCallsList;
                    if (s != null)
                    {
                        Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Список не получен");
                    }
                }

                catch (FaultException<UssWsApiException> exception)
                {
                    if (exception.Detail.errorCode == 20003 && exception.Detail.errorDescription == "TOKEN_NOT_FOUND")
                    {
                        Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                }

                catch (Exception ex)
                {
                    Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
                }
            }

            [Test]
            public void step_06()
            {
                Logger.PrintStepName("Step 5");
                SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
                SubscriberService.getUnbilledCallsListRequest getRequest = new getUnbilledCallsListRequest();
                getRequest.ctn = ctn_x;
                getRequest.token = token;

                try
                {
                    Logger.PrintAction("Получение unbilled", "Другой номер");
                    SubscriberService.getUnbilledCallsListResponse requestResponse = si.getUnbilledCallsList(getRequest);
                    var s = requestResponse.UnbilledCallsList;
                    if (s != null)
                    {
                        Logger.PrintRezult(false, "Список получен " + s[0].callDate);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Список не получен");
                    }
                }

                catch (FaultException<UssWsApiException> exception)
                {
                    if (exception.Detail.errorCode == 20005 && exception.Detail.errorDescription == "CTN_NOT_FOUND")
                    {
                        Logger.PrintRezult(true, "Код ошибки корректный " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                    else
                    {
                        Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                    }
                }

                catch (Exception ex)
                {
                    Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
                }
            }

        }

    }

