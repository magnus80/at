using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AT;
using NUnit.Framework;
using USSS.Helpers;
using USSS.SubscriberService;
using USSS.UmcsService;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    class GetCACBalance:TestBase
    {
        static string testName = "[SOAP API] Получение значений баланса САС и МК";
        private string ctnCAC = ReaderTestData.ReadExel(testName, "ctnCAC");
        private string ctnNoCAC = ReaderTestData.ReadExel(testName, "ctnNoCAC");
        private string ctnBlockCAC = ReaderTestData.ReadExel(testName, "ctnBlockCAC");
        private bool globalR = true;

       [Test]
        public void step_01()
        {           
            Logger.PrintStepName("Step 1");
            UmcsService.UMCSSoap si = new UMCSSoapClient();
            UmcsService.AccountGetValueRequest getRequest = new AccountGetValueRequest();
            getRequest.phone = ctnCAC;
            getRequest.isSendSms = false;
            getRequest.shopCertId = "00-81-ee-b0-d9-b0-8c-58-30";
            try
            {
                Logger.PrintAction("Получение информации о САС балансе (САС присутствует)", "");
                UmcsService.AccountGetValueResponse requestResponse = si.AccountGetValue(getRequest);
                var s = requestResponse.AccountGetValueResult;
                var s1 = requestResponse.status;
                //var val = requestResponse.value.ToString();
                if ((s == 0) & (s1 == 0))
                {
                    Logger.PrintRezult(true, "Баланс САС присутствует, корректно");
                    Logger.PrintAction("Размера САС баланса: " + requestResponse.value.ToString(), "");
                }
            }
            catch
                (Exception ex)
                {
                    Logger.PrintRezult(false, "Ошибка получения баланса САС: " + ex.Message);
                    globalR = false;
                }
         
        }


        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            UmcsService.UMCSSoap si = new UMCSSoapClient();
            UmcsService.AccountGetValueRequest getRequest = new AccountGetValueRequest();
            getRequest.phone = ctnNoCAC;
            getRequest.isSendSms = false;
            getRequest.shopCertId = "00-81-ee-b0-d9-b0-8c-58-30";
            try
            {
                Logger.PrintAction("Получение информации о САС балансе (САС отсутствует)", "");
                UmcsService.AccountGetValueResponse requestResponse = si.AccountGetValue(getRequest);
                var s = requestResponse.AccountGetValueResult;
                var s1 = requestResponse.status;
                if ((s==0)&(s1==2))
                {
                    Logger.PrintRezult(true, "Баланс САС отсутствует, корректно");                    
                }            
            }
            catch
                (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения баланса САС: " + ex.Message);
                globalR = false;
            }

        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            UmcsService.UMCSSoap si = new UMCSSoapClient();
            UmcsService.AccountGetValueRequest getRequest = new AccountGetValueRequest();
            getRequest.phone = ctnBlockCAC;
            getRequest.isSendSms = false;
            getRequest.shopCertId = "00-81-ee-b0-d9-b0-8c-58-30";
            try
            {
                Logger.PrintAction("Получение информации о САС балансе (САС заблокирован)", "");
                UmcsService.AccountGetValueResponse requestResponse = si.AccountGetValue(getRequest);
                var s = requestResponse.AccountGetValueResult;
                var s1 = requestResponse.status;
                //var val = requestResponse.value.ToString();
                if ((s == 0) & (s1 == 1))
                {
                    Logger.PrintRezult(true, "Баланс САС заблокирован, корректно");
                    Logger.PrintAction("Размера САС баланса: " + requestResponse.value.ToString(), "");
                }
            }
            catch
                (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения баланса САС: " + ex.Message);
                globalR = false;
            }

        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            UmcsService.UMCSSoap si = new UMCSSoapClient();
            UmcsService.AccountGetValueRequest getRequest = new AccountGetValueRequest();
            getRequest.phone = ctnCAC;
            getRequest.isSendSms = false;
            getRequest.shopCertId = "00-81-ee-b0-d9-b0-8c-58-31";
            try
            {
                Logger.PrintAction("Получение информации о САС балансе (САС заблокирован)", "");
                UmcsService.AccountGetValueResponse requestResponse = si.AccountGetValue(getRequest);
                var s = requestResponse.AccountGetValueResult;
                //var s1 = requestResponse.status;
                if (s == 1)
                {
                    Logger.PrintRezult(true, "Сервис UMCS недоступен, корректно");
                }
            }
            catch
                (Exception ex)
            {
                Logger.PrintRezult(false, "Ошибка получения баланса САС: " + ex.Message);
                globalR = false;
            }

        Logger.PrintRezultTest(globalR);
        }   

    }
}
