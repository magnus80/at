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
using o = USSS.WebPages.B2CPost;
using r = USSS.WebPages.B2CPre;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.B2CPost
{
    
    [Category("USSS")]
    public class CheckPriceTariff : TestBase
    {
        private AuthorizationPage ap;
        o.ProfilePage profilePageO;
        o.TariffsPage tariffsPageO;
        r.ProfilePage profilePageR;
        r.TariffsPage tariffsPageR;

        static string testName = "[1][B2C]Проверка отображения ненулевых стоимостей тарифов post+pre";
        string loginOF = ReaderTestData.ReadExel(testName, "LoginOF");
        string passwordOF = ReaderTestData.ReadExel(testName, "PasswordOF");
        string loginO = ReaderTestData.ReadExel(testName, "LoginO");
        string passwordO = ReaderTestData.ReadExel(testName, "PasswordO");
        string loginOS = ReaderTestData.ReadExel(testName, "LoginOS");
        string passwordOS = ReaderTestData.ReadExel(testName, "PasswordOS");
        string loginOA = ReaderTestData.ReadExel(testName, "LoginOA");
        string passwordOA = ReaderTestData.ReadExel(testName, "PasswordOA");

        string loginRF = ReaderTestData.ReadExel(testName, "LoginRF");
        string passwordRF = ReaderTestData.ReadExel(testName, "PasswordRF");
        string loginR = ReaderTestData.ReadExel(testName, "LoginR");
        string passwordR = ReaderTestData.ReadExel(testName, "PasswordR");
        string loginRS = ReaderTestData.ReadExel(testName, "LoginRS");
        string passwordRS = ReaderTestData.ReadExel(testName, "PasswordRS");
        string loginRA = ReaderTestData.ReadExel(testName, "LoginRA");
        string passwordRA = ReaderTestData.ReadExel(testName, "PasswordRA");
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
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
            Logger.PrintAction("Авторизация", "Логин:" + loginOF + ", Пароль: " + passwordOF);
            rezult = ap.Logon(loginOF, passwordOF);
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
            profilePageO = new o.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageO.ConstructionPage();
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
            string rezult = profilePageO.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageO = profilePageO.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageO.CheckTariffPrice(db_Ans, "free");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_03()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 3");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginO + ", Пароль: " + passwordO);
            rezult = ap.Logon(loginO, passwordO);
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
            profilePageO = new o.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageO.ConstructionPage();
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
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageO.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageO = profilePageO.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageO.CheckTariffPrice(db_Ans, "notfree");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_05()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 5");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginOS + ", Пароль: " + passwordOS);
            rezult = ap.Logon(loginOS, passwordOS);
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
            profilePageO = new o.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageO.ConstructionPage();
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
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageO.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageO = profilePageO.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageO.CheckTariffPrice(db_Ans, "sale");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_07()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 7");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginOA + ", Пароль: " + passwordOA);
            rezult = ap.Logon(loginOA, passwordOA);
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
            profilePageO = new o.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageO.ConstructionPage();
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
        public void step_08()
        {
            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageO.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageO = profilePageO.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageO.CheckTariffPrice(db_Ans, "saleActive");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        ///PRE

        [Test]
        public void step_09()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 9");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginRF + ", Пароль: " + passwordRF);
            rezult = ap.Logon(loginRF, passwordRF);
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
            profilePageR = new r.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageR.ConstructionPage();
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
        public void step_10()
        {
            Logger.PrintStepName("Step 10");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageR.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageR = profilePageR.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageR.CheckTariffPrice(db_Ans, "free");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_11()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 11");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginR + ", Пароль: " + passwordR);
            rezult = ap.Logon(loginR, passwordR);
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
            profilePageR = new r.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageR.ConstructionPage();
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
        public void step_12()
        {
            Logger.PrintStepName("Step 12");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageR.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageR = profilePageR.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageR.CheckTariffPrice(db_Ans, "notfree");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_13()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 13");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginRS + ", Пароль: " + passwordRS);
            rezult = ap.Logon(loginRS, passwordRS);
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
            profilePageR = new r.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageR.ConstructionPage();
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
        public void step_14()
        {
            Logger.PrintStepName("Step 14");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageR.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageR = profilePageR.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageR.CheckTariffPrice(db_Ans, "sale");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
        }

        [Test]
        public void step_15()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 15");
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
            Logger.PrintAction("Авторизация", "Логин:" + loginRA + ", Пароль: " + passwordRA);
            rezult = ap.Logon(loginRA, passwordRA);
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
            profilePageR = new r.ProfilePage();
            Logger.PrintAction("Проверка отображения профиля", "");
            rezult = profilePageR.ConstructionPage();
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
        public void step_16()
        {
            Logger.PrintStepName("Step 16");
            Logger.PrintAction("Переход на страницу тарифов", "");
            string rezult = profilePageR.GoToTariff();
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Страница отображается корректно");
            }
            tariffsPageR = profilePageR.tariffsPage;
            rezult = "";

            Logger.PrintAction("Проверка отображенных тарифов", "");
            rezult = tariffsPageR.CheckTariffPrice(db_Ans, "saleActive");
            if (rezult != "success")
            {
                Logger.PrintRezult(false, rezult);
                globalR = false;
            }
            else
            {
                Logger.PrintRezult(true, "Тарифы отображены верно");
            }

            ap.Close();
            Thread.Sleep(10000);
            Logger.PrintRezultTest(globalR);
        }
    }

}

