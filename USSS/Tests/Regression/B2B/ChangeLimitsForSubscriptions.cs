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
    class ChangeLimitsForSubscriptions : TestBase
    {
        static string testName = "[B2B] Подключение или изменение ограничений на подписки";

        string login = ReaderTestData.GetXMLTestData("ChangeLimitsForSubscriptions/login");
        string password = ReaderTestData.GetXMLTestData("ChangeLimitsForSubscriptions/password");
        string phoneNumber = ReaderTestData.GetXMLTestData("ChangeLimitsForSubscriptions/phoneNumber");

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
            Logger.PrintAction("Настраиваем ограничения", "Радиокнопки 1, 2, 3");
            
            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.ChangeLimitsSubscriptions(1);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Оформлена заявка 'Запрет доступа ко всем СМС-сервисам Провайдеров'");
            }

            rezult = numberProfilePage.ChangeLimitsSubscriptions(2);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Оформлена заявка 'Запрет доступа ко всем платным СМС-сервисам Провайдеров'");
            }

            rezult = numberProfilePage.ChangeLimitsSubscriptions(3);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Оформлена заявка 'Запрет доступа к СМС-сервисам Провайдеров дороже 50 руб.'");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
