using System.Threading;
using AT;
using AT.DataBase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.WebPages.B2CPre;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.B2CPre
{
    [TestFixture]
    [Category("USSS")]
    public class ChangeOfTariffPlan : TestBase ///[1][B2C] Смена тарифного плана B2C prepaid (Sanity)
    {
        private string nameNewTariff = null;
        private AuthorizationPage ap;
        ProfilePage profilePage;
        TariffsPage tariffsPage;
        USSS.WebPages.B2CPre.RequestHistoryPage requestHistoryPage;

        static string testName = "[1][B2C] Смена тарифного плана B2C prepaid (Sanity)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string loginB = ReaderTestData.ReadExel(testName, "LoginB");
        string passwordB = ReaderTestData.ReadExel(testName, "PasswordB");
        string ban = ReaderTestData.ReadExel(testName, "ban");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        string db_sms = ReaderTestData.ReadCExel(4, 10);
        string nameTariff = ReaderTestData.ReadExel(testName, "soc");
        string number;

        //Login
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
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult =  profilePage.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPage = profilePage.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPage.CheckTariffList(ban, db_Ans, db_Ms);
            if (rezult != "success")    
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

        }
       
        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Смена тарифа", "");
            string rezult = tariffsPage.ChangeOfTariff(nameTariff);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
              
            }
            else
            {
                Logger.PrintRezult(true, "Выбор осуществлен");
            }
          
            rezult = tariffsPage.SuccessChangeOfTariff(ref nameNewTariff, ref number, ban, db_Ans);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Смена подтверждена");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
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

            Logger.PrintAction("Проверка статуса запроса", "");
            rezult = requestHistoryPage.CheckStatus(number);
            if (rezult != "Обработан")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Заявка обработана");
            }
            var q = @"SELECT a.msg_body FROM " + db_sms + ".sms_submit a where a.request_id = '" + number + "'";
            var smsB = Executor.ExecuteSelect(q);
            if (smsB.Count != 0)
            {

                string sms = smsB[0, 0];
                if (sms.Contains("Запрос") & sms.Contains("на изменение тарифного плана") &
                    sms.Contains("успешно обработан."))
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
                Logger.PrintRezult(false, "СМС не создана");
                globalR = false;
            }
            var nullTran = @"INSERT INTO CSM_TRANSACTIONS@"+ db_Ans+ @" (TRX_SEQ_NO,ACTV_CODE,BAN,SUBSCRIBER_NO,SYS_CREATION_DATE)
                                                  values(csm_transactions_1sq.nextval@" + db_Ans + @",'CIW',1,'0000000000', sysdate)";
            Executor.ExecuteUnSelect(nullTran);
            rezult = "";
        }

        [Test]
        public void step_05()
        {
            ap.Close();
            Thread.Sleep(10000);
            ap.Open();
            ap.ConstructionPage();
            ap.Logon(login, password);
            
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Проверка работы е2е", "");
            profilePage.GoToTariff();
            string rezult = tariffsPage.ViewNewTariff(nameNewTariff);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Отображение корректно");
            }
            rezult = "";
            rezult = tariffsPage.CheckRezult(ban, db_Ans);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Перенос услуг корректен");
            }
        }

        [Test]
        public void step_6()
        {
            ap.Close();

            ap.Open();
            ap.ConstructionPage();
            ap.Logon(loginB, passwordB);

            Logger.PrintStepName("Step 6");
           
            profilePage.GoToTariff();
            Logger.PrintAction("Смена тарифа c недостаточным балансом", "");
            string rezult = tariffsPage.ChangeOfTariff(null);
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Выбор осуществлен");
            }
            rezult = tariffsPage.ReadMassageTariff(); 
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Подключение не доступно");
            }

            Logger.PrintRezultTest(globalR);
            ap.Close();
        }

    }
}

