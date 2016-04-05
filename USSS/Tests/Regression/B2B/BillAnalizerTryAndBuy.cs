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
using System.Xml;


namespace USSS.Tests.Regression.B2B
{
    [TestFixture]
    [Category("USSS")]
    class BillAnalizerTryAndBuy : TestBase
    {
        static string testName = "[1][B2B] Анализатор счёта. Try and Buy";

        string loginBC = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/loginBC");
        string loginCRM = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/loginCRM");
        string typeBC = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/typeBC");
        string typeCRM = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/typeCRM");
        string password = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/password");
        string banBC = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/banBC");
        string banCRM = ReaderTestData.GetXMLTestData("BillAnalizerTryAndBuy/banCRM");

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        BillAnalizerPage billAnalizerPage;

        [Test]
        public void step_01()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Отключаем услугу 'Анализатор счета'", banBC);
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ResetBillAnalyzer(banBC);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                Assertion("Ошибка запроса: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Услуга 'Анализатор счета' отключена");
            }

            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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
            
            Logger.PrintAction("Авторизация", "Логин: " + loginBC + ", Пароль: " + password);
            rezult = ap.Logon(loginBC, password);
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
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Перейти в 'Анализатор счета'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToBillAnalizer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Анализатор счета'");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Нажимаем кнопку 'Подключить пробный период'", "");
            billAnalizerPage = new BillAnalizerPage();
            string rezult = billAnalizerPage.tryTrialPeriod();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Пробный период на 30 дней подключен");
            }


            ap.Close();
        }

        [Test]
        public void step_04()
        {
            string rezult = string.Empty;
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Просрачиваем услугу 'Анализатор счета'", banBC);
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ExpiredBillAnalyzer(banBC, typeBC);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                Assertion("Ошибка запроса: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Услуга 'Анализатор счета' просрочена");
            }

            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация", "Логин: " + loginBC + ", Пароль: " + password);
            rezult = ap.Logon(loginBC, password);
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
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Перейти в 'Анализатор счета'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToBillAnalizer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Анализатор счета'");
            }

            Logger.PrintAction("Подключаем 'Анализатор счета'", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ConnectBillAnalyzerService();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Создан запрос на подключение услуги 'Анализатор счета'");
            }


            ap.Close();
        }

        [Test]
        public void step_06()
        {
            string rezult = "";
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Отключаем услугу 'Анализатор счета'", banCRM);
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ResetBillAnalyzer(banCRM);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                Assertion("Ошибка запроса: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Услуга 'Анализатор счета' отключена");
            }

            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация", "Логин: " + loginCRM + ", Пароль: " + password);
            rezult = ap.Logon(loginCRM, password);
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
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Перейти в 'Анализатор счета'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToBillAnalizer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Анализатор счета'");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Нажимаем кнопку 'Подключить пробный период'", "");
            billAnalizerPage = new BillAnalizerPage();
            string rezult = billAnalizerPage.tryTrialPeriod();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Пробный период на 30 дней подключен");
            }


            ap.Close();
        }

        [Test]
        public void step_09()
        {
            string rezult = string.Empty;
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Просрачиваем услугу 'Анализатор счета'", banCRM);
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ExpiredBillAnalyzer(banCRM, typeCRM);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                Assertion("Ошибка запроса: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Услуга 'Анализатор счета' просрочена");
            }

            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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

            Logger.PrintAction("Авторизация", "Логин: " + loginCRM + ", Пароль: " + password);
            rezult = ap.Logon(loginCRM, password);
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
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Перейти в 'Анализатор счета'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToBillAnalizer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Анализатор счета'");
            }

            Logger.PrintAction("Подключаем 'Анализатор счета'", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.ConnectBillAnalyzerService();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Создан запрос на подключение услуги 'Анализатор счета'");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
