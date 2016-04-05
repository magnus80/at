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
    class BillAnalyzerReportDesigner : TestBase
    {
        static string testName = "[1][B2B] Анализатор счета и Конструктор отчетов. Раздельный функционал";

        string loginAdmin = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/loginAdmin");
        string passwordAdmin = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/passwordAdmin");
        string login = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/login");
        string loginBEN = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/loginBEN");
        string password = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/password");
        string role = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/role");
        string checkboxBillAnalyzer = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/checkboxBillAnalyzer");
        string checkboxEditableCustomReports = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/checkboxEditableCustomReports");
        string blockNameBillAnalyzer = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/blockNameBillAnalyzer");
        string blockNameEditableCustomReports = ReaderTestData.GetXMLTestData("BillAnalyzerReportDesigner/blockNameEditableCustomReports");

        
        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ReportsPage reportsPage;
        RolesManagePage rolesManagePage;

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
            Logger.PrintAction("Авторизация под администратором", "Логин: " + loginAdmin + ", Пароль: " + passwordAdmin);
            rezult = ap.Logon(loginAdmin, passwordAdmin);
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
            Logger.PrintAction("Перейти на страницу 'Управление ролями'", "");
            navigatorPage = new NavigationPage();
            var rezult = navigatorPage.GoToRoleManage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница 'Управление ролями' открылась");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открываем роль изменения", "Роль: " + role);
            rolesManagePage = new RolesManagePage();
            var rezult = rolesManagePage.OpenChangeRole(role);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли роль");
            }

            Logger.PrintAction("Кликаем по псевдоссылке", "Детальная настройка полномочий");
            rezult = rolesManagePage.ClickPseudoLinkDetailedSettingsAuthority();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кликнули по 'Детальная настройка полномочий'");
            }

            Logger.PrintAction("Ставим галочку в чекбоксе", checkboxBillAnalyzer);
            rezult = rolesManagePage.SelectCheckBox(checkboxBillAnalyzer);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили галочку в чекбоксе: " + checkboxBillAnalyzer);
            }

            Logger.PrintAction("Ставим галочку в чекбоксе", checkboxEditableCustomReports);
            rezult = rolesManagePage.SelectCheckBox(checkboxEditableCustomReports);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили галочку в чекбоксе: " + checkboxEditableCustomReports);
            }

            Logger.PrintAction("Жмём 'Сохранить роль'", "кнопка");
            rezult = rolesManagePage.SaveRole();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Изменения в роле сохранены");
            }

            Logger.PrintAction("Выходим из системы", "кнопка разлогивания");
            navigatorPage = new NavigationPage();
            rezult = navigatorPage.DeAuthorization();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из системы");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Авторизация под пользователем B2B", "Логин: " + login + ", Пароль: " + password);
            ap = new AuthorizationPage();
            var rezult = ap.Logon(login, password);
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
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Переходим в раздел 'Отчёты'", "");
            navigatorPage = new NavigationPage();
            var rezult = navigatorPage.GoReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Отчёты'");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверям отображение блока", blockNameBillAnalyzer);
            reportsPage = new ReportsPage();
            var rezult = reportsPage.CheckBlockExist(blockNameBillAnalyzer);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отобразился блок: " + blockNameBillAnalyzer);
            }

            Logger.PrintAction("Проверям отображение блока", blockNameEditableCustomReports);
            reportsPage = new ReportsPage();
            rezult = reportsPage.CheckBlockExist(blockNameEditableCustomReports);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отобразился блок: " + blockNameEditableCustomReports);
            }

            Logger.PrintAction("Выходим из системы", "кнопка разлогивания");
            navigatorPage = new NavigationPage();
            rezult = navigatorPage.DeAuthorization();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из системы");
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Авторизация под администратором ", "Логин: " + loginAdmin + ", Пароль: " + passwordAdmin);
            ap = new AuthorizationPage();
            ap.Close(); ap.Open();
            var rezult = ap.Logon(loginAdmin, passwordAdmin);
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
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Перейти на страницу 'Управление ролями'", "");
            navigatorPage = new NavigationPage();
            var rezult = navigatorPage.GoToRoleManage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница 'Управление ролями' открылась");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Открываем роль изменения", "Роль: " + role);
            rolesManagePage = new RolesManagePage();
            var rezult = rolesManagePage.OpenChangeRole(role);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли роль");
            }

            Logger.PrintAction("Кликаем по псевдоссылке", "Детальная настройка полномочий");
            rezult = rolesManagePage.ClickPseudoLinkDetailedSettingsAuthority();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кликнули по 'Детальная настройка полномочий'");
            }

            Logger.PrintAction("Снимаем галочку с чекбокса", checkboxBillAnalyzer);
            rezult = rolesManagePage.UnSelectCheckBox(checkboxBillAnalyzer);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Сняли галочку с чекбокса: " + checkboxBillAnalyzer);
            }

            Logger.PrintAction("Снимаем галочку с чекбокса", checkboxEditableCustomReports);
            rezult = rolesManagePage.UnSelectCheckBox(checkboxEditableCustomReports);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Сняли галочку с чекбокса: " + checkboxEditableCustomReports);
            }

            Logger.PrintAction("Жмём 'Сохранить роль'", "кнопка");
            rezult = rolesManagePage.SaveRole();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Изменения в роле сохранены");
            }

            Logger.PrintAction("Выходим из системы", "кнопка разлогивания");
            navigatorPage = new NavigationPage();
            rezult = navigatorPage.DeAuthorization();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из системы");
            }
        }

        [Test]
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Авторизация под пользователем B2B", "Логин: " + login + ", Пароль: " + password);
            ap = new AuthorizationPage();
            var rezult = ap.Logon(login, password);
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
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Переходим в раздел 'Отчёты'", "");
            navigatorPage = new NavigationPage();
            var rezult = navigatorPage.GoReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Отчёты'");
            }
        }

        [Test]
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Проверям отсутствие блока", blockNameBillAnalyzer);
            reportsPage = new ReportsPage();
            var rezult = reportsPage.CheckBlockNotExist(blockNameBillAnalyzer);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отсутствует блок: " + blockNameBillAnalyzer);
            }

            Logger.PrintAction("Проверям отсутствие блока", blockNameEditableCustomReports);
            reportsPage = new ReportsPage();
            rezult = reportsPage.CheckBlockNotExist(blockNameEditableCustomReports);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отсутствует блок: " + blockNameEditableCustomReports);
            }

            Logger.PrintAction("Выходим из системы", "кнопка разлогивания");
            navigatorPage = new NavigationPage();
            rezult = navigatorPage.DeAuthorization();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вышли из системы");
            }


            ap.Close();
        }

        [Test]
        public void step_13()
        {
            Logger.PrintStepName("Step 13");
            Logger.PrintAction("Авторизация под пользователем B2B уровня BEN", "Логин: " + loginBEN + ", Пароль: " + password);
            ap = new AuthorizationPage();
            ap.Open();
            var rezult = ap.Logon(loginBEN, password);
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

            navigatorPage = new NavigationPage();
            rezult = navigatorPage.GoReports();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Отчёты'");
            }

            reportsPage = new ReportsPage();
            rezult = reportsPage.CheckBlockNotExist(blockNameBillAnalyzer);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отсутствует блок: " + blockNameBillAnalyzer);
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}