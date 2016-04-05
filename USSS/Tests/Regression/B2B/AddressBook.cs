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
    class AddressBook : TestBase
    {
        static string testName = "[B2B] Адресная книга";

        string login = ReaderTestData.GetXMLTestData("AddressBook/login");
        string password = ReaderTestData.GetXMLTestData("AddressBook/password");
        string numberSubscriber = ReaderTestData.GetXMLTestData("AddressBook/numberSubscriber");
        string lastName = ReaderTestData.GetXMLTestData("AddressBook/lastName");
        string firstName = ReaderTestData.GetXMLTestData("AddressBook/firstName");
        string email = ReaderTestData.GetXMLTestData("AddressBook/email");

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
            Logger.PrintAction("Перейти в Управление контрактом", "");
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

            Logger.PrintAction("Выбираем абонента", numberSubscriber);
            managerContractPage = new ManagerContractPage();
            rezult = managerContractPage.GoToNumberProfile(numberSubscriber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка перехода в профиль: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Абонент выбран");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Открываем форму для заполнения данных о владельце номера", "");
            numberProfilePage = new NumberProfilePage();
            var rezult = numberProfilePage.OpenToSubscriberForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Форма открылась");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Нажимаем кнопку ОК", "не заполняя форму");
            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.CheckErrorOfReqFields();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ошибки на форме отображены");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Заполняем обязательные поля и жмём ОК", 
                string.Format("Фамилия: {0}, Имя: {1}, email: {2}", lastName, firstName, email));
            numberProfilePage = new NumberProfilePage();
            string rezult = numberProfilePage.CheckMessageOfContactDataChanged(lastName, firstName, email);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, 
                    "Появилось сообщение 'Контактные данные были успешно изменены'");
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Переходим в 'Управление контрактом'",
                string.Format("Фамилия: {0}, Имя: {1}, email: {2}", lastName, firstName, email));
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
            rezult = managerContractPage.CheckContainsInDataTable(new string[] { lastName, firstName, email });
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "У абонента измененились данные");
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Переходим в профиль абонента", numberSubscriber);
            managerContractPage = new ManagerContractPage();
            string rezult = managerContractPage.GoToNumberProfile(numberSubscriber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка перехода в профиль: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли в профиль абонента");
            }

            numberProfilePage = new NumberProfilePage();
            rezult = numberProfilePage.CheckContainsDataInNumberProfile(new string[] { lastName, firstName, email });
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "У абонента измененились данные");
            }
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Открываем форму для заполнения данных о владельце номера", "");
            numberProfilePage = new NumberProfilePage();
            var rezult = numberProfilePage.OpenToSubscriberForm();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Форма открылась");
            }

            Logger.PrintAction("Заполняем поля: адрес, комментарий и документ", "более 500 символов");
            numberProfilePage = new NumberProfilePage();
            rezult = numberProfilePage.CheckErrorOfMoreCharacters();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true,
                    "Система выдаст сообщение о недопустимости ввода более 500 символов");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Выбрать в верхней части меню существующий контакт", "");
            //numberProfilePage = new NumberProfilePage();
            //var rezult = numberProfilePage.SelectExistContactOnForm(lastName);
            //if (rezult != "success")
            //{
            //    globalR = false;
            //    Logger.PrintRezult(false, rezult);
            //}
            //else
            //{
            //    Logger.PrintRezult(true, "Появилось поле для ввода существующего контакта");
            //}


            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
