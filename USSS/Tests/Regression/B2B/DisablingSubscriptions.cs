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
    class DisablingSubscriptions : TestBase
    {
        static string testName = "[B2B] Подключение или изменение ограничений на подписки";

        string login = ReaderTestData.GetXMLTestData("DisablingSubscriptions/login");
        string password = ReaderTestData.GetXMLTestData("DisablingSubscriptions/password");
        string phoneNumber = ReaderTestData.GetXMLTestData("DisablingSubscriptions/phoneNumber");

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ManagerContractPage managerContractPage;
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
            Logger.PrintAction("Перейти в профиль абонента", phoneNumber);
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
            rezult = managerContractPage.GoToNumberProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в профиль абонента " + phoneNumber);
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Нажимаем кнопку 'Показать мои подписки'", "");
            numberProfilePage = new NumberProfilePage();

            string rezult = numberProfilePage.OpenMySubscriptions();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылиcь подписки");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Нажать 'Отключить' напротив подписки", "CPA");

            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.ClickDisableCPASubscription();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Система отображает всплывающее окно подтверждения отключения подписки");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Нажать 'Отключить' в окне подтверждения", "");

            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.ClickDisableButton();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нажали по кнопке 'Отключить'");
            }

            rezult = numberProfilePage.CheckRequestDelSubsr();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найден запрос на отключение CPA подписки");
            }


            ap.Close();
        }

        [Test]
        public void step_06()
        {
            string rezult = "";
            Logger.PrintStepName("Step 6");
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
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Перейти в профиль абонента", phoneNumber);
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
            rezult = managerContractPage.GoToNumberProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в профиль абонента " + phoneNumber);
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Нажимаем кнопку 'Показать мои подписки'", "");
            numberProfilePage = new NumberProfilePage();

            string rezult = numberProfilePage.OpenMySubscriptions();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылиcь подписки");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Нажать 'Отключить' напротив подписки", "CDP");
            
            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.ClickDisableCDPSubscription();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Система отображает всплывающее окно подтверждения отключения подписки");
            }
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Нажать 'Отключить' в окне подтверждения", "");

            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.ClickDisableButton();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нажали по кнопке 'Отключить'");
            }

            rezult = numberProfilePage.CheckRequestDelSubsr();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найден запрос на отключение CDP подписки");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
