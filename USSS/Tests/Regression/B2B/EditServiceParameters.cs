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
    class EditServiceParameters : TestBase
    {
        static string testName = "[B2B] Редактирование параметров услуг";

        string login = ReaderTestData.GetXMLTestData("EditServiceParameters/login");
        string password = ReaderTestData.GetXMLTestData("EditServiceParameters/password");
        string ban = ReaderTestData.GetXMLTestData("EditServiceParameters/ban");
        string phoneNumber = ReaderTestData.GetXMLTestData("EditServiceParameters/phoneNumber");
        string serviceName = ReaderTestData.GetXMLTestData("EditServiceParameters/service");
        string country1 = ReaderTestData.GetXMLTestData("EditServiceParameters/country1");
        string country2 = ReaderTestData.GetXMLTestData("EditServiceParameters/country2");
        string requestNumber;

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
            Logger.PrintAction("Перейти в Управление контрактом", "");
            managerContractPage = new ManagerContractPage();
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

            Logger.PrintAction("Выбираем абонента", phoneNumber);
            rezult = managerContractPage.GoToAbonentProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при переходе к странице абонента: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу абонента");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Переходим в редактирование услуги", "");
            managerContractPage = new ManagerContractPage();
            string rezult = managerContractPage.GoToServiceEdit(serviceName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при переходе к редактированию услуги: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно открыли редактирование услуги");
            }

            Logger.PrintAction("Проверяем выбранную страну ", country1);
            rezult = managerContractPage.CheckServiceCounrty(country1);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Выбранная страна не соотвествует данным: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Выбранная страна соотвествует данным");
            }

            Logger.PrintAction("Меняем выбранную страну на новую ", country2);
            
            requestNumber = managerContractPage.ChangeCountry(country2);
            if (requestNumber != "error")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при сохранении изменений услуги: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно сохранили изменение услуги, запрос " + requestNumber);
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Перейти в Историю запросов", "");
            navigatorPage = new NavigationPage();
            navigatorPage.GoToHistoryRequest();
            requestHistoryPage = new RequestHistoryPage();
            string rezult = requestHistoryPage.CheckLastRequest(requestNumber, "Обработан");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при обработке запроса: " + requestNumber, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Искомый запрос обработан успешно");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Перейти в Управление контрактом", "");
            managerContractPage = new ManagerContractPage();
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

            Logger.PrintAction("Выбираем абонента", phoneNumber);
            rezult = managerContractPage.GoToAbonentProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при переходе к странице абонента: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу абонента");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Переходим в редактирование услуги", "");
            managerContractPage = new ManagerContractPage();
            string rezult = managerContractPage.GoToServiceEdit(serviceName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при переходе к редактированию услуги: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно открыли редактирование услуги");
            }

            Logger.PrintAction("Проверяем выбранную страну ", country2);
            rezult = managerContractPage.CheckServiceCounrty(country1);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Выбранная страна не соотвествует данным: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Выбранная страна соотвествует данным");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }

    }
}