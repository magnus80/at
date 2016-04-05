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
    class ChangeTariffWithInternationalRoaming : TestBase
    {
        static string testName = "[B2B] Смена ТП с услугой международного роуминга";

        string login = ReaderTestData.GetXMLTestData("ChangeTariffWithInternationalRoaming/login");
        string password = ReaderTestData.GetXMLTestData("ChangeTariffWithInternationalRoaming/password");
        string phoneNumber = ReaderTestData.GetXMLTestData("ChangeTariffWithInternationalRoaming/phoneNumber");
        string tariff = ReaderTestData.GetXMLTestData("ChangeTariffWithInternationalRoaming/tariff");
        string ban = ReaderTestData.GetXMLTestData("ChangeTariffWithInternationalRoaming/ban");
        string requestNumber;
        string countServicesBeforeChangeTariff;

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ManagerContractPage managerContractPage;
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
            Logger.PrintAction("Перейти в Управление контрактом", "Тариф");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoManagerContracts();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Управление контрактом");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Меняем тариф", "Тариф");
            managerContractPage = new ManagerContractPage();
            countServicesBeforeChangeTariff = managerContractPage.GetCountServicesBeforeChangeTariff(ban);
            var rezult = managerContractPage.ChangeTariff(phoneNumber, tariff, "CurrentDate");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Изменили тариф");
            }
            requestNumber = managerContractPage.GetRequestNumberOfTariff();
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Открыть Историю запросов", "");
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
            rezult = requestHistoryPage.CheckLastRequest(requestNumber, "Обработан");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Запрос на смену тарифа обработан");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проверяем подключенные ранее услуги", "");
            managerContractPage = new ManagerContractPage();
            string rezult = managerContractPage.CheckServicesAfterChangeTariff(ban, countServicesBeforeChangeTariff);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Услуги остались без изменений");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}