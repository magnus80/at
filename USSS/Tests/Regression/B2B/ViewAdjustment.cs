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
    class ViewAdjustment : TestBase
    {
        static string testName = "[B2B] Просмотр в биллинге ручных корректировок";

        string login = ReaderTestData.GetXMLTestData("ViewAdjustment/login");
        string password = ReaderTestData.GetXMLTestData("ViewAdjustment/password");
        string ban = ReaderTestData.GetXMLTestData("ViewAdjustment/ban");

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        FinancialInformationPage financialinformationPage;

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
            Logger.PrintAction("Перейти в Финансовую информацию", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFinanсialInformation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Финансовой информации");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбрать вкладку ручные корректировки", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.GoToAdjustment();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылись ручные корректировки счета");
            }
            Logger.PrintAction("Проверить соотвествие корректировок запросом", "");
            rezult = financialinformationPage.CheckAdjustment(ban);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ручные корректировки соотвествуют запросу");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
