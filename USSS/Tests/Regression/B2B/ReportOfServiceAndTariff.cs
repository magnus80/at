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
    class ReportOfServiceAndTariff : TestBase
    {
        static string testName = "[B2B] Отчет по услугам и тарифным планам";

        string login = ReaderTestData.GetXMLTestData("ReportOfServiceAndTariff/login");
        string password = ReaderTestData.GetXMLTestData("ReportOfServiceAndTariff/password");
        string numberSubscriber = ReaderTestData.GetXMLTestData("ReportOfServiceAndTariff/numberSubscriber");
        string email = ReaderTestData.GetXMLTestData("ReportOfServiceAndTariff/email");
        string requestNumber;


        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ReportsPage reportsPage;
        RequestHistoryPage requestHistoryPage;
        //ReaderMail readerMail;

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
            Logger.PrintAction("Перейти в раздел 'Отчеты' -> 'Отчёт по услугам и тарифным планам'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Отчеты'");
            }

            reportsPage = new ReportsPage();
            rezult = reportsPage.GoReportServicesAndTariffPlans();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Отчет по услугам и тарифным планам'");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбрать все услуги и все тарифные планы, нажать 'Заказать отчет'", "");
            reportsPage = new ReportsPage();
            var rezult = reportsPage.SelectAllServicesAndTariffPlans();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Появилась форма для ввода нотификаций");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Ввводим емейл и номер телефона на который придет нотификация и нажать 'Подтвердить'",
                string.Format("Email: {0}; Номер телефона: {1};", email, numberSubscriber));
            reportsPage = new ReportsPage();
            string rezult = reportsPage.ApproveEmailAndPhoneNumber(email, numberSubscriber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                requestNumber = reportsPage.GetRequestNumberOfReport();
                Logger.PrintRezult(true, "Запрос создан " + requestNumber);
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Перейти в раздел 'История запросов'", requestNumber);
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToHistoryRequest();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница 'История запросов открылась'");
            }

            requestHistoryPage = new RequestHistoryPage();
            rezult = requestHistoryPage.CheckLastRequest(requestNumber.Substring(1), "Обработан");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Запрос обработан");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверка нотификации по смс и email",
                string.Format("Номер телефона: {0}, Запрос: {1}; Email: {2}", numberSubscriber, requestNumber, email));
            reportsPage = new ReportsPage();
            string rezult = reportsPage.CheckSMS(numberSubscriber, requestNumber.Substring(1));
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Смс получена");
            }

            //readerMail = new ReaderMail();
            //readerMail.ReadLastMail();
            //rezult = "success";
            //if (rezult != "success")
            //{
            //    globalR = false;
            //    Logger.PrintRezult(false, rezult);
            //}
            //else
            //{
            //    Logger.PrintRezult(true, "Email получено");
            //}


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}