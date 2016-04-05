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
using AT.DataBase;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    class GetCtnInfoTest:TestBase
    {
        static string testName = "[SOAP API] Получение информации о ctn";
        private string login;// = ReaderTestData.ReadExel(testName, "Login13");
        private string password;//= ReaderTestData.ReadExel(testName, "Password13");
        private string ctn;//= ReaderTestData.ReadExel(testName, "ctn13");
        private string ban;//= Convert.ToInt64(ReaderTestData.ReadExel(testName, "BAN13"));
        private string ctn_x = ReaderTestData.ReadExel(testName, "ctn_x");
        TokenHashSoap ths = new TokenHashSoap();
        private string token;
        private bool globalR = true;
        
        public void step_01(string exlogin,string expas,string exctn,string exban, string at)
        {
            login = ReaderTestData.ReadExel(testName, exlogin);
            password = ReaderTestData.ReadExel(testName, expas);
            ctn = ReaderTestData.ReadExel(testName, exctn);
            ban = ReaderTestData.ReadExel(testName, exban);

            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "Тип аккаунта"+at);

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
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.ban = ban;

            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;
                
                Logger.PrintAction("Информация о ctn получена", "");

                var query = Executor.ExecuteSelect("SELECT SUBSCRIBER_NO AS CTN, SUB_STATUS_DATE AS statusDate, SUB_STATUS AS status, CURRENT_PP AS prШcePAL, SUB_STATUS_RSN_CODE AS reasonStatus from ecr9_subscriber WHERE SUBSCRIBER_NO = '"+ctn+"'");
                string qctn = s[0].ctn.Remove(0, 1);
                if(query[0,0].Replace(" ","")!=qctn)
                {
                    Logger.PrintRezult(false,"Не совпадает номер стн");
                    globalR = false;
                }
                string qStatusDate = Convert.ToString(s[0].statusDate);
                if(query[0,1]!=qStatusDate)
                {
                    Logger.PrintRezult(false, "Не совпадает дата");
                    globalR = false;
                }
                string qStatus = Convert.ToString(s[0].status[0]);
                if(query[0,2]!=qStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает status");
                    globalR = false;
                }
                string qPricePlan = s[0].pricePlan.Replace(" ", "");
                if(query[0,3].Replace(" ","")!=qPricePlan)
                {
                    Logger.PrintRezult(false, "Не совпадает priceplan");
                    globalR = false;
                }
                if (query[0, 4].Replace(" ", "") != s[0].reasonStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает reasonstatus");
                    globalR = false;
                }
                Logger.PrintRezult(true,"Данные корректны");
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false,exception.Detail.errorDescription+" "+exception.Detail.errorCode);
                globalR = false;
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,ex.Message);
                globalR = false;
            }
            
        }
        public void step_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();            
            getRequest.token = token;
            getRequest.ban = ban;

            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                Logger.PrintAction("Информация о ctn получена", "");

                var query = Executor.ExecuteSelect("SELECT SUBSCRIBER_NO AS CTN, SUB_STATUS_DATE AS statusDate, SUB_STATUS AS status, CURRENT_PP AS prШcePAL, SUB_STATUS_RSN_CODE AS reasonStatus from ecr9_subscriber WHERE SUBSCRIBER_NO = '" + ctn + "'");
                string qctn = s[0].ctn.Remove(0, 1);
                if (query[0, 0].Replace(" ", "") != qctn)
                {
                    Logger.PrintRezult(false, "Не совпадает номер стн");
                    globalR = false;
                }
                string qStatusDate = Convert.ToString(s[0].statusDate);
                if (query[0, 1] != qStatusDate)
                {
                    Logger.PrintRezult(false, "Не совпадает дата");
                    globalR = false;
                }
                string qStatus = Convert.ToString(s[0].status[0]);
                if (query[0, 2] != qStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает status");
                    globalR = false;
                }
                string qPricePlan = s[0].pricePlan.Replace(" ", "");
                if (query[0, 3].Replace(" ", "") != qPricePlan)
                {
                    Logger.PrintRezult(false, "Не совпадает priceplan");
                    globalR = false;
                }
                if (query[0, 4].Replace(" ", "") != s[0].reasonStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает reasonstatus");
                    globalR = false;
                }
                Logger.PrintRezult(true, "Данные корректны");
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                globalR = false;
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }
        public void step_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn_x;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if(s[0]!=null)
                {
                    Logger.PrintRezult(false,"Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20005 && exception.Detail.errorDescription == "CTN_NOT_FOUND")
                {
                    Logger.PrintRezult(true, "Код ошибки корректный "+exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
                else
                {
                    Logger.PrintRezult(false, "Некорректный код ошибки " + exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }
        public void step_05()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            //getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не введен токен");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

            getRequest.token = token;
            getRequest.ban = null;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не введен ban");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
        }
        public void step_06()
        {

            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn+"99";
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий ctn");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
            getRequest.token = token;
            getRequest.ban = ban+"11";
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий бан");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }

        public void step_07()
        {
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token + "AA";
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий токен");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
        }
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

            Logger.PrintStepName("Step 2");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                Logger.PrintAction("Информация о ctn получена", "");

                var query = Executor.ExecuteSelect("SELECT SUBSCRIBER_NO AS CTN, SUB_STATUS_DATE AS statusDate, SUB_STATUS AS status, CURRENT_PP AS prШcePAL, SUB_STATUS_RSN_CODE AS reasonStatus from ecr9_subscriber WHERE SUBSCRIBER_NO = '" + ctn + "'");
                string qctn = s[0].ctn.Remove(0, 1);
                if (query[0, 0].Replace(" ", "") != qctn)
                {
                    Logger.PrintRezult(false, "Не совпадает номер стн");
                    globalR = false;
                }
                string qStatusDate = Convert.ToString(s[0].statusDate);
                if (query[0, 1] != qStatusDate)
                {
                    Logger.PrintRezult(false, "Не совпадает дата");
                    globalR = false;
                }
                string qStatus = Convert.ToString(s[0].status[0]);
                if (query[0, 2] != qStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает status");
                    globalR = false;
                }
                string qPricePlan = s[0].pricePlan.Replace(" ", "");
                if (query[0, 3].Replace(" ", "") != qPricePlan)
                {
                    Logger.PrintRezult(false, "Не совпадает priceplan");
                    globalR = false;
                }
                if (query[0, 4].Replace(" ", "") != s[0].reasonStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает reasonstatus");
                    globalR = false;
                }
                Logger.PrintRezult(true, "Данные корректны");
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                globalR = false;
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }
        public void step_03_s()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.hash = ths.GetHashAPI(ban);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                Logger.PrintAction("Информация о ctn получена", "");

                var query = Executor.ExecuteSelect("SELECT SUBSCRIBER_NO AS CTN, SUB_STATUS_DATE AS statusDate, SUB_STATUS AS status, CURRENT_PP AS prШcePAL, SUB_STATUS_RSN_CODE AS reasonStatus from ecr9_subscriber WHERE SUBSCRIBER_NO = '" + ctn + "'");
                string qctn = s[0].ctn.Remove(0, 1);
                if (query[0, 0].Replace(" ", "") != qctn)
                {
                    Logger.PrintRezult(false, "Не совпадает номер стн");
                    globalR = false;
                }
                string qStatusDate = Convert.ToString(s[0].statusDate);
                if (query[0, 1] != qStatusDate)
                {
                    Logger.PrintRezult(false, "Не совпадает дата");
                    globalR = false;
                }
                string qStatus = Convert.ToString(s[0].status[0]);
                if (query[0, 2] != qStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает status");
                    globalR = false;
                }
                string qPricePlan = s[0].pricePlan.Replace(" ", "");
                if (query[0, 3].Replace(" ", "") != qPricePlan)
                {
                    Logger.PrintRezult(false, "Не совпадает priceplan");
                    globalR = false;
                }
                if (query[0, 4].Replace(" ", "") != s[0].reasonStatus)
                {
                    Logger.PrintRezult(false, "Не совпадает reasonstatus");
                    globalR = false;
                }
                Logger.PrintRezult(true, "Данные корректны");
            }
            catch (FaultException<UssWsApiException> exception)
            {
                Logger.PrintRezult(false, exception.Detail.errorDescription + " " + exception.Detail.errorCode);
                globalR = false;
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }
        public void step_04_s()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn_x;
            getRequest.hash = ths.GetHashAPI(ban + ctn_x);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

        }
        public void step_05_s()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            //getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не введен токен");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

            getRequest.token = token;
            getRequest.ban = null;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ctn);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не введен ban");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
        }
        public void step_06_s()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn + "99";
            getRequest.hash = ths.GetHashAPI(ban + ctn + "99");
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий ctn");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
            getRequest.token = token;
            getRequest.ban = ban + "11";
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban + "11" + ctn);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий бан");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s == null)
                {
                    Logger.PrintRezult(true, "Метод отрабатывает верно");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }

            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban + ctn)+"AA";
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий хэш");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
        }

        public void step_07_s()
        {
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            getRequest.token = token + "AA";
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Несуществующий токен");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
            }
        }
        public void step_08()
        {
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getCTNInfoListRequest getRequest = new getCTNInfoListRequest();
            ban = ReaderTestData.ReadExel(testName, "BAN105");
            ctn = ReaderTestData.ReadExel(testName, "ctn105");
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не аутентифицированный");
                SubscriberService.getCTNInfoListResponse requestResponse = si.getCTNInfoList(getRequest);
                var s = requestResponse.CTNInfoList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20006 && exception.Detail.errorDescription == "FORBIDDEN (login="+login+")")
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
                Logger.PrintRezult(false, ex.Message);
                globalR = false;
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
        public void step_finish()
        {
            Logger.PrintRezultTest(globalR);
        }
    }
}
