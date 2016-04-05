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
    class ConnectionClosedGroupByNumber  : TestBase
    {
        static string testName = "[B2B] Подключение закрытой группы на номер";

        string login = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/login");
        string password = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/password");
        string phoneNumber = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/phoneNumber");
        string tariff = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/tariff");
        string ban = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/ban");
        string subscriberNumber = ReaderTestData.GetXMLTestData("ConnectionClosedGroupByNumber/subscriberNumber");
        string requestNumber;

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ManagerContractPage managerContractPage;
        RequestHistoryPage requestHistoryPage;
        NumberProfilePage numberProfilePage;

        [Test]
        public void step_01()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
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
            rezult = "";
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
            Logger.PrintAction("Перейти в Управление контрактом", "");
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
            Logger.PrintAction("Меняем тариф", tariff);
            managerContractPage = new ManagerContractPage();
            var rezult = managerContractPage.ChangeTariff(phoneNumber, tariff);
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
            Logger.PrintAction("Проверяем наличие закрытой группы", "");
            managerContractPage = new ManagerContractPage();
            string rezult = managerContractPage.CheckClosedGroupExist(ban, subscriberNumber, tariff);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Закрытая группа присутствует");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Отменяем изменения тарифа", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoManagerContracts();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Управление контрактом'");
            }

            managerContractPage = new ManagerContractPage();
            rezult = managerContractPage.GoToNumberProfile(subscriberNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в профиль абонента");
            }

            numberProfilePage = new NumberProfilePage();
            rezult = numberProfilePage.CancelChangeTariff();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отменили изменение тарифа");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}