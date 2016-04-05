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
    class GetServicesTests: TestBase
    {
        static string testName = "[SOAP API] Получение списка услуг";
        private string login;// = ReaderTestData.ReadExel(testName, "Login13");
        private string password;//= ReaderTestData.ReadExel(testName, "Password13");
        private string ctn;//= ReaderTestData.ReadExel(testName, "ctn13");
        private string ban;//= Convert.ToInt64(ReaderTestData.ReadExel(testName, "BAN13"));
        private string ctn_x = ReaderTestData.ReadExel(testName, "ctn_x");
        TokenHashSoap ths = new TokenHashSoap();
        private string token;
        private bool globalR = true;
        string db_Ans = ReaderTestData.ReadCExel(4, 7);

        public void step_01(string exlogin, string expas, string exctn, string exban, string at)
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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.ban = ban;

            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;
                Logger.PrintAction("Список услуг получен", "");
                var query = Executor.ExecuteSelect("select subscriber_no, soc, effective_date, expiration_date from ecr9_service_agreement where subscriber_no = '"+ctn+"' and service_type <> 'P'");
                int row=0, reqCount=0,rowmax = query.Count,reqCountmax = s.Count();
                //string subscriber_no, soc, effective_date, expiration_date;
                while (reqCount<reqCountmax)
                {
                    while(row<rowmax)
                    {
                        if(s[reqCount].serviceId == query[row,1].Replace(" ",""))
                        {
                            var soc = query[row, 1].Replace(" ", "");
                            bool soc_name=false;
                            var soc_name1 =Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code like '" + soc + "%'");
                            var soc_name2 =Executor.ExecuteSelect("select price_plan_description from price_plan where external_price_plan like '" +soc + "%'");
                            var soc_name3 =Executor.ExecuteSelect("select soc_description from soc@" + db_Ans + " where soc like '" +soc + "%'");
                            if (soc_name1.Count != 0)
                            {
                                if (soc_name1[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name2.Count != 0)
                            {
                                if (soc_name2[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name3.Count != 0)
                            {
                                if (soc_name3[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if(soc_name==false)
                            {
                                Logger.PrintRezult(false, "С тарифами всё плохо");
                                globalR = false;
                                
                            }
                            if (!query[row, 0].Contains(s[reqCount].ctn))
                            {
                                Logger.PrintRezult(false,"Не совпадает ctn");
                                globalR = false;
                            }
                            if(s[reqCount].serviceId!=soc)
                            {
                                Logger.PrintRezult(false, "Не совпадает soc");
                                globalR = false;
                            }
                            if(s[reqCount].startDate.Date.ToString()!=query[row,2])
                            {
                                Logger.PrintRezult(false,"Не совпадает дата");
                                globalR = false;
                            }
                            row = 100500;
                        }
                        row++;
                    }
                    reqCount++;
                    row = 0;
                }
                Logger.PrintRezult(true,"Список услуг получен корректно");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,"Ошибка при получении списка услуг " + ex.Message);
            }
        }

        public void step_03()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;

            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;
                Logger.PrintAction("Список услуг получен", "");
                var query = Executor.ExecuteSelect("select subscriber_no, soc, effective_date, expiration_date from ecr9_service_agreement where ban = '" + ban + "' and service_type <> 'P'");
                int row = 0, reqCount = 0, rowmax = query.Count, reqCountmax = s.Count();
                //string subscriber_no, soc, effective_date, expiration_date;
                while (reqCount < reqCountmax)
                {
                    while (row < rowmax)
                    {
                        if (s[reqCount].serviceId == query[row, 1].Replace(" ", ""))
                        {
                            var soc = query[row, 1].Replace(" ", "");
                            bool soc_name = false;
                            var soc_name1 = Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code like '" + soc + "%'");
                            var soc_name2 = Executor.ExecuteSelect("select price_plan_description from price_plan where external_price_plan like '" + soc + "%'");
                            var soc_name3 = Executor.ExecuteSelect("select soc_description from soc@" + db_Ans + " where soc like '" + soc + "%'");
                            if (soc_name1.Count != 0)
                            {
                                if (soc_name1[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name2.Count != 0)
                            {
                                if (soc_name2[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name3.Count != 0)
                            {
                                if (soc_name3[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name == false)
                            {
                                Logger.PrintRezult(false, "С тарифами всё плохо");
                                globalR = false;

                            }
                            if (!query[row, 0].Contains(s[reqCount].ctn))
                            {
                                Logger.PrintRezult(false, "Не совпадает ctn");
                                globalR = false;
                            }
                            if (s[reqCount].serviceId != soc)
                            {
                                Logger.PrintRezult(false, "Не совпадает soc");
                                globalR = false;
                            }
                            if (s[reqCount].startDate.Date.ToString() != query[row, 2])
                            {
                                Logger.PrintRezult(false, "Не совпадает дата");
                                globalR = false;
                            }
                            row = 100500;
                        }
                        row++;
                    }
                    reqCount++;
                    row = 0;
                }
                Logger.PrintRezult(true, "Список услуг получен корректно");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении списка услуг " + ex.Message);
            }
        }
        public void step_04()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn_x;
            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
        public void step_05()
        {

            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            //getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение списка услуг", "Не введен токен");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
                Logger.PrintAction("Получение списка услуг", "Не введен ban");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn + "99";
            try
            {
                Logger.PrintAction("Получение списка услуг", "Несуществующий ctn");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            try
            {
                Logger.PrintAction("Получение списка услуг", "Несуществующий бан");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token + "AA";
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение списка услуг", "Несуществующий токен");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.ctn = ctn;
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;
                Logger.PrintAction("Список услуг получен", "");
                var query = Executor.ExecuteSelect("select subscriber_no, soc, effective_date, expiration_date from ecr9_service_agreement where subscriber_no = '" + ctn + "' and service_type <> 'P'");
                int row = 0, reqCount = 0, rowmax = query.Count, reqCountmax = s.Count();
                //string subscriber_no, soc, effective_date, expiration_date;
                while (reqCount < reqCountmax)
                {
                    while (row < rowmax)
                    {
                        if (s[reqCount].serviceId == query[row, 1].Replace(" ", ""))
                        {
                            var soc = query[row, 1].Replace(" ", "");
                            bool soc_name = false;
                            var soc_name1 = Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code like '" + soc + "%'");
                            var soc_name2 = Executor.ExecuteSelect("select price_plan_description from price_plan where external_price_plan like '" + soc + "%'");
                            var soc_name3 = Executor.ExecuteSelect("select soc_description from soc@" + db_Ans + " where soc like '" + soc + "%'");
                            if (soc_name1.Count != 0)
                            {
                                if (soc_name1[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name2.Count != 0)
                            {
                                if (soc_name2[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name3.Count != 0)
                            {
                                if (soc_name3[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name == false)
                            {
                                Logger.PrintRezult(false, "С тарифами всё плохо");
                                globalR = false;

                            }
                            if (!query[row, 0].Contains(s[reqCount].ctn))
                            {
                                Logger.PrintRezult(false, "Не совпадает ctn");
                                globalR = false;
                            }
                            if (s[reqCount].serviceId != soc)
                            {
                                Logger.PrintRezult(false, "Не совпадает soc");
                                globalR = false;
                            }
                            if (s[reqCount].startDate.Date.ToString() != query[row, 2])
                            {
                                Logger.PrintRezult(false, "Не совпадает дата");
                                globalR = false;
                            }
                            row = 100500;
                        }
                        row++;
                    }
                    reqCount++;
                    row = 0;
                }
                Logger.PrintRezult(true, "Список услуг получен корректно");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении списка услуг " + ex.Message);
            }
        }

        public void step_03_s()
        {

            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.hash = ths.GetHashAPI(ban);
            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;
                Logger.PrintAction("Список услуг получен", "");
                var query = Executor.ExecuteSelect("select subscriber_no, soc, effective_date, expiration_date from ecr9_service_agreement where ban = '" + ban + "' and service_type <> 'P'");
                int row = 0, reqCount = 0, rowmax = query.Count, reqCountmax = s.Count();
                //string subscriber_no, soc, effective_date, expiration_date;
                while (reqCount < reqCountmax)
                {
                    while (row < rowmax)
                    {
                        if (s[reqCount].serviceId == query[row, 1].Replace(" ", ""))
                        {
                            var soc = query[row, 1].Replace(" ", "");
                            bool soc_name = false;
                            var soc_name1 = Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code like '" + soc + "%'");
                            var soc_name2 = Executor.ExecuteSelect("select price_plan_description from price_plan where external_price_plan like '" + soc + "%'");
                            var soc_name3 = Executor.ExecuteSelect("select soc_description from soc@" + db_Ans + " where soc like '" + soc + "%'");
                            if (soc_name1.Count != 0)
                            {
                                if (soc_name1[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name2.Count != 0)
                            {
                                if (soc_name2[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name3.Count != 0)
                            {
                                if (soc_name3[0, 0].Contains(s[reqCount].serviceName))
                                {
                                    soc_name = true;
                                }
                            }
                            if (soc_name == false)
                            {
                                Logger.PrintRezult(false, "С тарифами всё плохо");
                                globalR = false;

                            }
                            if (!query[row, 0].Contains(s[reqCount].ctn))
                            {
                                Logger.PrintRezult(false, "Не совпадает ctn");
                                globalR = false;
                            }
                            if (s[reqCount].serviceId != soc)
                            {
                                Logger.PrintRezult(false, "Не совпадает soc");
                                globalR = false;
                            }
                            if (s[reqCount].startDate.Date.ToString() != query[row, 2])
                            {
                                Logger.PrintRezult(false, "Не совпадает дата");
                                globalR = false;
                            }
                            row = 100500;
                        }
                        row++;
                    }
                    reqCount++;
                    row = 0;
                }
                Logger.PrintRezult(true, "Список услуг получен корректно");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка при получении списка услуг " + ex.Message);
            }
        }
        public void step_04_s()
        {

            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn_x;
            getRequest.hash = ths.GetHashAPI(ban + ctn_x);
            try
            {
                Logger.PrintAction("Получение списка услуг", "");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            //getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban+ctn);
            try
            {
                Logger.PrintAction("Получение списка услуг", "Не введен токен");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
                Logger.PrintAction("Получение списка услуг", "Не введен ban");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn + "99";
            getRequest.hash = ths.GetHashAPI(ban + ctn + "99");
            try
            {
                Logger.PrintAction("Получение списка услуг", "Несуществующий ctn");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
                Logger.PrintAction("Получение списка услуг", "Несуществующий бан");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

                if (s == null)
                {
                    Logger.PrintRezult(true, "Метод отрабатывает верно");
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

        public void step_07_s()
        {
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            getRequest.token = token + "AA";
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            getRequest.hash = ths.GetHashAPI(ban + ctn);
            try
            {
                Logger.PrintAction("Получение списка услуг", "Несуществующий токен");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

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
            SubscriberService.getServicesListRequest getRequest = new getServicesListRequest();
            ban = ReaderTestData.ReadExel(testName, "BAN105");
            ctn = ReaderTestData.ReadExel(testName, "ctn105");
            getRequest.token = token;
            getRequest.ban = ban;
            getRequest.ctn = ctn;
            try
            {
                Logger.PrintAction("Получение информации о ctn", "Не аутентифицированный");
                SubscriberService.getServicesListResponse requestResponse = si.getServicesList(getRequest);
                var s = requestResponse.servicesList;

                if (s[0] != null)
                {
                    Logger.PrintRezult(false, "Метод отрабатывает");
                    globalR = false;
                }
            }
            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20006 && exception.Detail.errorDescription == "FORBIDDEN (login=" + login + ")")
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
