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
    class DisplayDeposits : TestBase
    {
        static string testName = "[B2B] Отображение депозитов";

        string login = ReaderTestData.GetXMLTestData("DisplayDeposits/login");
        string password = ReaderTestData.GetXMLTestData("DisplayDeposits/password");
        string ban = ReaderTestData.GetXMLTestData("DisplayDeposits/ban");

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
            Logger.PrintAction("Перейти в Финансовая информация", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFinanсialInformation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Финансовая информация");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открыть вкладку 'Гарантийные взносы", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.ClickGuaranteeFeesTab();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладка 'Гарантийные взносы' открылась");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверяем значения оплаченных гарантийных взносов", "");
            financialinformationPage = new FinancialInformationPage();

            string rezult = financialinformationPage.CheckPaidFee(ban);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Количество оплаченных гарантийных взносов совпадает");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проверяем колличество неоплаченных гарантийных взносов", "");
            financialinformationPage = new FinancialInformationPage();

            string rezult = financialinformationPage.CheckUnpaidFee(ban);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Количество неоплаченных гарантийных взносов совпадает");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
