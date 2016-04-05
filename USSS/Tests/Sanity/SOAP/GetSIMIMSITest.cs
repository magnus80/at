using System.ServiceModel;
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
    class GetSIMIMSITest: TestBase
    {
        static string testName = "[SOAP API] Получение номера SIMIMSI";
        string login;// = ReaderTestData.ReadExel(testName, "Login13");
        private string password;// = ReaderTestData.ReadExel(testName, "Password13");
        private string ctn;// = ReaderTestData.ReadExel(testName, "ctn13");
        private string ban;// = ReaderTestData.ReadExel(testName, "BAN13");

        GetUrl getUrl = new GetUrl();
        private string url ;

        string ctn_x = ReaderTestData.ReadExel(testName, "ctn_x");

        TokenHashSoap ths = new TokenHashSoap();
        private string token;
        private bool globalR = true;
        #region 13at
        #region user
        
        public void step_01(string exlogin, string expas, string exctn, string exban, string at)
        {
            url = getUrl.Url;
            
            

            login = ReaderTestData.ReadExel(testName, exlogin);
            password = ReaderTestData.ReadExel(testName, expas);
            ctn = ReaderTestData.ReadExel(testName, exctn);
            ban = ReaderTestData.ReadExel(testName, exban);

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
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ",s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban="+ban+" and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '"+ctn+"'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn + "'");
                if(result[0,0] == s[0].serialNumber && result1[0,0] == s[0].imsi)
                {
                    Logger.PrintRezult(true,"Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false,"Номер не совпадает со значением в бд");
                    globalR = false;
                }
            }
            catch(FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode+" "+faultException.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,"Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message,Assert.Fail);
            }
        }
        
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            try
            {
                Logger.PrintAction("Получение номера SIM", "Вызвать метод с параметрами ban");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn_x;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Повторить шаги 2, 3, передав ctn, привязанный к другому бану");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if(faultException.Detail.errorCode == 20005 && faultException.Detail.errorDescription == "CTN_NOT_FOUND")
                {
                    Logger.PrintRezult(true,"Код ошибки корректен");
                }
                else
                {
                    Logger.PrintRezult(false, "Код ошибки некорректен");
                    globalR = false;
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token;
            //Ban по умолчанию равен 0
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с пустыми параметрами обязательными(ban)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20001 && faultException.Detail.errorDescription == "INVALID_QUERY_PARAM")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
            getRequest.token = null;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с пустыми параметрами обязательными(token)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn_x + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn_x + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20001 && faultException.Detail.errorDescription == "INVALID_QUERY_PARAM")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }
        
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token+"ABC";
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(token)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20003 && faultException.Detail.errorDescription == "TOKEN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
            //
            getRequest.token = token;
            getRequest.ban = ban + 11;//getRequest.ban = Convert.ToInt64(ban+11);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(ban)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn_x + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn_x + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20005 && faultException.Detail.errorDescription == "CTN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }
        
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token + "ABC";
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(token)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20003 && faultException.Detail.errorDescription == "TOKEN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        #endregion
        #region system
        public void step_01_s(string exlogin, string expas, string exctn, string exban, string at)
        {
            login = ReaderTestData.ReadExel(testName, exlogin);
            password = ReaderTestData.ReadExel(testName, expas);
            ctn = ReaderTestData.ReadExel(testName, exctn);
            ban = ReaderTestData.ReadExel(testName, exban);

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
            Logger.PrintAction("SYSTEM TOKEN","");
            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }
        
        public void step_03_s()
        {
            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ban);
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            try
            {
                Logger.PrintAction("Получение номера SIM", "Вызвать метод с параметрами ban");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        
        public void step_04_s()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ban + ctn_x);
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn_x;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Повторить шаги 2, 3, передав ctn, привязанный к другому бану");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20005 && faultException.Detail.errorDescription == "CTN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        
        public void step_05_s()
        {
            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ctn);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с пустыми параметрами обязательными(ban)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20001 && faultException.Detail.errorDescription == "INVALID_QUERY_PARAM")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
            getRequest.token = null;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с пустыми параметрами обязательными(token)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn_x + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn_x + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20001 && faultException.Detail.errorDescription == "INVALID_QUERY_PARAM")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }
        
        public void step_06_s()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ban + ctn)+1;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(hash)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn_x + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn_x + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20007 && faultException.Detail.errorDescription == "INVALID_SYSTEM_HASH")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
            //
            getRequest.token = ths.GetSystemToken();
            getRequest.hash = ths.GetHashAPI(ban + 11 + ctn);
            getRequest.ban = ban + 11;//getRequest.ban = Convert.ToInt64(ban+11);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(ban)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                var result = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='ICC' and subscriber_no = '" + ctn_x + "'");
                var result1 = Executor.ExecuteSelect("select atr_value from ecr9_lg_res_atr where ban=" + ban + " and ph_type='GSMSIM' and atr_name='IMSI' and subscriber_no = '" + ctn_x + "'");
                if (result[0, 0] == s[0].serialNumber && result1[0, 0] == s[0].imsi)
                {
                    Logger.PrintRezult(true, "Номер совпадает со значением в бд");
                }
                else
                {
                    Logger.PrintRezult(false, "Номер не совпадает со значениемв бд");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20005 && faultException.Detail.errorDescription == "CTN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
                globalR = false;
            }
        }
        
        public void step_07_s()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            getRequest.token = token + "ABC";
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "Выполнить запрос с несуществующими параметрами(token)");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20003 && faultException.Detail.errorDescription == "TOKEN_NOT_FOUND")
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }
        #endregion
        #endregion


        
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getSIMListRequest getRequest = new getSIMListRequest();
            string login = ReaderTestData.ReadExel(testName, "Login13");
            string password = ReaderTestData.ReadExel(testName, "Password13");
            string ctn = ReaderTestData.ReadExel(testName, "ctn");
            string ban = ReaderTestData.ReadExel(testName, "BAN13");
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
            getRequest.token = token;
            getRequest.ban = ban;//getRequest.ban = Convert.ToInt32(ban);
            
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение номера SIM", "По SSO");
                var requestResponse = si.getSIMList(getRequest);
                var s = requestResponse.SIMList;
                Logger.PrintAction("номера Sim получены ", s[0].serialNumber);
                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> faultException)
            {
                Logger.PrintRezult(true, "Ошибка при получении номера SIM: " + faultException.Detail.errorCode + " " + faultException.Detail.errorDescription);
                if (faultException.Detail.errorCode == 20006 && faultException.Detail.errorDescription.Contains("FORBIDDEN"))
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
                Logger.PrintRezult(false, "Ошибка при получении номера SIM: " + ex.Message);
                Assertion("Ошибка при получении номера SIM: " + ex.Message, Assert.Fail);
            }
        }

        [Test]
        public void step_13at()
        {
            step_01("Login13", "Password13", "ctn13", "BAN13", " 13");
            step_02();
            step_03();
            step_04();
            step_05();
            step_06();
            step_07();
            step_08();
            step_01_s("Login13", "Password13", "ctn13", "BAN13", " 13");
            step_02_s();
            step_03_s();
            step_04_s();
            step_05_s();
            step_06_s();
            step_07_s();
        }

        [Test]
        public void step_101at()
        {
            step_01("Login101", "Password101", "ctn101", "BAN101", " 101");
            step_02();
            step_03();
            step_04();
            step_05();
            step_06();
            step_07();
            step_01_s("Login101", "Password101", "ctn101", "BAN101", " 101");
            step_02_s();
            step_03_s();
            step_04_s();
            step_05_s();
            step_06_s();
            step_07_s();
        }
        [Test]
        public void step_37at()
        {
            step_01("Login37", "Password37", "ctn37", "BAN37", " 37");
            step_02();
            step_03();
            step_04();
            step_05();
            step_06();
            step_07();
            step_01_s("Login37", "Password37", "ctn37", "BAN37", " 37");
            step_02_s();
            step_03_s();
            step_04_s();
            step_05_s();
            step_06_s();
            step_07_s();
        }
        [Test]
        public void step_105at()
        {
            step_01("Login105", "Password105", "ctn105", "BAN105", " 105");
            step_02();
            step_03();
            step_04();
            step_05();
            step_06();
            step_07();
            step_01_s("Login105", "Password105", "ctn105", "BAN105", " 105");
            step_02_s();
            step_03_s();
            step_04_s();
            step_05_s();
            step_06_s();
            step_07_s();
        }
        [Test]
        public void step99_finish()
        {
            Logger.PrintAction("Завершение тестирования","");
            Logger.PrintRezultTest(globalR);
        }
    }
}
