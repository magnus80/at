using System.Threading;
using AT;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.B2BPost;
using USSS.WebPages.CommonPages;
using AT.DataBase;
using RequestHistoryPage = USSS.WebPages.B2BPost.RequestHistoryPage;


namespace USSS.Tests.Sanity.B2BPost
{
    [TestFixture]
    [Category("USSS_t")]

    class InvoiceBillsReports : TestBase
    {
        static string testName = "[SANITY][B2B] Заказ отчёта по начислениям выставленного счёта";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        string db_sms = ReaderTestData.ReadCExel(4, 10);
        string ReqID = "";
        string ReqID2 = "";
        AuthorizationPage ap;
        HomePage homePage;
        ReportsPage reportsPage;
        RequestHistoryPage requestHistoryPage;
        FinancePage financePage;
        bool globalR = true;

        //  

        [Test]
        public void step_01() //Авторизоваться под пользователем b2b post
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
            //Проверка отображения страницы авторизации
            Logger.PrintAction("Проверка отображения страницы авторизации", "");
            rezult = ap.ConstructionPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница авторизации корректна");
            }
            rezult = "";
            //Авторизация
            Logger.PrintAction("Авторизация", "Логин:" + login + ", Пароль: " + password);
            rezult = ap.Logon(login, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Logger.PrintRezultTest(globalR);
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            rezult = "";
            //Проверка отображения профиля
            homePage = new HomePage();
            Logger.PrintAction("Проверка отображения главной страницы", "");
            rezult = homePage.ConstructionPage("105");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Главная страница корректна");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Переход в  Отчеты'", "");
            string rezult = homePage.GoToReportsPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчеты корректна");
            }
            reportsPage = homePage.reportsPage;
        }
        [Test]

        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбрать отчет по начислениям выставленного счёта'", "");
            string rezult = reportsPage.GoReportAccrualsInvoice();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчета корректна");
            }
          //  reportsPage = homePage.reportsPage;
        }

        [Test]

        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выбрать расчетный период и нажать заказать отчет'", "");
            string rezult = reportsPage.SelectAccrualsPeriod();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
          //  else
          //  {
            //    Logger.PrintRezult(true, "Страница Отчета корректна");
          //  }
            //  reportsPage = homePage.reportsPage;
            rezult = reportsPage.ClickOrderReport("Accruals");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отчет заказан успешно.");
            }
            
        }

        [Test]
        public void step_05()
        {
            

            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проставить чекбоксы, ввести номер телефона, емейл, нажать кнопку подтвердить'", "");
            string rezult = reportsPage.CheckSubmitNotifWindow(ref ReqID);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчета корректна запрос создан: " + ReqID);
            }
            //  reportsPage = homePage.reportsPage;
            //rezult = reportsPage.ClickOrderReport();
            //if (rezult != "success")
            //{
            //    globalR = false;
            //    Logger.PrintRezult(false, rezult);
            //}
            //else
            //{
            //    Logger.PrintRezult(true, "Отчет заказан успешно.");
            //}
        }


        [Test]
         public void step_06()
         {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Перейти в раздел история запросов'", "");

            Thread.Sleep(5000);
             string rezult = homePage.GoToRequestHistoryPage();
             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Страница истории запросов корректна ");
             }
             requestHistoryPage = homePage.requestHistoryPage;
             ReqID = ReqID.Replace("№", "");
             rezult = requestHistoryPage.CheckStatus(ref ReqID);
             if (rezult != "Обработан")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "запрос обработан: " + ReqID);
             }

             rezult = requestHistoryPage.CheckDownloadReportLink();
             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "ссылка Загрузить отчет найдена");
             }
         }

        // TODO Скачать отчет и чето там проверить.

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Переход в  Отчеты'", "");
            string rezult = homePage.GoToReportsPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчеты корректна");
            }
            reportsPage = homePage.reportsPage;
        }

        [Test]

        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Выбрать отчет по детализации выставленного счета'", "");
            string rezult = reportsPage.GoReportDetail();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчета корректна");
            }
            //  reportsPage = homePage.reportsPage;
        }


        [Test]

        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Выбрать расчетный период и нажать заказать отчет'", "");
            string rezult = reportsPage.SelectAccrualsPeriod();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            //  else
            //  {
            //    Logger.PrintRezult(true, "Страница Отчета корректна");
            //  }
            //  reportsPage = homePage.reportsPage;
            rezult = reportsPage.ClickOrderReport("Details");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отчет заказан успешно.");
            }

        }
        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Проставить чекбоксы, ввести номер телефона, емейл, нажать кнопку подтвердить'", "");
            string rezult = reportsPage.CheckSubmitNotifWindow(ref ReqID2);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Отчета корректна запрос создан: " + ReqID2);
            }

        }
        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Перейти в раздел история запросов'", "");

            string rezult = homePage.GoToRequestHistoryPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории запросов корректна ");
            }
            requestHistoryPage = homePage.requestHistoryPage;
            ReqID2 = ReqID2.Replace("№", "");
            rezult = requestHistoryPage.CheckStatus(ref ReqID2);
            if (rezult != "Обработан")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "запрос обработан: " + ReqID2);
            }

            rezult = requestHistoryPage.CheckDownloadReportLink();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "ссылка Загрузить отчет найдена");
            }
        }

        // TODO Скачать отчет и чето там проверить.
        [Test]

        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Проверить полученные уведомления - СМС", ""); //TODO: проверка e-mail.
            string sms = "";
            var q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + ReqID2 + "'";
            var smsB = Executor.ExecuteSelect(q);
             if (smsB.Count!=0){
             sms = smsB[0, 0];
          
            if (sms.Contains("Запрос") & sms.Contains("на формирование")  &
                sms.Contains("отчёта детализации успешно обработан") & sms.Contains("my.beeline.ru"))
            {
                Logger.PrintRezult(true, "СМС корректна");
            }
            else
            {
                Logger.PrintRezult(false, "СМС некорректна");
                globalR = false;
            }
            }
            else
            {
                Logger.PrintRezult(false, "СМС не найдена");
                globalR = false;
            }
            q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + ReqID + "'";
             smsB = Executor.ExecuteSelect(q);
             if (smsB.Count != 0)
             {
                 sms = smsB[0, 0];

                 if (sms.Contains("Запрос") & sms.Contains("на формирование") &
                     sms.Contains("отчёта по начислениям счёта успешно обработан") & sms.Contains("my.beeline.ru"))
                 {
                     Logger.PrintRezult(true, "СМС корректна");
                 }
                 else
                 {
                     Logger.PrintRezult(false, "СМС некорректна");
                     globalR = false;
                 }
             }
             else
             {
                 Logger.PrintRezult(false, "СМС не найдена");
                 globalR = false;
             }
           
        }

        [Test]
        public void step_13()
        {
            Logger.PrintStepName("Step 13");
            Logger.PrintAction("Перейти в раздел Финансовая информация ", "");

            string rezult = homePage.GoToFinancePage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница  Финансовой информация корректна ");
            }
            financePage = homePage.financePage;
        }

        [Test]
        public void step_14()
        {
            Logger.PrintStepName("Step 14");
            Logger.PrintAction("Выбрать вкладку все счета, нажать на номер счета.", "");

            string rezult = financePage.SelectAllBills();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладка выбрана, счета отображены ");
            }

            rezult = financePage.ClickOnBill();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Счет выбран");
            }

            rezult = financePage.CheckBillPage(); //TODO : Выпадающий список - структура расходов по пользователям 
            if (rezult != "success")               //TODO:  там надо выбрать СЧЕТА КОМПАНИИ, и тогда селектор будет, но у нас нет такого юзера что бы он удовлетворял еще и другим условиям, что делать хз
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница счета корректна");
            }
            
        }

        [Test]
        public void step_15()
        {
            Logger.PrintStepName("Step 15");
            Logger.PrintAction("Нажать псевдоссылку Заказать отчет по детализации", "");

            string rezult = financePage.ClickOrderXLSReport();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ссылка нажата ");
            }

           // Logger.PrintStepName("Step 10");
            Logger.PrintAction("Проставить чекбоксы, ввести номер телефона, емейл, нажать кнопку подтвердить'", "");
             rezult = reportsPage.CheckSubmitNotifWindow();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
          //  else
           // {
              //  Logger.PrintRezult(true, "запрос создан " );
           // }

            rezult = homePage.GoToRequestHistoryPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории запросов корректна ");
            }
            requestHistoryPage = homePage.requestHistoryPage;
            rezult = requestHistoryPage.getLastRequestId(ref ReqID2);
            
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "запрос создан ");
            }

            //ReqID2 = ReqID2.Replace("№", "");
            rezult = requestHistoryPage.CheckStatus(ref ReqID2);
            if (rezult != "Обработан")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "запрос обработан: " + ReqID2);
            }

            rezult = requestHistoryPage.CheckDownloadReportLink();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "ссылка Загрузить отчет найдена");
            }

            Logger.PrintAction("Проверить полученные уведомления - СМС", ""); //TODO: проверка e-mail.
            string sms = "";
            var q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + ReqID2 + "'";
            var smsB = Executor.ExecuteSelect(q);
            if (smsB.Count != 0)
            {
                sms = smsB[0, 0];

                if (sms.Contains("Запрос") & sms.Contains("на формирование") &
                    sms.Contains("отчёта детализации успешно обработан") & sms.Contains("my.beeline.ru"))
                {
                    Logger.PrintRezult(true, "СМС корректна");
                }
                else
                {
                    Logger.PrintRezult(false, "СМС некорректна");
                    globalR = false;
                }
                
            }
            ap.Close();
            Logger.PrintRezultTest(globalR);
        }


        
    }
}
