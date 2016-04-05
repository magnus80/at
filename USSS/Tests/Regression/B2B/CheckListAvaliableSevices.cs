﻿using System.IO;
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
    class CheckListAvaliableSevices : TestBase
    {
        static string testName = "[B2B] Проверка корректности списка доступных услуг";

        string login = ReaderTestData.GetXMLTestData("BillAnalizerOff/login");
        string password = ReaderTestData.GetXMLTestData("BillAnalizerOff/password");
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
            Logger.PrintAction("Перейти в Управление контрактом", "Подключить услуги");
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

            managerContractPage = new ManagerContractPage();
            rezult = managerContractPage.GoToServicesList();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли список всех услуг");
            }
            requestNumber = managerContractPage.CheckServicesList();

            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Список услуг корректен");
            }
        }

    }
}
