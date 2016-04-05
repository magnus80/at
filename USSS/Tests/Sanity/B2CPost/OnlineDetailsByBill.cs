using AT;
using NUnit.Framework;
using System;
using USSS.Helpers;
using USSS.WebPages.B2CPost;
using USSS.WebPages.CommonPages;
using AT.DataBase;


namespace USSS.Tests.Sanity.B2CPost
{
    [TestFixture]
    [Category("USSS_t")]

    class OnlineDetailsByBill : TestBase
    {
        static string testName = "[SANITY][B2C] Онлайн детализация по счету B2C postpaid";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);

      
      //  string startDD = "7"; //ReaderTestData.ReadExel(testName, "StartDate DD");
      //  string startMM = "Июнь"; //ReaderTestData.ReadExel(testName, "StartDate MM");
        //string startYYYY = "2014";//ReaderTestData.ReadExel(testName, "StartDate YYYY");
        //string endDD = "6";//ReaderTestData.ReadExel(testName, "EndDate DD");
        //string endMM = "Июнь";//ReaderTestData.ReadExel(testName, "EndDate MM");
        //string endYYYY = "2015";//ReaderTestData.ReadExel(testName, "EndDate YYYY");

        private string[] MonthArr =
            {
                "","Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь",
                "Октябрь", "Ноябрь", "Декабрь"
            };

        AuthorizationPage ap;
        ProfilePage profilePage;
        FinansAndDetalizationPage finansAndDetalizationPage;

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
            
                if(Convert.ToInt32(d[0])<10)
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
           string rezult = finansAndDetalizationPage.ClickOnBill(); 
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
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Выбрать вкладку таблица", "");
            string rezult = finansAndDetalizationPage.ClickShowByTable();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Данные на вкладке по таблица отображены коррекнтно.");
            }

            rezult = "";
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Снять чекбокс для СМС и обновить отчет.", "");
            string rezult = finansAndDetalizationPage.UncheckRefreshReport();
            if (rezult != "Не найден элемент: таблицы SMS/MMS руб.")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Фильтр работает корректно.");
            }

            rezult = "";
        }
        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Перейти на вкладку история операций", "");
            string rezult = finansAndDetalizationPage.GoToHistoryOperationsTab();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Данные на вкладке корректные.");
            }

            rezult = "";
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Подвигать бегунок, изменить фильтры", "");
            string rezult = finansAndDetalizationPage.SetFiltersMoveSlider();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Данные на вкладке корректные.");
            }
            //TODO : нет проверки бегунка, потому что юзеры без таких данных.
            rezult = "";
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Проверить проставленные даты начала и конца периода.", "");
            string rezult = finansAndDetalizationPage.CheckFilterDate();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Даты в фильтрах корректные");
            }

            rezult = "";
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Вернуться в раздел фин инфо и нажать на псевдоссылку Перейти в структуру расходов", "");
            string rezult = profilePage.GoToDetalizationHistory("105");//finansAndDetalizationPage.GoTo();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            rezult = finansAndDetalizationPage.GoToChargesStructure(1); ////TODO: i - индекс нужного счета нам типа нужен оплаченный вродь, можно его конечно находить но пока так
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница структура расходов коректная.");
            }

            rezult = "";
        }

        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Выбрать вкладку таблица", "");
            string rezult = finansAndDetalizationPage.ClickShowByTable(); 
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            
            else
            {
                Logger.PrintRezult(true, "Страница  коректная.");
            }

            rezult = "";
        }
        [Test]
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Вернуться в раздел фин инфо и нажать на псевдоссылку Перейти в историю расходов", "");
            string rezult = profilePage.GoToDetalizationHistory("105");//finansAndDetalizationPage.GoTo();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            rezult = finansAndDetalizationPage.GoToChargesHistory(0); ////TODO: i - индекс нужного счета нам типа нужен оплаченный вродь, можно его конечно находить но пока так
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница история расходов коректная.");
            }

            rezult = "";
        }


        [Test]
        public void step_13()
        {
            Logger.PrintStepName("Step 13");
            Logger.PrintAction("Перейти в Финансовую информацию нажать на псевдоссылку с суммой по всем номерм данного счета", "");
            string rezult = profilePage.GoToDetalizationHistory("105");//finansAndDetalizationPage.GoTo();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            rezult = finansAndDetalizationPage.ClickOnSummByBill(1); ////TODO: i - индекс нужного счета нам типа нужен оплаченный вродь, можно его конечно находить но пока так
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отчет по расходам коректная.");
            }
            ap.Close();
            Logger.PrintRezultTest(globalR);
            rezult = "";
        }
    }
}
