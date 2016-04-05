using AT;
using AT.DataBase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.WebPages.B2BPost;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.B2BPost
{
    [TestFixture]
    [Category("USSS")]
    class ChangeSim: TestBase
    {
        static string testName = "[B2B] Замена SIM(Sanity)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string phoneNumber = ReaderTestData.ReadExel(testName, "phoneNumber");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Usss = ReaderTestData.ReadCExel(4, 8);
        AuthorizationPage ap;
        HomePage homePage;
        ManagerContractPage managerContractPage;
        NumberProfilePage numberProfilePage;
        USSS.WebPages.B2BPost.RequestHistoryPage requestHistoryPage;
        bool globalR = true;
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
            phoneNumber = numberProfilePage.Number;
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

        string sim;

        [Test]
        public void step_05()
        {
            var query = @"select serial_no from serial_item_inv@" + db_Ans + " where primary_ctn is null and ngp=1 and resource_status='AA'";
            var simQ = Executor.ExecuteSelect(query);
            sim = simQ[0,0];
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Ввод корректного номера sim", "");
            string rezult = numberProfilePage.ChangeSim(sim.Remove(0, 7));
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
            Thread.Sleep(10000);
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
            if (rezult != "Обработан")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Заявка обработана");
            }
            Logger.PrintAction("Проверка проверка комментария заявки", "");
            rezult = requestHistoryPage.GetDetails();
            if (rezult != "Замена сим карты для номера "+ phoneNumber +".")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Комментарий корректен");
            }
            try
            {
                var query = @"select SERIAL_NO from serial_item_inv@'" + db_Ans + "' where primary_ctn='" + phoneNumber + "'";
                var simQ = Executor.ExecuteSelect(query);
                string newSim = simQ[0, 0];
                if (sim != newSim)
                {
                    globalR = false;
                    Logger.PrintRezult(false, "Sim не сменен");
                }
                else
                {
                    Logger.PrintRezult(true, "Номер SIM изменен");
                }
            }
            catch (Exception)
            {
                
              
            }
            
            Logger.PrintRezultTest(globalR);
            ap.Close();
        }
    }
}
