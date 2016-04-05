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
    class UnlockUser : TestBase
    {
        static string testName = "[B2B] Разблокировка абонента B2B";

        string login = ReaderTestData.GetXMLTestData("BlockUsers/login");
        string password = ReaderTestData.GetXMLTestData("BlockUsers/password");
        string ban = ReaderTestData.GetXMLTestData("BlockUsers/ban");
        string phoneNumber = ReaderTestData.GetXMLTestData("BlockUsers/phoneNumber");
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
            managerContractPage.BlockNumberInDataBase(ban, phoneNumber);
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

            rezult = managerContractPage.CheckAbonentString(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при выборе абонента: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Абонент выбран");
            }
            Logger.PrintAction("Разблокируем абонента", phoneNumber);
            rezult = managerContractPage.UnlockSelectedAbonent();
            if (rezult == "error")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                requestNumber = rezult;
                Logger.PrintRezult(true, "Абонент разблокирован");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Перейти в историю запросов", "");
            navigatorPage = new NavigationPage();
            navigatorPage.GoToHistoryRequest();
            requestHistoryPage = new RequestHistoryPage();
            string rezult = requestHistoryPage.CheckLastRequest(requestNumber, "Обработан");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Искомый запрос обработан успешно");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
