using System.IO;
using System.Net;
using System.Net.Security;
using AT;
using AT.WebDriver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Comv;
using USSS.Helpers;
using USSS.Helpers.SOAP;
using USSS.WebPages.Regression;
using USSS.WebPages.CommonPages;
using System.ServiceModel;
using System.Xml;


namespace USSS.Tests.Regression.B2B
{
    [TestFixture]
    [Category("USSS")]
    class CreateClaimToCRMClarify : TestBase
    {
        static string testName = "[B2B] Создание обращения в CRM Clarify под пользователем B2B";

        string login = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/login");
        string password = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/password");
        string textClaim = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/textClaim");
        string errorMessage = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/errorMessage");
        string requestType = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/requestType");
        string requestStatus = ReaderTestData.GetXMLTestData("CreateClaimToCRMClarify/requestStatus");

        string requestNumber;

        bool globalR = true;
        AuthorizationPage ap;
        NavigationPage navigatorPage;
        FeedBackPage feedbackPage;


        [Test]
        public void step_01()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();
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
            Logger.PrintAction("Авторизация", "Логин: " + login + ", Пароль: " + password);
            rezult = ap.Logon(login, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка авторизации: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Перейти в 'Обратная связь'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFeedBack();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Обратная связь'");
            }
        }

        [Test]
        public void step_03()
        {
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Нажимаем кнопку 'Новое обращение'", "");
            feedbackPage = new FeedBackPage();

            string rezult = feedbackPage.ClickNewClaim();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Открылся раздел отправки нового обращения");
            }

            rezult = feedbackPage.ConstructionCreateClaim();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Отображаются эелементы: 'Договор', 'Тип обращения', 'Текст запроса', 'Отправить'");
            }
        }

        [Test]
        public void step_04()
        {
            Logger.PrintStepName("Step 4");
            Logger.PrintAction("Нажимаем кнопку 'Отправить'", "не заполняя ничего");
            feedbackPage = new FeedBackPage();
            string rezult = feedbackPage.CheckErrorWithEmptyText(errorMessage);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Появилось сообщение 'Введите текст обращения в текстовое поле'");
            }
        }

        [Test]
        public void step_05()
        {
            Logger.PrintStepName("Step 5");
            Logger.PrintAction("Заполняем и отправляем обращение", textClaim);
            feedbackPage = new FeedBackPage();
            string rezult = feedbackPage.EnterClaim(textClaim);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Обращение отправлено");
            }
            requestNumber = feedbackPage.GetRequestNumber();
            
            rezult = feedbackPage.CheckClaimCRMClarify(requestNumber, requestType, login, requestStatus);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, 
                    string.Format("В базе появилось обращение {0} c типом '{1}', от пользователя '{2}', в статусе '{3}'",
                    requestNumber, requestType, login, requestStatus));
            }
        }

        [Test]
        public void step_06()
        {
            Logger.PrintStepName("Step 6");
            Logger.PrintAction("Перейти в 'Обратная связь'", "");
            navigatorPage = new NavigationPage();
            string rezult = navigatorPage.GoToFeedBack();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Перешли на страницу 'Обратная связь'");
            }

            feedbackPage = new FeedBackPage();
            rezult = feedbackPage.CheckTableClaim(textClaim);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "В таблице найдено обращение");
            }
            

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
