﻿//using AT;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using USSS.Helpers;
//using USSS.UmcsService;

//namespace USSS.Tests.SOAP
//{
//    [TestFixture]
//    [Category("SOAP")]
//    class AccountGetValueTest : TestBase
//    {
//        static string testName = "[SOAP API] Получение значений баланса САС и МК";
//        string login = ReaderTestData.ReadExel(testName, "Login");
//        string password = ReaderTestData.ReadExel(testName, "Password");
//        string ctn = ReaderTestData.ReadExel(testName, "phoneNumber");
//        long ban = Convert.ToInt64(ReaderTestData.ReadExel(testName, "ban"));
//        long requestId = Convert.ToInt64(ReaderTestData.ReadExel(testName, "requestId"));
//        TokenHashSoap ths = new TokenHashSoap();
//        string token = String.Empty;
//        DateTime startDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "StartDate"));
//        DateTime endDate = DateTime.Parse(ReaderTestData.ReadExel(testName, "EndDate"));

//        [Test]
//        public void step_01()
//        {

//            Logger.PrintHeadTest(testName);
//            Logger.PrintStepName("Step 1");
//            Logger.PrintAction("подключению к сервису", "");


//            try
//            {
//                token = ths.GetToken(login, password);
//                Logger.PrintAction("Токен получен", token);
//            }
//            catch (Exception ex)
//            {
//                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
//            }
//        }

//        [Test]
//        public void step_02()
//        {

//            Logger.PrintStepName("Step 2");
//            UmcsService.UMCSSoapClient usp = new UMCSSoapClient();
            

//            try
//            {
//                Logger.PrintAction("Получение значений баланса САС и МК", "");
//                string a;
//                int i;
//                int requestResponse = usp.AccountGetValue("", ctn, true, out a, out a, out a, out a, out a,out i, out a);
//                Logger.PrintAction("Данные получены", "");
//            }
//            catch (Exception ex)
//            {
//                Assertion("Ошибка при получении значений баланса САС и МК " + ex.Message, Assert.Fail);
//            }
//        }
//    }
//}
