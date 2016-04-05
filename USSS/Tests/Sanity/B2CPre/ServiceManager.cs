using System.Threading;
using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.WebPages.B2CPre;
using USSS.WebPages.CommonPages;
using AT.DataBase;

namespace USSS.Tests.B2CPre
{
    [TestFixture]
    [Category("USSS_t")]
    public class ServiceManager : TestBase ///[1][B2C] Управление услугами B2C pre(Sanity)
    {
        static string testName = "[1][B2C] Управление услугами B2C pre(Sanity)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ban = ReaderTestData.ReadExel(testName, "ban");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        private string nameNewService = null;
        private AuthorizationPage ap;
        ProfilePage profilePage;
        ServicesPage servicesPage;
        USSS.WebPages.B2CPre.RequestHistoryPage requestHistoryPage;
        private string currentTariff;
        string category = ReaderTestData.ReadExel(testName, "category");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string soc = ReaderTestData.ReadExel(testName, "soc");
        bool globalR = true;
        private string number;
        string db_sms = ReaderTestData.ReadCExel(4, 10);

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
            currentTariff = profilePage.GetCurrentTariff(db_Ans, phoneNumber);
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Переход на страницу услуг", "");
            string rezult = profilePage.GoToService();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            servicesPage = profilePage.servicesPage;
            rezult = "";

            rezult = servicesPage.CheckFiltr();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Фильтр по умолчанию корректен");
            }
            rezult = "";
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Переход к доступным услугам", "");
            string rezult = servicesPage.GoToAvailableSevices(ban, db_Ans, currentTariff, db_Ms);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }

            rezult = "";
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Выбор категории", "");
            string rezult = servicesPage.ClickCategory(ban, db_Ans, currentTariff, db_Ms, category);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Услуги отображены корректно");
            }
           
            rezult = "";
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Выбор услуги", "");
            string rezult = servicesPage.ConnectOfService(ref soc);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Выбор осуществлен");
            }

            rezult = "";
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверка отображения карточки услуги и подключение", "");
            string rezult = servicesPage.SuccessConnectOfService(ref nameNewService, ref number, db_Ans);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отображение корректно, заявка сформирована");
            }

            rezult = "";
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Переход в историю запросов", "");
            string rezult = profilePage.GoToRequestHistoryPage();
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
            rezult = requestHistoryPage.CheckStatus(number);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {

                Logger.PrintRezult(true, "Заявка обработана");
                // Заявка №2147677422 от 13.05.2015 13:21 на подключение пакета услуг GPRS-пакет в международном роуминге 100 Мб для абонента 9030339107 обработана. Изменения вступят в силу 06.04.2015. https://my.beeline.ru
                var q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + number + "'";
                var smsB = Executor.ExecuteSelect(q);
                if (smsB.Count != 0)
                {
                    string sms = smsB[0, 0];
                    if (sms.Contains("Заявка") & sms.Contains("на подключение") & sms.Contains("для абонента") & sms.Contains(phoneNumber) &
                        sms.Contains("обработана. Изменения вступят в силу") & sms.Contains(" https://my.beeline.ru"))
                    {
                        Logger.PrintRezult(true, "СМС корректна");
                    }
                    else
                    {
                        Logger.PrintRezult(false, "СМС некорректна");
                        globalR = false;
                    }
                }
                else
                {
                    Logger.PrintRezult(false, "СМС отсутствует");
                    globalR = false;
                }
            }
            rezult = "";
           
        }

        [Test]
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Переход на страницу услуг", "");
            string rezult = profilePage.GoToService();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            servicesPage = profilePage.servicesPage;
            rezult = "";

           
            Logger.PrintAction("Переход к подключенным услугам", "");
            rezult = servicesPage.GoTConnectedServices();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            rezult = "";

            Logger.PrintAction("Проверка подключенных услуг", "");
            rezult = servicesPage.CheckConnectedServices(ban, db_Ans, db_Ms, phoneNumber);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Услуги отображены корректно");
            }

            rezult = "";
        }


        [Test]
        public void step_09()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Выбор услуги", "");
            string rezult = servicesPage.DisconnectOfService(null);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Выбор осуществлен");
            }

            rezult = "";
        }

        [Test]
        public void step_10()
        {
            nameNewService = null;
            number = null;
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Проверка отображения карточки услуги и отключение", "");
            string rezult = servicesPage.SuccessDisconnectOfService(ref nameNewService, ref number, db_Ans);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отображение корректно, заявка сформирована");
            }

            rezult = "";
        }

        [Test]
        public void step_11()
        {
            Logger.PrintStepName("Step 11");
            Logger.PrintAction("Переход в историю запросов", "");
            string rezult = profilePage.GoToRequestHistoryPage();
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
            rezult = requestHistoryPage.CheckStatus(number);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Заявка обработана");

                //  Заявка №2147677588 от 13.05.2015 17:58 на отключение услуги Счастливое время для абонента 9030335210 обработана.Изменения вступят в силу 06.04.2015. https://my.beeline.ru"
                var q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + number + "'";
                var smsB = Executor.ExecuteSelect(q);
                if (smsB.Count != 0)
                {
                    string sms = smsB[0, 0];
                    if (sms.Contains("Заявка") & sms.Contains("на отключение") & sms.Contains("для абонента") & sms.Contains(phoneNumber) &
                        sms.Contains("обработана.Изменения вступят в силу") & sms.Contains(" https://my.beeline.ru"))
                    {
                        Logger.PrintRezult(true, "СМС корректна");
                    }
                    else
                    {
                        Logger.PrintRezult(false, "СМС некорректна");
                        globalR = false;
                    }
                }
                else
                {
                    Logger.PrintRezult(false, "СМС отсутствует");
                    globalR = false;
                }
            }
            rezult = "";
            

            Logger.PrintRezultTest(globalR);
            ap.Close();
        }
    }
}
