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
    class RequestListTest : TestBase
    {
        static string testName = "[SOAP API] Получение списка запросов пользователя";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        DateTime startDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "startDate"));
        DateTime endDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "endDate"));
        private long requestId = Convert.ToInt64(ReaderTestData.ReadExel(testName, "requestId"));
        private string startDateHash =
            ReaderTestData.ReadExel(testName, "startDate").Replace("T", "").Replace(":", "").Replace("-", "");
        private string endDateHash =
            ReaderTestData.ReadExel(testName, "endDate").Replace("T", "").Replace(":", "").Replace("-", "");
        private string loginx = ReaderTestData.ReadExel(testName, "loginx");
        int page = 1;
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
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            getRequests.requestId = requestId;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                var s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", s.requests[0].requestComments[0]);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,"Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = loginx;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            getRequests.requestId = requestId;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", s.requests[0].requestComments[0]);
            }
            catch(FaultException<UssWsApiException> exception)
            {
                if(exception.Detail.errorCode==20006 && exception.Detail.errorDescription=="FORBIDDEN (login="+login+")")
                 Logger.PrintRezult(true,exception.Detail.errorCode+exception.Detail.errorDescription);
                else
                {
                    Logger.PrintRezult(false, exception.Detail.errorCode + exception.Detail.errorDescription);
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.page = page;
            getRequests.hash = "?";
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
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
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = ths.GetHashAPI(login+startDateHash+endDateHash+requestId);
            getRequests.recordsPerPage = 20;
            getRequests.requestId = requestId;
            
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", s.requests[0].requestComments[0]);
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_s_03()
        {
            Logger.PrintStepName("Step 3");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = ths.GetHashAPI(login + startDateHash + endDateHash+"0");
            getRequests.recordsPerPage = 20;
            
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_s_04()
        {
            Logger.PrintStepName("Step 4");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.startDate = startDate;
            getRequests.endDate = Convert.ToDateTime(null);
            getRequests.page = page;
            getRequests.hash = ths.GetHashAPI(login + startDateHash + "0");
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_s_05()
        {
            Logger.PrintStepName("Step 5");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.endDate = endDate;
            getRequests.startDate = Convert.ToDateTime(null);
            getRequests.page = page;
            getRequests.hash = ths.GetHashAPI(login + endDateHash + "0");
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
        [Test]
        public void step_s_06()
        {
            Logger.PrintStepName("Step 6");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = loginx;
            getRequests.endDate = endDate;
            getRequests.startDate = startDate;
            getRequests.page = page;
            getRequests.hash = ths.GetHashAPI(loginx+startDateHash+endDateHash+requestId);
            getRequests.recordsPerPage = 20;
            getRequests.requestId = requestId;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                if (s.requests == null)
                    Logger.PrintRezult(true, "");
                Logger.PrintAction("Список заявок получен", s.requests[0].requestComments[0]);
            }
            catch (FaultException<UssWsApiException> exception)
            {
                if (exception.Detail.errorCode == 20006 && exception.Detail.errorDescription == "FORBIDDEN (login=" + login + ")")
                    Logger.PrintRezult(true, exception.Detail.errorCode + exception.Detail.errorDescription);
                else
                {
                    Logger.PrintRezult(false, exception.Detail.errorCode + exception.Detail.errorDescription);
                }
            }
            catch (Exception ex)
            {
                
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }

        [Test]
        public void step_s_07()
        {
            Logger.PrintStepName("Step 7");
            SubscriberService.SubscriberInterface si = new SubscriberInterfaceClient();
            SubscriberService.getRequestListRequest getRequests = new getRequestListRequest();
            getRequests.token = token;
            getRequests.login = login;
            getRequests.page = page;
            getRequests.startDate = Convert.ToDateTime(null);
            getRequests.endDate = Convert.ToDateTime(null);
            getRequests.hash = ths.GetHashAPI(login + "0");
            getRequests.recordsPerPage = 20;
            try
            {
                Logger.PrintAction("Получение списка запросов", "");
                SubscriberService.getRequestListResponse requestList = si.getRequestList(getRequests);
                apiRequestPageDO s = requestList.requestList;
                Logger.PrintAction("Список заявок получен", "");
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения истории запросов: " + ex.Message);
            }
        }
    }
}
