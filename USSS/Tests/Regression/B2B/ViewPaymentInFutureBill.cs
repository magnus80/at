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
    class ViewPaymentInFutureBill : TestBase
    {
        static string testName = "[B2B] Просмотр начислений в будующий счёт через ЛК";

        string login = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/login");
        string password = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/password");
        string operationData = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/operationData");
        string phoneNumber = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/phoneNumber");
        string serviceCode = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/serviceCode");
        string subscriberFee = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/subscriberFee");
        string serviceActivationCost = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/serviceActivationCost");
        string vat = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/vat");
        string comment = ReaderTestData.GetXMLTestData("ViewPaymentInFutureBill/comment");
        
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
            Logger.PrintAction("Перейти в Финансовая информация", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFinanсialInformation();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу Финансовая информация");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открыть вкладку 'Начисления в будущий счет", "");
            financialinformationPage = new FinancialInformationPage();
            string rezult = financialinformationPage.GoToPaymentInFutureBill();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладка 'Начисления в будущий счет' открылась");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверяем значения счета будущих начислений", "");
            financialinformationPage = new FinancialInformationPage();

            string rezult = financialinformationPage.CheckPaymentInFutureBill(new string[] { operationData, phoneNumber, serviceCode, subscriberFee, serviceActivationCost, vat, comment });
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Значения счета будущих начислений присутствуют");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
