using System.ServiceModel;
using System.Threading;
using AT;
using AT.DataBase;
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
    class ChangeSimTest: TestBase
    {

        static string testName = "[SOAP API] Замена SIM";
        string login = ReaderTestData.ReadExel(testName, "Login101");
        string password = ReaderTestData.ReadExel(testName, "Password101");
        string ctn = ReaderTestData.ReadExel(testName, "ctn101");

        string login_n   = ReaderTestData.ReadExel(testName, "LoginN");
        string password_n = ReaderTestData.ReadExel(testName, "PasswordN");
        string ctn_n = ReaderTestData.ReadExel(testName, "ctnN");

        private string ctn_x = ReaderTestData.ReadExel(testName, "ctnx");
        private string newSim;
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Usss = ReaderTestData.ReadCExel(4, 8);

        private string lockdatenow;
        DateTime lockdate;
        TokenHashSoap ths = new TokenHashSoap();
        string token = String.Empty;
        private string tokenN = string.Empty;

        [Test]
        public void step_01()
        {

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "");
            lockdatenow = Executor.ExecuteSelect("select logical_date from logical_date@" + db_Ans)[0, 0]/*DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString() + "T00:00:00"*/;
            
            lockdate = Convert.ToDateTime(lockdatenow);
            lockdatenow = lockdate.Year.ToString() + "-" + lockdate.Month.ToString() + "-" +
                          lockdate.Date.Day.ToString() + "T00:00:00";
            try
            {
                tokenN = ths.GetToken(login_n,password_n);
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
            SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
            replaceRequest.ctn = ctn;
            replaceRequest.token = token;
            newSim =
                Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                       " where primary_ctn is null and ngp=1 and resource_status='AA'")[0, 0];
            replaceRequest.serialNumber = newSim;

            try
            {
                Logger.PrintAction("Замена сим 105 AT", "");
                SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на замену сим создана", s.ToString());
            }
            catch(FaultException<UssWsApiException> exception)
            {
                    Logger.PrintRezult(false,exception.Detail.errorCode+exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();


            replaceRequest.ctn = ctn;
            replaceRequest.token = token;
            newSim =
                Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                       " where primary_ctn is null and ngp=1 and resource_status='AA'")[1, 0];
            replaceRequest.serialNumber = newSim;

            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "STO";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;

            try
            {
                Logger.PrintAction("Замена сим 105 AT", "");
                SubscriberService.suspendCTNResponse suspendCtnResponse = si.suspendCTN(suspendRequest);
                Thread.Sleep(15000);
                Logger.PrintAction("Заблокирован STO ",suspendCtnResponse.@return.ToString());
                SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на замену сим создана", s.ToString());
                Logger.PrintRezult(true,"");
                Thread.Sleep(15000);
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
            SubscriberService.suspendCTNRequest suspendRequest = new suspendCTNRequest();
            SubscriberService.restoreCTNRequest restoreRequest = new restoreCTNRequest();


            replaceRequest.ctn = ctn;
            replaceRequest.token = token;
            newSim =
                Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                       " where primary_ctn is null and ngp=1 and resource_status='AA'")[2, 0];
            replaceRequest.serialNumber = newSim;

            suspendRequest.ctn = ctn;
            suspendRequest.reasonCode = "WIB";
            suspendRequest.token = token;
            suspendRequest.actvDate = lockdatenow;

            restoreRequest.ctn = ctn;
            restoreRequest.reasonCode = "RSBO";
            restoreRequest.token = token;
            restoreRequest.actvDate = lockdatenow;
            try
            {
                Logger.PrintAction("Замена сим 105 AT", "");
                SubscriberService.suspendCTNResponse suspendCtnResponse = si.suspendCTN(suspendRequest);
                Thread.Sleep(15000);
                Logger.PrintAction("Заблокирован WIB ", suspendCtnResponse.@return.ToString());
                SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на замену сим создана", s.ToString());
                Logger.PrintRezult(true, "");

                SubscriberService.restoreCTNResponse restoreResponse = si.restoreCTN(restoreRequest);
                Logger.PrintAction("Разблокировка",restoreResponse.@return.ToString());
                Thread.Sleep(15000);
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_05()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
            replaceRequest.ctn = ctn_n;
            replaceRequest.token = tokenN;
            newSim =
                Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                       " where primary_ctn is null and ngp=1 and resource_status='AA'")[0, 0];
            replaceRequest.serialNumber = newSim;

            try
            {
                Logger.PrintAction("Замена сим 105 AT", "");
                SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на замену сим создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorCode + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_06()
        {

            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
            replaceRequest.ctn = ctn_n;
            replaceRequest.token = token;
            newSim =
                Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                       " where primary_ctn is null and ngp=1 and resource_status='AA'")[0, 0];
            replaceRequest.serialNumber = newSim;

            try
            {
                Logger.PrintAction("Замена сим 105 AT", "");
                SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                long s = requestResponse.@return;
                Logger.PrintAction("Заявка на замену сим создана", s.ToString());
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(true, exception.Detail.errorCode + exception.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
            }
        }
         [Test]
        public void step_07()
         {

             Logger.PrintStepName("Step 7");
             SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
             SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
             replaceRequest.ctn = "9681002030";
             replaceRequest.token = token;
             newSim =
                 Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                        " where primary_ctn is null and ngp=1 and resource_status='AA'")[0, 0];
             replaceRequest.serialNumber = newSim;

             try
             {
                 Logger.PrintAction("Замена сим 105 AT", "");
                 SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                 long s = requestResponse.@return;
                 Logger.PrintAction("Заявка на замену сим создана", s.ToString());
             }
             catch (FaultException<UssWsApiException> exception)
             {
                 Logger.PrintRezult(true, exception.Detail.errorCode + exception.Detail.errorDescription);
             }
             catch (Exception ex)
             {
                 Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
             }
         }

         
        #region system

         public void step_01_s(string exlogin, string expas, string exctn, string at)
         {
             login = ReaderTestData.ReadExel(testName, exlogin);
             password = ReaderTestData.ReadExel(testName, expas);
             ctn = ReaderTestData.ReadExel(testName, exctn);
             //ban = ReaderTestData.ReadExel(testName, exban);

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

        #endregion

         public void step_08()
         {
             step_01_s("Login13", "Password13", "ctn13","13");
             Logger.PrintStepName("Step 8");
             SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
             SubscriberService.replaceSIMRequest replaceRequest = new replaceSIMRequest();
             replaceRequest.ctn = ctn;
             //replaceRequest.token = token;
             newSim =
                 Executor.ExecuteSelect("select serial_no from serial_item_inv@" + db_Ans +
                                        " where primary_ctn is null and ngp=1 and resource_status='AA'")[0, 0];
             replaceRequest.serialNumber = newSim;

             try
             {
                 Logger.PrintAction("Замена сим 105 AT", " system hash");
                 SubscriberService.replaceSIMResponse requestResponse = si.replaceSIM(replaceRequest);
                 long s = requestResponse.@return;
                 Logger.PrintAction("Заявка на замену сим создана", s.ToString());
             }
             catch (FaultException<UssWsApiException> exception)
             {
                 Logger.PrintRezult(true, exception.Detail.errorCode + exception.Detail.errorDescription);
             }
             catch (Exception ex)
             {
                 Assertion("Ошибка при замене сим: " + ex.Message, Assert.Fail);
             }

         }
    }
}
