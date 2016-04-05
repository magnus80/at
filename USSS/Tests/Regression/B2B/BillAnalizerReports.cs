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
    class BillAnalizerReports : TestBase
    {
        static string testName = "[B2B] Анализатор счёта. Отчёты";

        string login = ReaderTestData.GetXMLTestData("BillAnalizerReports/login");
        string password = ReaderTestData.GetXMLTestData("BillAnalizerReports/password");
        string requestNumber;

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        BillAnalizerPage billAnalizerPage;
        ReportsPage reportsPage;
        RequestHistoryPage requestHistoryPage;

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
            Logger.PrintAction("Авторизация", "Логин: " + login + ", Пароль: " + password);
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

            Logger.PrintAction("Перейти в раздел 'Анализатор счета'", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.GoToBillAnalyzerTab();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Анализатор счета'");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Нажимаем ссылку 'Конструктор отчетов'", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.GoToConstructorReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли 'Конструктор отчетов'");
            }

        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Переходим в раздел 'Отчет по предварительной стоимости'", "");
            
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли страницу 'Отчёты'");
            }

            reportsPage = new ReportsPage();
            rezult = reportsPage.GoReportPreliminaryCost();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли раздел 'Отчет по предварительной стоимости'");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Заказываем отчёт", "");
            
            reportsPage = new ReportsPage();
            string rezult = reportsPage.ToOrderReport();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылась страница заявки на создание отчета с номером запроса");
            }
            requestNumber = reportsPage.GetRequestNumberOfReport();

            navigatorPage = new NavigationPage();
            navigatorPage.GoToHistoryRequest();
            requestHistoryPage = new RequestHistoryPage();
            rezult = requestHistoryPage.CheckLastRequest(requestNumber.Substring(1), "Обработан");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заявка на отчет обработана");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
