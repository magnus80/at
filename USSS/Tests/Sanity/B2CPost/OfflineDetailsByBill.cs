using AT;
using NUnit.Framework;
using System;
using USSS.Helpers;
using USSS.WebPages.B2CPost;
using USSS.WebPages.CommonPages;
using AT.DataBase;
using RequestHistoryPage = USSS.WebPages.B2CPost.RequestHistoryPage;

namespace USSS.Tests.Sanity.B2CPost
{
    [TestFixture]
    [Category("USSS_t")]

    class OfflineDetailsByBill : TestBase
    {

        
        
        static string testName = "[SANITY][B2C] Оффлайн детализация по счету B2C postpaid";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        string BillNumber = "";
        string BAN = ReaderTestData.ReadExel(testName, "BAN");
        private string reqID;
        private string[] MonthArr =
            {
                "","Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь",
                "Октябрь", "Ноябрь", "Декабрь"
            };

        AuthorizationPage ap;
        ProfilePage profilePage;
        FinansAndDetalizationPage finansAndDetalizationPage;
        RequestHistoryPage requestHistoryPage;
        bool globalR = true;

        [Test]
        public void step_01()
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
                Logger.PrintRezult(false, rezult);
                globalR = false;
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
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            rezult = "";
            //Проверка отображения профиля
            profilePage = new ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePage.ConstructionPage();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница профиля корректна");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Переход на страницу Финансовая информация", "");
            string rezult = profilePage.GoToDetalizationHistory("105");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отображает блок счета и платежи.");
            }
            finansAndDetalizationPage = profilePage.finansAndDetalizationPage;
            rezult = "";
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбрать период и нажать кнопку Составить отчет.", "");
            string startDD, startMM, startYYYY, endDD, endMM, endYYYY;


            //   var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect("select logical_date from logical_date@" + db_Ans + @" where rownum<2");

            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');

            if (Convert.ToInt32(d[0]) < 10)
            {
                startDD = d[0].Substring(1);
            }
            else
            {
                startDD = d[0];
            }
            startMM = MonthArr[Convert.ToInt32(d[1])];
            startYYYY = (Convert.ToInt32(d[2]) - 1).ToString();
            endDD = startDD;
            endMM = startMM;
            endYYYY = d[2];

            finansAndDetalizationPage.SetPeriod(Convert.ToInt32(startDD), startMM, startYYYY, Convert.ToInt32(endDD), endMM, endYYYY);

            string rezult = finansAndDetalizationPage.CheckBillTable();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Таблица счетов корретная");
            }

            rezult = "";
        }

        [Test]
        public void step_04()
        {
            
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Нажать на номер счета", "");
            string rezult = finansAndDetalizationPage.ClickOnBill(1,ref BillNumber);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница счета отображена корректно");
            }


            rezult = finansAndDetalizationPage.ClickOnDownloadXLS(); 
            //TODO: проверить файл
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отчет типа загружен");
            }

            rezult = "";
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Нажать на псевдоссылку Отправить по е-мейл", "");
            string rezult = finansAndDetalizationPage.ClickSentOnEmail(BillNumber,BAN);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница счета отображена корректно");
            }
         
            rezult = "";
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Ввести е-мейл, нажать кнопку отправить", "");
            string rezult = finansAndDetalizationPage.SentDetailsOnMail(ref reqID);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отправки детальки на почту отображена корректно");
            }

            rezult = "";
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Перейти в историю заявок", "");
            string rezult = profilePage.GoToRequestHistoryPage();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории заявок отображена корректно");
            }
            requestHistoryPage = profilePage.requestHistoryPage;

            rezult = requestHistoryPage.CheckStatus(reqID);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Статус заявки -обработан");
            }

            rezult = requestHistoryPage.CheckTableField(4, "Счёт в формате pdf будет сформирован и отправлен на указанный Вами E-mail");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Текст Счет в формате пдф будет сформирован и отправлен... найден!");
            }
            rezult = "";
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Проверить данные в полученном письме с детализацией", "");
            string rezult = "Там нужно чето заходить в ансамбль и искать нужный файл хз как и надо ли"; //TODO : <<---
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Письмо коррекнтное");
            }

            rezult = "";
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Выбрать вкладку История операций, нажать псевдо ссылку Выгрузить в Excel", "");
            string rezult = profilePage.GoToDetalizationHistory("105"); 
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница детальки корректна");
            }
            rezult = finansAndDetalizationPage.ClickOnBill(1, ref BillNumber);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница счета отображена корректно");
            }

            rezult = finansAndDetalizationPage.GoToHistoryOperationsTab();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Данные на вкладке корректные.");
            }

            rezult = finansAndDetalizationPage.HistoryOperationPageClickDownloadXLS();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отчет скачан.");
            }

            rezult = "";
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Перейти в Финансовую информацию, нажать на псевдоссылку отправить по электронной почте", "");
            string rezult = profilePage.GoToDetalizationHistory("105");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница детальки корректна");
            }
                
                
                rezult = finansAndDetalizationPage.BillPageClickSentOnEmail(BillNumber, BAN); 
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Письмо коррекнтное");
            }

            rezult = "";
        }

        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("повторить шаги 6-8", "");
            step_06();
            step_07();
            step_08();
        
        }

        [Test]
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Вернуться на страницу ФинИнфо нажать на пседвоссылку Выгрузить в Excel в блоке счета", "");
            string rezult = profilePage.GoToDetalizationHistory("105");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница детальки корректна");
            }


            rezult = finansAndDetalizationPage.DownloadExportInvoiceWE(); //TODO : мб проверить файл.
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отчет скачан");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
