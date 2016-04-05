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
    class ChooseBlockType : TestBase
    {
        static string testName = "[B2B] Возможность выбора варианта блокировки";

        string loginAdmin = ReaderTestData.GetXMLTestData("ChooseBlockType/loginAdmin");
        string passwordAdmin = ReaderTestData.GetXMLTestData("ChooseBlockType/passwordAdmin");
        string userRole = ReaderTestData.GetXMLTestData("ChooseBlockType/userRole");
        string login = ReaderTestData.GetXMLTestData("ChooseBlockType/login");
        string password = ReaderTestData.GetXMLTestData("ChooseBlockType/password");
        string ban = ReaderTestData.GetXMLTestData("ChooseBlockType/ban");
        string phoneNumber = ReaderTestData.GetXMLTestData("ChooseBlockType/phoneNumber");
        string requestNumber;

        string checkboxExtendBlock = "Доступ к функционалу выбора типа блокировки абонента через ЛК."; 

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ManagerContractPage managerContractPage;
        RequestHistoryPage requestHistoryPage;
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
            Logger.PrintAction("Открываем роль изменения", "Роль: " + userRole);
            rolesManagePage = new RolesManagePage();
            var rezult = rolesManagePage.OpenChangeRole(userRole);
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

            Logger.PrintAction("Ставим галочку в чекбоксе", checkboxExtendBlock);
            rezult = rolesManagePage.SelectCheckBox(checkboxExtendBlock);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили галочку в чекбоксе: " + checkboxExtendBlock);
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
            Logger.PrintAction("Переходим в раздел 'Управление контрактом'", "");
            navigatorPage = new NavigationPage();
            var rezult = navigatorPage.GoManagerContracts();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Управление контрактом'");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Переходим на страницу выбранного абонента ", phoneNumber);
            managerContractPage = new ManagerContractPage();
            var rezult = "";
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в раздел 'Управление контрактом'");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
