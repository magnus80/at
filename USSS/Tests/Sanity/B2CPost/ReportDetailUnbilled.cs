using AT;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.B2CPost;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Sanity.B2CPost
{
    [TestFixture]
    [Category("USSS_t")]

    class ReportDetailUnbilled:TestBase
    {
        static string testName = "[SANITY][B2C] Отчёт по звонкам текущего периода postpaid";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
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
            string rezult = profilePage.GoToDetalizationHistory();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно, диаграмма присутствует");
            }
            finansAndDetalizationPage = profilePage.finansAndDetalizationPage;
            rezult = "";
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Переход на вкладку итория операция и выбрать текущий период", "");
            string rezult = finansAndDetalizationPage.GoToRequestHistoryTab();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно, таблица и график присутствует");
            }
           
            rezult = "";
        }
        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Отсортировать данные в таблице по различным фильтрам.", "");
            string rezult = finansAndDetalizationPage.CheckSortTable();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Сортировка работает корректно.");
            }

            rezult = "";
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Выгрузить данные в Excel", "");
            string rezult = finansAndDetalizationPage.DownloadUnbilledDetails();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Файл скачан");
            }

            Logger.PrintAction("Выбрать период", "");
            rezult = finansAndDetalizationPage.SelectPeriodDetails();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Период выбран страница отображена корректно");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
            rezult = "";
        }

        //CheckSortTable
    }
}
