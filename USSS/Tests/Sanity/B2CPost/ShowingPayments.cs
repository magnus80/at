using AT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.CommonPages;
using USSS.WebPages.B2CPost;

namespace USSS.Tests.Sanity.B2CPost
{
    [TestFixture]
    [Category("USSS_t")]
    internal class ShowingPayments : TestBase
    {
        private static string testName = "[B2C] Отображение платежей у пользователя B2C postpaid";
        private string login = ReaderTestData.ReadExel(testName, "Login");
        private string password = ReaderTestData.ReadExel(testName, "Password");
        private string ban = ReaderTestData.ReadExel(testName, "ban");
        private string temp = ReaderTestData.ReadCExel(10, 7);
        private AuthorizationPage ap;
        private ProfilePage profilePage;
        private bool globalR = true;
        private FinansAndDetalizationPage finansAndDetalizationPage;
        string db_Ans = ReaderTestData.ReadCExel(4, 7);

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
            string rezult = "";
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Проверка отображения платежей", "");
            rezult = finansAndDetalizationPage.CheckPayments(0);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Платеж корректен");
            }
            rezult = finansAndDetalizationPage.CheckPayments(1);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            rezult = finansAndDetalizationPage.CheckPayments(2);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            rezult = finansAndDetalizationPage.CheckPayments(3);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }

            rezult = finansAndDetalizationPage.CheckPayments(db_Ans, ban);
                 if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            finansAndDetalizationPage = profilePage.finansAndDetalizationPage;
            rezult = "";
        }

        [Test]
        public void step_04()
        {
            string rezult = "";
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверка отображения фильтров по умолчанию", "");
            rezult = finansAndDetalizationPage.CheckDefaulFilter(db_Ans);
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
            Logger.PrintAction("Выбор фильтров с отсутсвием результатов", "");
            rezult = finansAndDetalizationPage.CheckUnRezultFilter();
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
        public void step_06()
        {
            string rezult = "";
            Logger.PrintStepName("Step 6");
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
        public void step_07()
        {
            string rezult = "";
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Проверка кнопки Все платежи", "");
            rezult = finansAndDetalizationPage.ClickAllPayments(db_Ans);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Работа кнопки корректна");
            }
        }

        [Test]
        public void step_08()
        {
            string rezult = "";
            Logger.PrintStepName("Step 8");
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
