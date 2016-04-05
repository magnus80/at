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
    class MobileBudgetStructure : TestBase
    {
        static string testName = "[B2B Мобильный бюджет] Структура";

        string login = ReaderTestData.GetXMLTestData("MobileBudgetStructure/login");
        string password = ReaderTestData.GetXMLTestData("MobileBudgetStructure/password");
        string labelName = ReaderTestData.GetXMLTestData("MobileBudgetStructure/labelName");
        string signMobileBudget = ReaderTestData.GetXMLTestData("MobileBudgetStructure/signMobileBudget");
        string subscriberNotExist = ReaderTestData.GetXMLTestData("MobileBudgetStructure/subscriberNotExist");

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        ManagerContractPage managerContractPage;

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
            Logger.PrintAction("Открыть 'Структура'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.OpenStructure();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылась 'Структура'");
            }

            rezult = navigatorPage.ConstructionStructure();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отображается колонки Договоры / Группы счетов / Пользовательские группы");
            }

            rezult = navigatorPage.CheckPanelAccounts();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "В колонке 'Договор' отображаются договор с мобильным бюджетом");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбрать чекбоксом в структуре 'Договор - плательщик'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.PressCheckBoxContractPayer();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили чекбокс в структуре 'Договор - плательщик'");
            }

            rezult = navigatorPage.CheckSelectedCheckBoxMobileBudget();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Автоматически выбрался 'Мобильный бюджет'");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Открыть раздел структуры: 'Редактирование структуры мобильного бюджета'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.OpenEditHierarchy();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открыли раздел 'Редактирование структуры мобильного бюджета'");
            }

            rezult = navigatorPage.CheckPanelGroupsSubscribers();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отображены колонки 'Группы' и 'Абоненты'");
            }

            rezult = navigatorPage.AddLabel(labelName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Создали метку");
            }

            rezult = navigatorPage.CheckButtonsItemSelected();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Присутствуют кнопки 'Редактирования' и 'Удаления'");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Нажать на кнопку 'Завершить редактирование'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.PressButtonDoneEdit();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нажали кнопку 'Завершить редактирование', открылась 'Главная станица'");
            }

            rezult = navigatorPage.OpenStructure();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылась 'Структура'");
            }

            rezult = navigatorPage.CheckSignPanelGroups(signMobileBudget);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "В структуре в группах появилась надпись " + signMobileBudget);
            }
        }
        
        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Выбрать в структуре 'Мобильный бюджет'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.SelectCheckBoxMobileBudget();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили чекбокс 'Мобильный бюджет'");
            }

            rezult = navigatorPage.CheckSignPanelGroups(labelName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Найдена группа: " + labelName);
            }

            rezult = navigatorPage.PressCheckBoxItemHierarchyGroups(labelName);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Поставили галочку в группе: " + labelName);
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Переходим в 'Управление контрактом'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoManagerContracts();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в 'Управление контрактом'");
            }

            managerContractPage = new ManagerContractPage();
            rezult = managerContractPage.CheckContainsInDataTable(new string[] { subscriberNotExist });
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отобразилась надпись 'Абонентов не найдено', так как группа пустая");
            }


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}