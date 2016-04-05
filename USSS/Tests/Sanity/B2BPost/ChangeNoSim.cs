using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.WebPages.B2BPost;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.B2BPost
{
    [TestFixture]
    [Category("USSS")]
    class ChangeNoSim: TestBase
    {
        static string testName = "[B2B] Замена sim (сценарий с некорректным вводом номера sim)(Sanity)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        bool globalR = true;

        AuthorizationPage ap;
        HomePage homePage;
        ManagerContractPage managerContractPage;
        NumberProfilePage numberProfilePage;
        USSS.WebPages.B2BPost.RequestHistoryPage requestHistoryPage;

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
                Logger.PrintRezultTest(globalR);
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            rezult = "";
            //Проверка отображения профиля
            homePage = new HomePage();
            Logger.PrintAction("Проверка отображения главной страницы", "");
            rezult = homePage.ConstructionPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Главная страница корректна");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Переход в Управление контарктом'", "");
            string rezult = homePage.GoToManagerContractPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница управление контрактом корректна");
            }
            managerContractPage = homePage.managerContractPage;
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Переход в профиль номера'", "");
            string rezult = managerContractPage.GoToNumberProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница профиля корректна");
            }
            rezult = "";
            numberProfilePage = managerContractPage.numberProfilePage;
        }

        [Test]
        public void step_04()
        {
            
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Нажатие кнопку Заменить SIM", "");
            string rezult = numberProfilePage.GoToChangeSim();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата, форма замены сим отображена");
            }
            rezult = "";

        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Ввод сим меньше 11 символов", "");
            string rezult = numberProfilePage.ChangeSim("1111");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Номер введен и подтвержен");
            }
            rezult = "";
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Ожидаение сообщения об ошибке", "");
            string rezult = numberProfilePage.ReadErrorMassage();
            if (rezult != "success")
            {
                
                Logger.PrintRezult(true, rezult);
            }
            else
            {
                globalR = false;
                Logger.PrintRezult(false, "Сообщение об ошибки не возникло");
            }
            rezult = "";
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Ввод сим 11 символов, только цифры, номер некорректен", "");
            string rezult = numberProfilePage.ChangeSim("11111111111");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Номер введен и подтвержен");
            }
            rezult = "";
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Подтверждение нотификации", "");
            string rezult = numberProfilePage.ConfirmNotif();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Нотификаци подтверждена");
            }
            rezult = "";

            Logger.PrintAction("Переход в историю заявок", "");
            homePage.ConstructionPage();
            rezult = homePage.GoToRequestHistoryPage();
            requestHistoryPage = homePage.requestHistoryPage;
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории запросов корректна");
            }
            rezult = "";

            Logger.PrintAction("Проверка статуса последнего запроса", "");
            rezult = requestHistoryPage.CheckStatus();
            if (rezult != "Отклонен")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заявка отклоненна");
            }
            rezult = "";

            rezult = requestHistoryPage.GetDetails();
            if (rezult != "Замена сим карты для номера " + phoneNumber + ". Произошла ошибка: SIM-карта не загружена в систему")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Комментарий корректен");
            }
        }

        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 9");
            Logger.PrintAction("Переход в Управление контарктом'", "");
            homePage.ConstructionPage();
            string rezult = homePage.GoToManagerContractPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница управление контрактом корректна");
            }
            managerContractPage = homePage.managerContractPage;
            rezult = "";

            Logger.PrintAction("Переход в профиль номера'", "");
            rezult = managerContractPage.GoToNumberProfile(phoneNumber);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница профиля корректна");
            }
            rezult = "";
            numberProfilePage = managerContractPage.numberProfilePage;

           
            Logger.PrintAction("Нажатие кнопку Заменить SIM", "");
            rezult = numberProfilePage.GoToChangeSim();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата, форма замены сим отображена");
            }
            rezult = "";

            Logger.PrintAction("Ввод сим c не цифровыми символами", "");
            rezult = numberProfilePage.ChangeSim("1111abc$");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Номер введен и подтвержен");
            }
            rezult = "";

            Logger.PrintAction("Проверка введеного номера сим", "");
            rezult = numberProfilePage.CheckChangeSim();
            if (rezult != "success")
            {
                Logger.PrintRezult(true, "Буквенный номер сим введен не был");
            }
            else
            {
                globalR = false;
                Logger.PrintRezult(false, "Номер введен");
            }
            rezult = "";

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }

    }
}
