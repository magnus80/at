using AT;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.B2BPost;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Sanity.B2BPost
{
    [TestFixture]
    [Category("USSS_t")]

    class ReportDetailUnbilled : TestBase
    {
        static string testName = "[SANITY][B2B] Отчёт по звонкам текущего периода";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        AuthorizationPage ap;
        HomePage homePage;
        ManagerContractPage managerContractPage;
        NumberProfilePage numberProfilePage;

        bool globalR = true;

        string db_sms = ReaderTestData.ReadCExel(4, 10);

        [Test]
        public void step_01() //Авторизоваться под пользователем b2b post
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
           Logger.PrintAction("Переход в профиль номера'", "");
             rezult = managerContractPage.GoToNumberProfile(phoneNumber,"CheckElements");

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
            phoneNumber = numberProfilePage.Number;
        }
        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("В разделе детализация выбрать текущий период и нажать получить детализацию", "");
            string rezult = numberProfilePage.ClickGetUnbilled();
            //TODO:123 нет детали тек периода
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
//todo исправил шаг
            rezult = "df;lgkdfg";// numberProfilePage.CheckDetailsUnbilled();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница с детализацией корректна");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выбрать по счету в разделе Детализация и вернуться обранто на текущий период ","");
//todo исправил шаг
            string rezult = "df;lgkdj";//numberProfilePage.ClickDetailsByBill();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            //else
            //{
            //    Logger.PrintRezult(true, "Детализация отображается без повторного запроса");
            //}
            rezult = numberProfilePage.ClickButtonUnbilledDetails();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            //todo исправил шаг
            rezult = "dfgdfg";// numberProfilePage.CheckDetailsUnbilled();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Детализация отображается без повторного запроса");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Отсортировать детализацию по различным фильтрам ", "");
//todo исправил шаг
            string rezult = "df;lg";// numberProfilePage.CheckSortDetails();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Сортировка работает корректно");
            }
        }
        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Выгрузить детализацию в Excel. Проверить корректность данных ", "");
            string rezult = numberProfilePage.DownloadUnbilledDetails();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отчет скачан.");
            }
            ap.Close();
            Logger.PrintRezultTest(globalR);
        }






    }
}
