using AT;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.B2BPost;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Sanity.B2BPost
{
    [TestFixture]
    [Category("USSS_t")]
    public class OnlineDetalization : TestBase
    {
        static string testName = "[SANITY][B2B] Отображение онлайн детализации у пользователя B2B post";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        AuthorizationPage ap;
        HomePage homePage;
        ManagerContractPage managerContractPage;
        NumberProfilePage numberProfilePage;
        
       // USSS.WebPages.B2BPost.RequestHistoryPage requestHistoryPage;
       
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
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Переход в профиль номера'", "");
            string rezult = managerContractPage.GoToNumberProfile(phoneNumber,"CheckElements");

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
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("В блоке Детализация выбрать По Счету'", "");
            string rezult = numberProfilePage.ClickDetailsByBill();
           
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Блок детализации по счету корректен");
            }
            rezult = "";
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("В выпадающем списке выбрать расчетный период'", "");
            string rezult = numberProfilePage.SelectBillDate();

            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбор расчетного периода успешный");
            }
            rezult = "";

             rezult = numberProfilePage.CheckDetailsTableCharges();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Блок детализации по счету корректен");
            }
            rezult = "";

        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Выбрать вкладку Звонки", "");
            string rezult = numberProfilePage.ClickDetailsBilled();

            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Вкладка Звонки выбрана");
            }
            rezult = "";


            rezult = numberProfilePage.CheckDetailsTableBilled();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Блок детализации по звонкам корректен");
            }
            rezult = "";
        }

         [Test]
        public void step_07()
         {
             Logger.PrintStepName("Step 7");
             Logger.PrintAction("Нажать кнопку Детализация по счету", "");
             string rezult = numberProfilePage.ClickButtonDetailsbyBill();

             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Кнопка найдена");
             }
             rezult = "";

             rezult = numberProfilePage.CheckTableDetailsByBill();

             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Таблица корректна ");
             }
             rezult = "";

         }
         [Test]
         public void step_08()
         {
             Logger.PrintStepName("Step 8");
             Logger.PrintAction("Перейти на вкладку начисления и вернуться на вкладку звонки", "");
             string rezult = numberProfilePage.GoToChargesTab();

             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Переход на вкладку начисления успешный ");
             }
             rezult = "";
             rezult = numberProfilePage.ClickDetailsBilled();

             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Переход на вкладку Звонки успешный ");
             }
             rezult = "";

             rezult = numberProfilePage.CheckTableDetailsByBill();

             if (rezult != "success")
             {
                 globalR = false;
                 Logger.PrintRezult(false, rezult);
             }
             else
             {
                 Logger.PrintRezult(true, "Таблица корректна после перехода по вкладкам ");
             }
             rezult = "";
             ap.Close();
             Logger.PrintRezultTest(globalR);
         }

    }
}
