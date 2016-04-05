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
    class CompareBillsB2B : TestBase
    {
        static string testName = "[B2B] Сравнение счетов B2B";

        string login = ReaderTestData.GetXMLTestData("CompareBillsB2B/login");
        string password = ReaderTestData.GetXMLTestData("CompareBillsB2B/password");
        int totalNumbers = Convert.ToInt32(ReaderTestData.GetXMLTestData("CompareBillsB2B/billCompareList/totalNumbers"));

        private List<string> billCompareList()
        {
            List<string> compareList = new List<string>();
            for (int number = 1; number <= totalNumbers; number++)
            {
                compareList.Add(ReaderTestData.GetXMLTestData("CompareBillsB2B/billCompareList/n" + number));
            }

            return compareList;
        }

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        FinancialInformationPage financialinformationPage;

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
            Logger.PrintAction("Перейти в 'Финансовая информация'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFinanсialInformation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Финансовая информация'");
            }

            financialinformationPage = new FinancialInformationPage();
            rezult = financialinformationPage.CheckPaymentTabAll();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладки отображены");
            }

        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открыть вкладку 'Все счета'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.ClickAllBillsTab();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладка 'Все счета' открылась");
            }

            rezult = financialinformationPage.CheckBills(2);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Необходимое количество счетов отображено");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выбираем 1 счет", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.SelectBill(1);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали 1 счёт");
            }

            rezult = financialinformationPage.CheckButtonCompareBills(1);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка 'Сравнение счетов' задизейблена");
            }

            rezult = financialinformationPage.CheckButtonReset();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка 'Сброс' не задизейблена");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Выбрать 2 счет и нажать кнопку 'Сравнение счетов'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.SelectBill(2);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбрали 2 счёт");
            }

            rezult = financialinformationPage.CheckButtonCompareBills();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка 'Сравнение счетов' не задизейблена");
            }


            rezult = financialinformationPage.PressButtonCompareBills();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нажали кнопку 'Сравнение счетов' открылось Сравнение счетов");
            }

            rezult = financialinformationPage.CheckDefaultTab();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "По умолчанию выбрана вкладка 'По периодам'");
            }

            rezult = financialinformationPage.CheckCompareElements();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Основные элементы отображены");
            }

            Logger.PrintAction("Проверяем список параметров для сравнения", "");
            for (int i = 0; i <= totalNumbers - 1; i++)
            {
                rezult = financialinformationPage.CheckListCompareParameters(billCompareList()[i]);
                if (rezult != "success")
                {
                    globalR = false;
                    Logger.PrintRezult(false, rezult);
                }
                else
                {
                    Logger.PrintRezult(true, "Найден параметр: " + billCompareList()[i]);
                }
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверяем график в категории 'Общее'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.CheckHighcharts("Overall");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найден график в категории 'Общее'");
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Проверяем график в категории 'По периодам'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.CheckHighcharts("ByPeriods");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найден график в категории 'По периодам'");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Проверяем график в категории 'По группам счетов'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.CheckHighcharts("ByBillGroups");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найден график в категории 'По группам счетов'");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Нажимаем кнопку 'Завершить'", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.PressButtonComplete();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нажали кнопку 'Завершить' и перешли в раздел 'Финансовая информация'");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
