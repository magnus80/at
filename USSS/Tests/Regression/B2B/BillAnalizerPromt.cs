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
    class BillAnalizerPromt : TestBase
    {
        static string testName = "[B2B] Анализатор счетаю Подсказки";

        string login = ReaderTestData.GetXMLTestData("BillAnalizerPromt/login");
        string password = ReaderTestData.GetXMLTestData("BillAnalizerPromt/password");
        string ban = ReaderTestData.GetXMLTestData("BillAnalizerPromt/ban");
        string value = ReaderTestData.GetXMLTestData("BillAnalizerPromt/value");
        string compensationName = "test " + DateTime.Now;

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        BillAnalizerPage billAnalizerPage;

        [Test]
        public void step_01()
        {
            billAnalizerPage.ResetBillAnalyzer(ban);
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
            Logger.PrintAction("Перейти в Анализатор счета", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToBillAnalizer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Анализатор счета");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Подключаем анализатор счета", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.tryTrialPeriod();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Подключили анализатор счета");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Переходим по ссылке 'Начните с настройки иерархии'", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.GoToCreatePayoff();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли редактирование иерархии");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Перейдем на страницу создания компенсаций", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.GoToCreatePayoff();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли создание компенсаций");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Создадим компенсацию", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.CreateCompensation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли создание компенсаций");
            }
            rezult = billAnalizerPage.FillCompensationForm(compensationName, value);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заполнили форму компенсаций");
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Перейдем на страницу выбора абонентов", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.GoToChooseAbonents();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли страницу выбора абонентов");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Выберем абонентов и перейдем к назначению компенсаций", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.ChooseAbonent();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали абонентов");
            }
            rezult = billAnalizerPage.GoToRefundAssing();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли к назначению компенсаций");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Назначим компенсацию", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.AssignCompensation(compensationName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию");
            }
            rezult = billAnalizerPage.RefundAssing();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Компенсации успешно назначены");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
