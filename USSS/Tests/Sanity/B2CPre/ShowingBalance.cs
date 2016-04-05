using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.Helpers.SOAP;
using USSS.WebPages.B2CPre;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Sanity.B2CPre
{
    [TestFixture]
    [Category("USSS")]
    class ShowingBalance: TestBase
    {
        private AuthorizationPage ap;
        ProfilePage profilePage;


        static string testName = "[B2C] Отображение баланса у пользователя B2C prepaid";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        private string balance;

        //Login
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

            rezult = "";
            //Проверка отображения баланса
            Logger.PrintAction("Проверка отображения баланса", "");
            rezult = profilePage.CheckBalance();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Баланс отображен");
                balance = profilePage.balance;
            }
        }


        [Test]
        public void step_02()
        {
            string rezult = "";
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Изменение баланса на 20р", "");
            ChangeBalance cb = new ChangeBalance();
            cb.SetBalance("7" + phoneNumber, "20");
            rezult = "";
            //Проверка отображения баланса
            Logger.PrintAction("Проверка отображения баланса", "");
            rezult = profilePage.CheckBalance();
            double b = Convert.ToDouble(balance.Replace(" руб.", "")) + 20;
            double bn = Convert.ToDouble(profilePage.balance.Replace(" руб.", ""));
            if (rezult != "success" || b != bn)
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Баланс отображен корректно");
            }
            Logger.PrintRezultTest(globalR);
            ap.Close();
        }
    }
}
