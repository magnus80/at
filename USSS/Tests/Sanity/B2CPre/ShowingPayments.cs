using System.Globalization;
using AT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.CommonPages;
using USSS.WebPages.B2CPre;
using AT.DataBase;

namespace USSS.Tests.Sanity.B2CPre
{
    [TestFixture]
    [Category("USSS_t")]
    internal class ShowingPayments : TestBase
    {
        private static string testName = "[B2C] Отображение платежей у пользователя B2C pre";
        private string login = ReaderTestData.ReadExel(testName, "Login");
        private string password = ReaderTestData.ReadExel(testName, "Password");
        private AuthorizationPage ap;
        private ProfilePage profilePage;
        private bool globalR = true;
        private FinansAndDetalizationPage finansAndDetalizationPage;
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        private string hostBP = ReaderTestData.ReadCExel(9, 7);
        private string portBP = ReaderTestData.ReadCExel(9, 8);
        private string sidBP = ReaderTestData.ReadCExel(9, 9);
        private string userBP = ReaderTestData.ReadCExel(9, 10);
        private string passwordBP = ReaderTestData.ReadCExel(9, 11);
        private string temp = ReaderTestData.ReadCExel(10, 7);
        string startDD = ReaderTestData.ReadExel(testName, "StartDate DD");
        string startMM = ReaderTestData.ReadExel(testName, "StartDate MM");
        string startYYYY = ReaderTestData.ReadExel(testName, "StartDate YYYY");
        string endDD = ReaderTestData.ReadExel(testName, "EndDate DD");
        string endMM = ReaderTestData.ReadExel(testName, "EndDate MM");
        string endYYYY = ReaderTestData.ReadExel(testName, "EndDate YYYY");
        string startDDO = ReaderTestData.ReadExel(testName, "StartDateO DD");
        string startMMO = ReaderTestData.ReadExel(testName, "StartDateO MM");
        string startYYYYO = ReaderTestData.ReadExel(testName, "StartDateO YYYY");
        string endDDO = ReaderTestData.ReadExel(testName, "EndDateO DD");
        string endMMO = ReaderTestData.ReadExel(testName, "EndDateO MM");
        string endYYYYO = ReaderTestData.ReadExel(testName, "EndDateO YYYY");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        //Login
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
            string rezult = "";
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Проверка отображения страницы финансовой информации", "");
            rezult = profilePage.GoToFinanceAndDetalization();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница финансовой информации корректна");
            }
            finansAndDetalizationPage = profilePage.finansAndDetalizationPage;
            rezult = "";
        }

        [Test]
        public void step_03()
        {
            finansAndDetalizationPage.SelectPeriod("За другие дни");
            finansAndDetalizationPage.SetPeriod(startDD, startMM, startYYYY, endDD, endMM, endYYYY);

            string rezult = "";
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Проверка отображения фильтров по умолчанию", "");
            rezult = finansAndDetalizationPage.CheckDefaulFilter();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Фильтр корректен");
            }
        }

        [Test]
        public void step_04()
        {
            string rezult = "";
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверка работы фильтра по типу", "");//Бывший пустой фильтр
            rezult = finansAndDetalizationPage.CheckFilter();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Фильтр корректен");
            }
        }

        [Test]
        public void step_05()
        {
            string rezult = "";
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проверка валидации фильтра по дате", "");
            rezult = finansAndDetalizationPage.CheckDateFilterValid();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Валидация фильтра по дате корректна");
            }
        }

        [Test]
        public void step_06()
        {
            string rezult = "";
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверка отображение платежей", "");
            finansAndDetalizationPage.SetPeriod(startDD, startMM, startYYYY, endDD, endMM, endYYYY);
            rezult = finansAndDetalizationPage.CheckPayment(phoneNumber, hostBP, portBP, sidBP, userBP, passwordBP, startDD, startMM, startYYYY, endDD, endMM, endYYYY);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Платежи отображены верно");
            }
        }

        [Test]
        public void step_07()
        {
            string rezult = "";
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Проверка кнопки выгрузки", "");

            rezult = finansAndDetalizationPage.ClickExportPayments(temp);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Файл загружен");
            }

            Logger.PrintRezultTest(globalR);
            ap.Close();
        }
    }
}
