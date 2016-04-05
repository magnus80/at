using System.ServiceModel;
using System.Threading;
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
    class AddDelSocTest: TestBase
    {
        static string testName = "[SOAP API] Подключение (отключение) услуг [addDelSOC]";

        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        private string ctn = ReaderTestData.ReadExel(testName, "ctn");
        string soc = ReaderTestData.ReadExel(testName, "soc");

        string login_blocked = ReaderTestData.ReadExel(testName, "LoginB");
        string password_blocked = ReaderTestData.ReadExel(testName, "PasswordB");
        private string ctn_blocked = ReaderTestData.ReadExel(testName, "ctnB");

        string login_futpp = ReaderTestData.ReadExel(testName, "LoginF");
        string password_futpp = ReaderTestData.ReadExel(testName, "PasswordF");
        private string ctn_futpp = ReaderTestData.ReadExel(testName, "ctnF");

        string login_low = ReaderTestData.ReadExel(testName, "LoginL");
        string password_low = ReaderTestData.ReadExel(testName, "PasswordL");
        private string ctn_low = ReaderTestData.ReadExel(testName, "ctnL");

        private string soc1 = ReaderTestData.ReadExel(testName, "soc1");
        private string soc2 = ReaderTestData.ReadExel(testName, "soc2");

        TokenHashSoap ths = new TokenHashSoap();
        string token = String.Empty;
        private string token_blocked;
        private string token_low;
        private string token_futpp;

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

                token_blocked = ths.GetToken(login_blocked, password_blocked);
                token_low = ths.GetToken(login_low, password_low);
                token_futpp = ths.GetToken(login_futpp, password_futpp);
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


            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();
            SubscriberService.getRequestListRequest getRequestList = new getRequestListRequest();


            getRequestList.token = token;
            getRequestList.login = login;
            getRequestList.startDate = DateTime.Now;
            getRequestList.page = 1;
            getRequestList.recordsPerPage = 5;
            string requestId;

            getRequest.inclusionType = "A";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Подключение услуги", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", requestId = s.ToString());
                getRequestList.requestId = Convert.ToInt64(requestId);
                Thread.Sleep(15000);
                SubscriberService.getRequestListResponse requestListResponse = si.getRequestList(getRequestList);
                Logger.PrintAction("Заявка "+requestId," "+requestListResponse.requestList.requests[0].requestStatus);
            }
                catch(FaultException<UssWsApiException> exception)
                {
                    Logger.PrintRezult(false,exception.Detail.errorCode+" "+exception.Detail.errorDescription);
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
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            SubscriberService.getRequestListRequest getRequestList = new getRequestListRequest();


            getRequestList.token = token;
            getRequestList.login = login;
            getRequestList.startDate = DateTime.Now;
            getRequestList.page = 1;
            getRequestList.recordsPerPage = 5;
            string requestId;

            getRequest.inclusionType = "D";
            getRequest.soc = soc;
            getRequest.ctn = ctn;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token;

            try
            {
                Logger.PrintAction("Отключение услуги", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на отключение услуги создана", requestId = s.ToString());
                getRequestList.requestId = Convert.ToInt64(requestId);
                Thread.Sleep(15000);
                SubscriberService.getRequestListResponse requestListResponse = si.getRequestList(getRequestList);
                Logger.PrintAction("Заявка " + requestId, " " + requestListResponse.requestList.requests[0].requestStatus);

            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при отключении услуги: " + ex.Message, Assert.Fail);
            }
        }
        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            SubscriberService.getRequestListRequest getRequestList = new getRequestListRequest();


            getRequestList.token = token;
            getRequestList.login = login;
            getRequestList.startDate = DateTime.Now;
            getRequestList.page = 1;
            getRequestList.recordsPerPage = 5;
            string requestId;

            getRequest.inclusionType = "A";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now.AddDays(1);
            getRequest.token = token;
            getRequest.ctn = ctn;
            try
            {
                Thread.Sleep(15000);
                Logger.PrintAction("Подключение услуги", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", requestId = s.ToString());
                getRequestList.requestId = Convert.ToInt64(requestId);
                Thread.Sleep(15000);
                SubscriberService.getRequestListResponse requestListResponse = si.getRequestList(getRequestList);
                Logger.PrintAction("Заявка " + requestId, " " + requestListResponse.requestList.requests[0].requestStatus);
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
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
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            SubscriberService.getRequestListRequest getRequestList = new getRequestListRequest();


            getRequestList.token = token;
            getRequestList.login = login;
            getRequestList.startDate = DateTime.Now;
            getRequestList.page = 1;
            getRequestList.recordsPerPage = 5;
            string requestId;

            getRequest.inclusionType = "D";
            getRequest.soc = soc;
            getRequest.ctn = ctn;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now.AddDays(1);
            getRequest.token = token;

            try
            {
                Thread.Sleep(15000);
                Logger.PrintAction("Отключение услуги", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на отключение услуги создана", requestId = s.ToString());
                getRequestList.requestId = Convert.ToInt64(requestId);
                Thread.Sleep(15000);
                SubscriberService.getRequestListResponse requestListResponse = si.getRequestList(getRequestList);
                Logger.PrintAction("Заявка " + requestId, " " + requestListResponse.requestList.requests[0].requestStatus);
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при отключении услуги: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "A";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token_blocked;
            getRequest.ctn = ctn_blocked;
            try
            {
                Logger.PrintAction("Подключение услуги заблокированным пользователем", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "A";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token_low;
            getRequest.ctn = ctn_low;
            try
            {
                Logger.PrintAction("Подключение услуги с недост балансом", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "A";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token_futpp;
            getRequest.ctn = ctn_futpp;
            try
            {
                Logger.PrintAction("Подключение услуги с буд сменой тп", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "D";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token_futpp;
            getRequest.ctn = ctn_futpp;
            try
            {
                Logger.PrintAction("Отключение услуги с буд сменой тп", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "D";
            getRequest.soc = soc;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token_blocked;
            getRequest.ctn = ctn_blocked;
            try
            {
                Logger.PrintAction("Отключение услуги заблокированным пользователем", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "A";
            getRequest.soc = soc1;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Подключение услуги которая уже подключена", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }

        [Test]
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.addDelSOCRequest getRequest = new addDelSOCRequest();

            getRequest.inclusionType = "D";
            getRequest.soc = soc2;
            getRequest.effDate = DateTime.Now;
            getRequest.expDate = DateTime.Now;
            getRequest.token = token;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Отключение услуги заблокированным пользователем", soc);
                SubscriberService.addDelSOCResponse requestResponse = si.addDelSOC(getRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на подключение создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + " " + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при подключении услуги: " + ex.Message, Assert.Fail);
            }

        }
    }
}
