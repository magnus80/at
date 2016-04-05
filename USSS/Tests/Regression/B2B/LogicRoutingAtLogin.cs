using System.IO;
using System.Net;
using System.Net.Security;
using AT;
using AT.WebDriver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Comv;
using USSS.Helpers;
using USSS.Helpers.SOAP;
using USSS.WebPages.Regression;
using USSS.WebPages.CommonPages;
using System.ServiceModel;


namespace USSS.Tests.Regression.B2B
{
    [TestFixture]
    [Category("USSS")]
    class LogicRoutingAtLogin : TestBase
    {
        static string testName = "[B2B] Логика маршрутизации при логине - B2B";

        string loginSelfreg = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginSelfreg");
        string loginAT105 = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginAT105");
        string loginAT116 = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginAT116");
        string loginCTN = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginCTN");
        string loginBAN = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginBAN");
        string loginBANbens = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginBANbens");
        string loginBENbens = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginBENbens");
        string loginBENhierarchy = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/loginBENhierarchy");
        string password = ReaderTestData.GetXMLTestData("LogicRoutingAtLogin/password");

        bool globalR = true;
        AuthorizationPage ap;
        MainPage mainPage;

        [Test]
        public void step_01()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем - SELFREG", "Логин: " + loginSelfreg + ", Пароль: " + password);
            rezult = ap.Logon(loginSelfreg, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2CPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2C");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Выйти из ЛК", "B2C");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2C");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_03()
        {
            string rezult = "";
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем AT105", "Логин: " + loginAT105 + ", Пароль: " + password);
            rezult = ap.Logon(loginAT105, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2BPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2B");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выйти из ЛК", "B2B");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2B");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_05()
        {
            string rezult = "";
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем AT116", "Логин: " + loginAT116 + ", Пароль: " + password);
            rezult = ap.Logon(loginAT116, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2CPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2C");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Выйти из ЛК", "B2C");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2C");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_07()
        {
            string rezult = "";
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем уровня CTN", "Логин: " + loginCTN + ", Пароль: " + password);
            rezult = ap.Logon(loginCTN, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2CPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2C");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Выйти из ЛК", "B2C");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2C");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_09()
        {
            string rezult = "";
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем уровня BAN", "Логин: " + loginBAN + ", Пароль: " + password);
            rezult = ap.Logon(loginBAN, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2BPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2B");
            }
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Выйти из ЛК", "B2B");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2B");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_11()
        {
            string rezult = "";
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем уровня BAN с несколькими ben", "Логин: " + loginBANbens + ", Пароль: " + password);
            rezult = ap.Logon(loginBANbens, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2BPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2B");
            }
        }

        [Test]
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Выйти из ЛК", "B2B");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2B");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_13()
        {
            string rezult = "";
            Logger.PrintStepName("Step 13");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем уровня BEN с несколькими ben", "Логин: " + loginBENbens + ", Пароль: " + password);
            rezult = ap.Logon(loginBENbens, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2CPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2C");
            }
        }

        [Test]
        public void step_14()
        {
            Logger.PrintStepName("Step 14");
            Logger.PrintAction("Выйти из ЛК", "B2C");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2C");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
        }

        [Test]
        public void step_15()
        {
            string rezult = "";
            Logger.PrintStepName("Step 15");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация пользователем уровня BEN с иерархией", "Логин: " + loginBENhierarchy + ", Пароль: " + password);
            rezult = ap.Logon(loginBENhierarchy, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }

            mainPage = new MainPage();
            rezult = mainPage.ConstructionB2BPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Cтраница имеет вид B2B");
            }
        }

        [Test]
        public void step_16()
        {
            Logger.PrintStepName("Step 16");
            Logger.PrintAction("Выйти из ЛК", "B2B");
            mainPage = new MainPage();
            string rezult = mainPage.LogoutAccount("B2B");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из ЛК");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}