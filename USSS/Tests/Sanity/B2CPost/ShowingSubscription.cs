using AT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using USSS.Helpers;
using USSS.WebPages.CommonPages;
using USSS.WebPages.B2CPost;

namespace USSS.Tests.Sanity.B2CPost
{
    [TestFixture]
    [Category("USSS_t")]
    internal class ShowingSubscription : TestBase
    {
        private static string testName = "[B2C] Отображение подписок";
        private string login = ReaderTestData.ReadExel(testName, "Login");
        private string password = ReaderTestData.ReadExel(testName, "Password");
        private string ban = ReaderTestData.ReadExel(testName, "ban");
        private string temp = ReaderTestData.ReadCExel(10, 7);
        private AuthorizationPage ap;
        private ProfilePage profilePage;
        private bool globalR = true;
        private USSS.WebPages.B2CPost.RequestHistoryPage requestHistoryPage;

        //Login
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
                Logger.PrintRezult(false, rezult);
                globalR = false;
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
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            rezult = "";
            //Проверка отображения профиля
            profilePage = new ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePage.ConstructionPage();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница профиля корректна");
            }
        }

        [Test]
        public void step_02()
        {
            string rezult = "";
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Проверка отображения подписок", "");
            rezult = profilePage.CheckSubscription();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Подписки отображены корректно");
            }
            rezult = "";
        }

        [Test]
        public void step_03()
        {
            string rezult = "";
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Проверка работы параметризованной подписки", "");
            rezult = profilePage.CheckSettingSubscription();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Подписка работает корректно");
            }
            rezult = "";
        }

        [Test]
        public void step_04()
        {
            string rezult = "";
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверка отключения CPA подписки", "");
            rezult = profilePage.CheckCPAUnconnect();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Подписки отображены корректно");
            }
            rezult = "";

            Logger.PrintAction("Переход в историю запросов", "");
            rezult = profilePage.GoToRequestHistoryPage();
            requestHistoryPage = profilePage.requestHistoryPage;
            if (rezult != "success")
            {

                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории запросов корректна");
            }
            rezult = "";

            Logger.PrintAction("Проверка статуса последнего запроса", "");
            rezult = requestHistoryPage.CheckStatus(profilePage.number);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Заявка обработана");
            }
            profilePage.number = "";
        }

        [Test]
     public void step_05()
     {
         profilePage.GoToProfile();
            string rezult = "";
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проверка отключения CDP подписки", "");
            rezult = profilePage.CheckCDPUnconnect();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Подписки отображены корректно");
            }
            rezult = "";

            Logger.PrintAction("Переход в историю запросов", "");
            rezult = profilePage.GoToRequestHistoryPage();
            requestHistoryPage = profilePage.requestHistoryPage;
            if (rezult != "success")
            {

                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница истории запросов корректна");
            }
            rezult = "";

            Logger.PrintAction("Проверка статуса последнего запроса", "");
            rezult = requestHistoryPage.CheckStatus(profilePage.number);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Заявка обработана");
            }

            Logger.PrintRezultTest(globalR);
            ap.Close();
        }
    }
}

