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
using RequestHistoryPage = USSS.WebPages.B2BPost.RequestHistoryPage;

namespace USSS.Tests.Sanity.B2BPost
{
    [TestFixture]
    [Category("Regression B2B")]
    class ChangePublicPricePlan : TestBase
    {
        static string testName = "Изменение тарифного плана для BAN с доступом только к публичным ТП";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ban = ReaderTestData.ReadExel(testName, "BAN");
        private string ctn = ReaderTestData.ReadExel(testName, "ctn");

        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        string db_Ms = ReaderTestData.ReadCExel(4, 9);
        string newTariff;
        string oldSoc;

        private AuthorizationPage ap;
        private HomePage homePage;
        private ManagerContractPage managerContractPage;
        private TariffChangePage tariffChangePage;
        private FinalTariffChange finalTariffChange;
        private RequestHistoryPage requestHistoryPage;
        private NumberProfilePage numberProfilePage;
        private bool globalR = true;

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
            string result = "";
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Открытие и проверка страницы управления контрактом","");

            result = homePage.GoToManagerContractPage();
            if(result=="success")
            {
                
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            managerContractPage = homePage.managerContractPage;
        }

        [Test]
        public void step_03()
        {
            string result="";
            oldSoc = managerContractPage.GetCurrentTariffSoc(ban, db_Ans, db_Ms);
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Выбираем абонента","");
            result = managerContractPage.ClickCheckBox();
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
                Logger.PrintAction("Нажатие на кнопку Изменить тариф","");
                result = managerContractPage.GoToTariffChange();
                if(result == "success")
                {
                    Logger.PrintRezult(true,result);
                    
                }
                else
                {
                    Logger.PrintRezult(false,result);
                    globalR = false;
                }
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            tariffChangePage = managerContractPage.tariffChange;
        }

        [Test]
        public void step_04()
        {
            string result = "";
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Проверка отображения тарифов","");
            result = tariffChangePage.CheckTariffList(ban,db_Ans,db_Ms);
            if(result=="success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                globalR = false;
                Logger.PrintRezult(false,result);
            }
            Logger.PrintAction("Выбор тарифного плана","");
            result = tariffChangePage.TariffSelect();
            newTariff = tariffChangePage.NewTariff();
            if(result == "success")
            {
                Logger.PrintRezult(true,tariffChangePage.NewTariff());
                Logger.PrintAction("Нажатие на копку Перейти на тарифный план","");
                result = tariffChangePage.ClickButtonGoTo();
                if(result == "success")
                {
                    Logger.PrintRezult(true,result);
                }
                else
                {
                    Logger.PrintRezult(false,result);
                    globalR = false;
                }
            }
            else
            {
                Logger.PrintRezult(true,result);
                globalR = false;
            }
        }

        [Test]
        public void step_05()
        {
            string result = "";
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Переход к окну выбора нотификаций","");
            
            result = tariffChangePage.GoToNotification();
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
        }

        [Test]
        public void step_06()
        {
            string result = "";
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Ввод значений в поля нотификаций и нажатие Подтвердить","");
            result = tariffChangePage.ConfirmTariff();
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            Logger.PrintAction("Перенход на страницу запросов","");
            finalTariffChange = tariffChangePage.finalTariffChange;
            result = finalTariffChange.GoToRequestPage();
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            requestHistoryPage = finalTariffChange.requestHistoryPage;
        }

        [Test]
        public void step_07()
        {
            string result = "";
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Проверка статуса запроса","");
            result = requestHistoryPage.CheckStatus();
            if(result == "Обработан")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            Logger.PrintAction("Переход на запрос","");

            result = requestHistoryPage.GoToRequest();
            if (result == "success")
            {
                Logger.PrintRezult(true, result);
            }
            else
            {
                Logger.PrintRezult(false, result);
                globalR = false;
            }
            Logger.PrintAction("Проверка отображения запроса", "");
            result = requestHistoryPage.CheckTariffChangeRequest(oldSoc, managerContractPage.GetNewTariffSoc(ban, db_Ans, db_Ms, oldSoc));
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
        }
        [Test]
        public void step_08()
        {
            string result = "";
            Logger.PrintStepName("Step 8");

            Logger.PrintAction("Закрытие стенда","");
            requestHistoryPage.Close();

            Logger.PrintAction("Открытие стенда и авторизация","");
            ap.Open();
            result = ap.ConstructionPage();
            if (result != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, result);
            }
            else
            {
                Logger.PrintRezult(true, "Страница авторизации корректна");
            }
            result = "";
            //Авторизация
            Logger.PrintAction("Авторизация", "Логин:" + login + ", Пароль: " + password);
            result = ap.Logon(login, password);
            if (result != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, result);
                ap.Close();
                Logger.PrintRezultTest(globalR);
                Assertion("Ошибка авторизации: " + result, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            Logger.PrintAction("Проверка отображения главной страницы", "");
            result = homePage.ConstructionPage();
            if (result != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, result);
            }
            else
            {
                Logger.PrintRezult(true, "Главная страница корректна");
            }
            homePage.GoToManagerContractPage();
            Logger.PrintAction("Открытие страницы Управления контрактом и проверка смены тарифа","");
            result = managerContractPage.CheckTariffChange(newTariff);
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            Logger.PrintAction("Проверка смены тарифа в бд", "");
            result = managerContractPage.CheckTariffDBChange(oldSoc, ban, db_Ans, db_Ms);
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
            }
            else
            {
                Logger.PrintRezult(false,result);
                globalR = false;
            }
            Logger.PrintRezultTest(globalR);
        }

        [Test]
        public void step_99()
        {
            string result = "";
            Logger.PrintStepName("Отмена произведенных действий");
            Logger.PrintAction("Переход на страницу профиля","");
            result = managerContractPage.GoToNumberProfile(ctn,"flag");
            Logger.PrintRezult(result == "success", result);

            numberProfilePage = managerContractPage.numberProfilePage;
            Logger.PrintAction("Отмена смены тарифного плана","");
            result = numberProfilePage.CancelChangeTariff();
            if(result == "success")
            {
                Logger.PrintRezult(true,result);
                result = numberProfilePage.GoToRequestPage();
                if(result == "success")
                {
                    result = requestHistoryPage.CheckStatus();
                    Logger.PrintRezult(result == "Обработан", result);
                }
                else
                {
                    Logger.PrintRezult(false,result);
                }
            }
            else
            {
                Logger.PrintRezult(false,result);
            }

            requestHistoryPage.Close();
        }
    }
}
