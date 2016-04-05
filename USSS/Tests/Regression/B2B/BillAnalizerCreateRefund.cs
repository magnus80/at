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
    class BillAnalizerCreateRefund : TestBase
    {
        static string testName = "[B2B] Анализатор счета. Создание компенсации";

        string login = ReaderTestData.GetXMLTestData("BillAnalizerCreateRefund/login");
        string password = ReaderTestData.GetXMLTestData("BillAnalizerCreateRefund/password");
        string ban = ReaderTestData.GetXMLTestData("BillAnalizerCreateRefund/ban");
        string compensationName = "test " + DateTime.Now;
        string compensationName2 = "test2 " + DateTime.Now;
        string compensationValue = ReaderTestData.GetXMLTestData("BillAnalizerCreateRefund/value");
        string phoneNumber = ReaderTestData.GetXMLTestData("BillAnalizerCreateRefund/phone");

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        BillAnalizerPage billAnalizerPage;

        [Test]
        public void step_01()
        {
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
            //Проверка отображения страницы авторизации
            Logger.PrintAction("Проверка отображения страницы авторизации", "");
            string rezult = ap.ConstructionPage();
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
            Logger.PrintAction("Переходим к Списку компенсации", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.GoToRefundList();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Список компенсаций");
            }
            rezult = billAnalizerPage.GoToCreateRefundForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли форму создания компенсаций");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Заполняем форму создания компенсации", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.FillCompensationForm(compensationName, compensationValue, phoneNumber, "except numbers");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заполнили форму данными");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выбираем абонентов для созданной компенсации'", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.ChooseCompensation(compensationName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
            rezult = billAnalizerPage.ChooseCurrentAbonent(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Назначим компенсации выбранному абоненту", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.AssignCurrentCompensation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно назначили компенсации");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверим обработку некорректных номеров", "");
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
            Logger.PrintAction("Переходим к Списку компенсации", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.GoToRefundList();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Список компенсаций");
            }
            rezult = billAnalizerPage.GoToCreateRefundForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли форму создания компенсаций");
            }  
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Заполняем форму создания компенсации", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.FillCompensationForm(compensationName2, compensationValue, phoneNumber);
            if (rezult != "success")
            {
                if (rezult != "Некорректный номер телефона")
                {
                    globalR = false;
                    Logger.PrintRezult(false, rezult);
                }
                else 
                {
                    Logger.PrintRezult(true, "Некорретный номер телефона успешно обработан");
                }
            }
            else
            {
                Logger.PrintRezult(true, "Заполнили форму данными");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Проверяем обработку относительной компенсации", "");
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
            Logger.PrintAction("Переходим к Списку компенсации", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.GoToRefundList();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Список компенсаций");
            }
            rezult = billAnalizerPage.GoToCreateRefundForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли форму создания компенсаций");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Заполняем форму создания компенсации", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.FillCompensationForm(compensationName, compensationValue, phoneNumber, "относительная");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заполнили форму данными");
            }
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Выбираем абонентов для созданной компенсации'", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.ChooseCompensation(compensationName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
            rezult = billAnalizerPage.ChooseCurrentAbonent(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
        }

        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Назначим компенсации выбранному абоненту", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.AssignCurrentCompensation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно назначили компенсации");
            }
        }

        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Проверяем обработку обратной компенсации", "");
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
            Logger.PrintAction("Переходим к Списку компенсации", "");
            billAnalizerPage = new BillAnalizerPage();
            rezult = billAnalizerPage.GoToRefundList();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Список компенсаций");
            }
            rezult = billAnalizerPage.GoToCreateRefundForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли форму создания компенсаций");
            }
        }

        [Test]
        public void step_13()
        {
            Logger.PrintStepName("Step 13");
            Logger.PrintAction("Заполняем форму создания компенсации", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.FillCompensationForm(compensationName, compensationValue, phoneNumber, "обратная");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заполнили форму данными");
            }
        }

        [Test]
        public void step_14()
        {
            Logger.PrintStepName("Step 14");
            Logger.PrintAction("Выбираем абонентов для созданной компенсации'", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.ChooseCompensation(compensationName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
            rezult = billAnalizerPage.ChooseCurrentAbonent(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали компенсацию из списка");
            }
        }

        [Test]
        public void step_15()
        {
            Logger.PrintStepName("Step 15");
            Logger.PrintAction("Назначим компенсации выбранному абоненту", "");
            billAnalizerPage = new BillAnalizerPage();

            string rezult = billAnalizerPage.AssignCurrentCompensation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно назначили компенсации");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
