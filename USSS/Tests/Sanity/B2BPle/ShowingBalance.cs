using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using USSS.Helpers;
using USSS.WebPages.B2BPle;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Sanity.B2BPle
{
    [TestFixture]
    [Category("USSS")]
    class ShowingBalance: TestBase
    {
        static string testName = "[B2B] Отображение баланса у пользователя B2B prepaid(Sanity)";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");
        string ban = ReaderTestData.ReadExel(testName, "ban");
        bool globalR = true;
        string scpUrl = ReaderTestData.ReadCExel(1, 10);
        AuthorizationPage ap;
        HomePage homePage;
        ContractPage contractPage;
        private double balance;

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
            rezult = "";
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Открытие Договоров", "");
            string rezult = homePage.BoxContactInfoOpen();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Таблица договоров отображена корректно");
            }

            Logger.PrintAction("Открытие Договора", "");
            rezult = homePage.GoToContract();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                contractPage = homePage.contractPage;
                Logger.PrintRezult(true, "Страница договора корректна");
            }
            ap.Close();
        }

        [Test]
        public void step_03()
        {
            Thread.Sleep(5000);
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Сравнение суммы с Системой корпоративной предоплаты", "");
            SKPPage skp = new SKPPage();
            Thread t = new Thread(Aut);

            skp.Open(scpUrl);
            t.Start();

            string rezult = skp.GetBalance(ban);
            if (skp.balance.Replace(" руб.", "") != homePage.balance.Replace(" ", "") || !skp.balance.Replace(" руб.", "").Contains(contractPage.balance.Replace("На балансе Вашего договора ", "").Replace(" руб.", "")))
            {
                globalR = false;
                Logger.PrintRezult(false, "Балансы некорректен");
            }
            else
            {
                Logger.PrintRezult(true, "Баланс корректен");
            }
            balance = Convert.ToDouble(homePage.balance.Replace(" ", ""));

            skp.AddPayment("20");
            skp.Close();
            t.Abort();
            GC.Collect();
            Logger.PrintAction("Сравнение суммы с Системой корпоративной предоплаты после платежа", "");
            ap = new AuthorizationPage();
            ap.Open();
            //Проверка отображения страницы авторизации
            Logger.PrintAction("Проверка отображения страницы авторизации", "");
            ap.ConstructionPage();
            ap.Logon(login, password);
            homePage = new HomePage();
            homePage.ConstructionPage();
            homePage.BoxContactInfoOpen();
            if (balance + 20 != Convert.ToDouble(homePage.balance.Replace(" ", "")))
            {
                Logger.PrintRezult(false, "Баланс не корректен");
            }
            else
            {
                Logger.PrintRezult(true, "Баланс корректен");
            }
            Logger.PrintRezultTest(globalR);
            ap.Close();
        }

        private static void Aut()
        {
            Thread.Sleep(5000);
            SendKeys.SendWait("KIryshkov");
            SendKeys.SendWait("{TAB}");
            SendKeys.SendWait("Xtndthu26");
            SendKeys.SendWait("{ENTER}");
        }
    }
}
