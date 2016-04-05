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
    class LockUnlockTest: TestBase
    {

        static string testName = "[SOAP API] Блокировка и Разблокировка номера";
        string login;// = ReaderTestData.ReadExel(testName, "Login");
        string password;// = ReaderTestData.ReadExel(testName, "Password");
        private string ctn;// = ReaderTestData.ReadExel(testName, "phoneNumber");
        private DateTime lockdate = DateTime.Now.AddDays(1);
        string lockdatenow = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString() + "T00:00:00";
        string lockdatenowH = DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Day.ToString() + "000000";
        string lockdatefut = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + (DateTime.Now.Date.Day+1).ToString() + "T00:00:00";
        string lockdatefutH = DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Month.ToString() + (DateTime.Now.Date.Day + 1).ToString() + "000000";

        TokenHashSoap ths = new TokenHashSoap();
        string token = String.Empty;

        
        public void step_01(string exlogin, string expas, string exctn, string exban, string at)
        {
            login = ReaderTestData.ReadExel(testName, exlogin);
            password = ReaderTestData.ReadExel(testName, expas);
            ctn = ReaderTestData.ReadExel(testName, exctn);

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "Тип аккаунта" + at);

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

        public void step_02()
        {

            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка текущей датой", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,"Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);

        }
        public void step_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка текущей датой на заблокированном", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(false, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(true, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(true, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }
        public void step_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.restoreCTNRequest restoreRequest = new restoreCTNRequest();
            restoreRequest.ctn = ctn;
            restoreRequest.reasonCode = "RSBO";
            restoreRequest.token = token;
            restoreRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("Разблокировка текущей датой", "");
                SubscriberService.restoreCTNResponse requestResponse = si.restoreCTN(restoreRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }

        public void step_05()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatefut;
            try
            {
                Logger.PrintAction("блокировка будущей датой", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);

        }

        public void step_06()
        {

            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.restoreCTNRequest restoreRequest = new restoreCTNRequest();
            restoreRequest.ctn = ctn;
            restoreRequest.reasonCode = "RSBO";
            restoreRequest.token = token;
            restoreRequest.actvDate = lockdatefut;
            try
            {
                Logger.PrintAction("Разблокировка будущей датой", "");
                SubscriberService.restoreCTNResponse requestResponse = si.restoreCTN(restoreRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
                Thread.Sleep(12000);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }
        public void step_07()
        {

            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.ctn = ReaderTestData.ReadExel(testName, "ctn");
            suspendRequest.reasonCode = "AAA";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка с некорректным типом блокировки", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(false, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(true, "Заявка не создана");
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true,faultException.Detail+" "+faultException.Code);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
        }
        public void step_08()
        {

            Logger.PrintStepName("Step 8");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.ctn = ReaderTestData.ReadExel(testName,"ctn");
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка с абонентом не из иерархии", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(false, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(true, "Заявка не создана");
                }
            }
            catch(FaultException<UssWsApiException> faultException)
            {
                if (faultException.Detail.errorCode == 20006 && faultException.Detail.errorDescription == "FORBIDDEN (login=" + login + ")")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный " + faultException.Detail.errorDescription + " " + faultException.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + faultException.Detail.errorDescription + " " + faultException.Detail.errorCode);
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
        }

        public void step_01_s(string exlogin, string expas, string exctn, string exban, string at)
        {
            login = ReaderTestData.ReadExel(testName, exlogin);
            password = ReaderTestData.ReadExel(testName, expas);
            ctn = ReaderTestData.ReadExel(testName, exctn);

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "Тип аккаунта" + at);

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

        public void step_02_s()
        {

            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.hash = ths.GetHashAPI(ctn + lockdatenowH + "WIB");
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка текущей датой", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);

        }
        public void step_03_s()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.hash = ths.GetHashAPI(ctn + lockdatenowH + "WIB");
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка текущей датой на заблокированном", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(false, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(true, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(true, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }
        public void step_04_s()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.restoreCTNRequest restoreRequest = new restoreCTNRequest();
            restoreRequest.hash = ths.GetHashAPI(ctn + lockdatenowH + "RSBO");
            restoreRequest.ctn = ctn;
            restoreRequest.reasonCode = "RSBO";
            restoreRequest.token = token;
            restoreRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("Разблокировка текущей датой", "");
                SubscriberService.restoreCTNResponse requestResponse = si.restoreCTN(restoreRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }

        public void step_05_s()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.hash = ths.GetHashAPI(ctn + lockdatefutH + "WIB");
            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatefut;
            try
            {
                Logger.PrintAction("блокировка будущей датой", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);

        }

        public void step_06_s()
        {

            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.restoreCTNRequest restoreRequest = new restoreCTNRequest();
            restoreRequest.hash = ths.GetHashAPI(ctn + lockdatefutH + "RSBO");
            restoreRequest.ctn = ctn;
            restoreRequest.reasonCode = "RSBO";
            restoreRequest.token = token;
            restoreRequest.actvDate = lockdatefut;
            try
            {
                Logger.PrintAction("Разблокировка будущей датой", "");
                SubscriberService.restoreCTNResponse requestResponse = si.restoreCTN(restoreRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(true, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(false, "Заявка не создана");
                }
                Thread.Sleep(12000);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
            Thread.Sleep(12000);
        }
        public void step_07_s()
        {

            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            suspendRequest.hash = ths.GetHashAPI(ctn + lockdatefutH + "AAA");
            suspendRequest.ctn = ReaderTestData.ReadExel(testName, "ctn");
            suspendRequest.reasonCode = "AAA";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("блокировка с некорректным типом блокировки", "");
                SubscriberService.suspendCTNResponse requestResponse = si.suspendCTN(suspendRequest);
                if (requestResponse != null)
                {
                    Logger.PrintRezult(false, "Заявка создана " + requestResponse.@return);
                }
                else
                {
                    Logger.PrintRezult(true, "Заявка не создана");
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, faultException.Detail + " " + faultException.Code);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при блокировке: " + ex.Message);
            }
        }
        
        [Test]
        public void step_13at()
        {
            step_01("Login13", "Password13", "ctn13", "BAN13", " 13");
            step_02();
            step_03();
            step_04();
            step_02();
            step_06();
            step_04();
            step_07();
            step_08();
            step_05();

            step_01_s("Login13", "Password13", "ctn13", "BAN13", " 13");
            step_02_s();
            step_03_s();
            step_04_s();
            step_02_s();
            step_06_s();
            step_04_s();
            step_07_s();
            step_05_s();
        }
        [Test]
        public void step_37at()
        {
            step_01("Login37", "Password37", "ctn37", "BAN37", " 37");
            step_02();
            step_03();
            step_04();
            step_02();
            step_06();
            step_04();
            step_07();
            step_08();
            step_05();

            step_01_s("Login37", "Password37", "ctn37", "BAN37", " 37");
            step_02_s();
            step_03_s();
            step_04_s();
            step_02_s();
            step_06_s();
            step_04_s();
            step_07_s();
            step_05_s();
        }
        [Test]
        public void step_101at()
        {
            step_01("Login101", "Password101", "ctn101", "BAN101", " 101");
            step_02();
            step_03();
            step_04();
            step_02();
            step_06();
            step_04();
            step_07();
            step_08();
            step_05();

            step_01_s("Login101", "Password101", "ctn101", "BAN101", " 101");
            step_02_s();
            step_03_s();
            step_04_s();
            step_02_s();
            step_06_s();
            step_04_s();
            step_07_s();
            step_05_s();
        }
        [Test]
        public void step_105at()
        {
            step_01("Login105", "Password105", "ctn105", "BAN105", " 105");
            step_02();
            step_03();
            step_04();
            step_02();
            step_06();
            step_04();
            step_07();
            step_08();
            step_05();

            step_01_s("Login105", "Password105", "ctn105", "BAN105", " 105");
            step_02_s();
            step_03_s();
            step_04_s();
            step_02_s();
            step_06_s();
            step_04_s();
            step_07_s();
            step_05_s();
        }
    }
}
