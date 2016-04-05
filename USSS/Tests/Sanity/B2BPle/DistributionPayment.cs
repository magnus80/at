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
using USSS.WebPages.B2BPle;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.B2BPle
{
    [TestFixture]
    [Category("USSS")]
    internal class DistributionPayment : TestBase
    {
        private static string testName = "[B2B PLE] Распределение платежа (Sanity)";
        private string login = ReaderTestData.ReadExel(testName, "Login");
        private string password = ReaderTestData.ReadExel(testName, "Password");
        private string ban = ReaderTestData.ReadExel(testName, "ban");
        private bool globalR = true;
        private string balance;
        private DistributionPaymentPage distributionPaymentPage;
        private AuthorizationPage ap;
        private HomePage homePage;
        private ManagerContractPage managerContractPage;
        private USSS.WebPages.B2BPle.RequestHistoryPage requestHistoryPage;


       

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
            homePage.BoxContactInfoOpen();
            balance = homePage.balance;

            Logger.PrintAction("Переход в Управление контарктом'", "");
            rezult = homePage.GoToManagerContractPage();
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
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Выбор всех номеров", "");

            managerContractPage.SelectNumbers();
            Logger.PrintAction("Переход в 'Распределение баланса'", "");
            string rezult = managerContractPage.onClickDistributionPayment();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница Распределения баланса корректна");
            }
            distributionPaymentPage = managerContractPage.DistributionPaymentPage;
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("По умолчанию 'Равномерное'", "");
            Logger.PrintAction("Указываем сумму", balance);

            string rezult = distributionPaymentPage.SetSumDistribution(balance);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Сумма задана");
            }

            Logger.PrintAction("Сохранить шаблон", balance);

            rezult = distributionPaymentPage.onClickSaveTemplate();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }

            Logger.PrintAction("Нажимаем 'отменить'", "");
            rezult = distributionPaymentPage.onClickCancel();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }

            managerContractPage.onClickDistributionPayment();
            distributionPaymentPage.SetSumDistribution(balance);

            Logger.PrintAction("Нажимаем 'распределить'", "");

            rezult = distributionPaymentPage.onClickDistribution();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }
            Logger.PrintAction("Подтверждаем", "");
            rezult = distributionPaymentPage.Submit();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Распределение подтверждено");
            }


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
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Переход в 'Управление контрактами'", "");
            string rezult = homePage.GoToManagerContractPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница управления контрактом корректна");
            }
            // managerContractPage.SelectNumbers();
            Logger.PrintAction("Переход в 'Распределение баланса'", "");
            rezult = managerContractPage.onClickDistributionPayment();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница распределения корректна");
            }

            Logger.PrintAction("Выбираем 'Одна сумма на всех'", "");
            rezult = distributionPaymentPage.GoOneSumToAll();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Выбираем - одна сумма на всех");
            }

            Logger.PrintAction("Проверка страниццы распределения", "");
            rezult = distributionPaymentPage.ConstructionPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница распределения корректна");
            }

            Logger.PrintAction("Ввод максимального значения больше баланса", "");
            rezult = distributionPaymentPage.SetMaxSumDistribution();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница распределения корректна");
            }

            Logger.PrintAction("Ввод значения меньше баланса", "");
            rezult = distributionPaymentPage.SetSumDistribution("100");
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница распределения корректна");
            }

            Logger.PrintAction("Сохранить шаблон", balance);

            rezult = distributionPaymentPage.onClickSaveTemplate();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }

            Logger.PrintAction("Нажимаем 'распределить'", "");
            rezult = distributionPaymentPage.onClickDistribution();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }
            Logger.PrintAction("Подтверждаем", "");
            rezult = distributionPaymentPage.Submit();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Распределение подтверждено");
            }

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

        }

        [Test]
        public void step_05()
        {
            Logger.PrintAction("Переход в 'Управление контрактами'", "");
            string rezult = homePage.GoToManagerContractPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница управления контрактом корректна");
            }
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Переход в 'Распределение баланса'", "");
            rezult = managerContractPage.onClickDistributionPayment();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница распределения отображена корректно");
            }

            Logger.PrintAction("Нажимаем 'Различные суммы'", "");

            rezult = distributionPaymentPage.GoDifferentSum();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Различные суммы выбраны");
            }

            Logger.PrintAction("Указываем суммы", "200;200");
            List<string> sums = new List<string>();
            sums.Add("200");
            sums.Add("200");

            rezult = distributionPaymentPage.SetSumDistribution(sums);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Суммы указаны");
            }

            Logger.PrintAction("Нажимаем 'Сохранить шаблон'", "");
            rezult = distributionPaymentPage.onClickSaveTemplate();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }

            Logger.PrintAction("Нажимаем 'отменить'", "");
            rezult = distributionPaymentPage.onClickCancel();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }

            managerContractPage.onClickDistributionPayment();
            distributionPaymentPage.SetSumDistribution(balance);

            Logger.PrintAction("Нажимаем 'распределить'", "");

            rezult = distributionPaymentPage.onClickDistribution();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Кнопка нажата");
            }
            Logger.PrintAction("Подтверждаем", "");
            rezult = distributionPaymentPage.Submit();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Распределение подтверждено");
            }

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

        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            homePage.GoToManagerContractPage();
            Logger.PrintAction("Переход в 'Распределение баланса'", "");
            managerContractPage.onClickDistributionPayment();
            distributionPaymentPage.GoOneSumToAll();
            Logger.PrintAction("Указываем сумму", "15001");
            distributionPaymentPage.SetSumDistribution("15001");
            string rezult = distributionPaymentPage.ValidMassage();
            if (rezult.Contains("Сумма распределения для абонента должна находиться в пределах от 100 до 15 000 руб."))
            {
                Logger.PrintRezult(true, "Сообщение об ошибке корректно");
            }
            else
            {
                Logger.PrintRezult(false, "Сообщение об ошибке некорректно");
            }

            Logger.PrintAction("Указываем сумму", "99");
            distributionPaymentPage.SetSumDistribution("99");
            rezult = distributionPaymentPage.ValidMassage();
            if (rezult.Contains("Сумма распределения для абонента должна находиться в пределах от 100 до 15 000 руб."))
            {
                Logger.PrintRezult(true, "Сообщение об ошибке корректно");
            }
            else
            {
                Logger.PrintRezult(false, "Сообщение об ошибке некорректно");
            }
        }

        [Test]
        public void step_07()
        {
            Logger.PrintStepName("Step 7");
            Logger.PrintAction("Проверка повтора распределения", "");
            string rezult = distributionPaymentPage.RepeatDistribution();

            if (rezult == "success")
            {
                Logger.PrintRezult(true, "Повтор корректен");
            }
            else
            {
                Logger.PrintRezult(false, rezult);
            }
            ap.Close();
        }

        [Test]
        public void step_08()
        {
            var nullTran = @"UPDATE ecr9_subscriber SET Sub_Status = 'S' WHERE Customer_id = " + ban;
            Executor.ExecuteUnSelect(nullTran);

            Logger.PrintStepName("Step 8");
            Logger.PrintAction("Проверка распредения заблокированным абонентам", "");
            ap = new AuthorizationPage();
            ap.Open();
            //Проверка отображения страницы авторизации
            Logger.PrintAction("Проверка отображения страницы авторизации", "");
            ap.ConstructionPage();
            ap.Logon(login, password);
            homePage = new HomePage();
            homePage.ConstructionPage();
            homePage.GoToManagerContractPage();
            managerContractPage = homePage.managerContractPage;
            managerContractPage.SelectNumbers();
            Logger.PrintAction("Переход в 'Распределение баланса'", "");
            string rezult = managerContractPage.onClickDistributionPayment();
            if (rezult == "lookusers")
            {
                Logger.PrintRezult(true, "Предупрежедние о том что юзеры заблокированны отображено");
            }
            nullTran = @"UPDATE ecr9_subscriber SET Sub_Status = 'A' WHERE Customer_id = " + ban;
            Executor.ExecuteUnSelect(nullTran);

            Logger.PrintRezultTest(globalR);
            managerContractPage.Close();
        }
    }
}
