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


namespace USSS.Tests.Regression.B2B
{
    [TestFixture]
    [Category("USSS")]
    class FormBlankAutofilling : TestBase
    {
        static string testName = "[B2B] Форма для автоматического заполнения бланка заявления";

        string name = ReaderTestData.GetXMLTestData("FormBlankAutofilling/name");
        string inn = ReaderTestData.GetXMLTestData("FormBlankAutofilling/inn");
        string address = ReaderTestData.GetXMLTestData("FormBlankAutofilling/address");
        string phone = ReaderTestData.GetXMLTestData("FormBlankAutofilling/phone");
        string email = ReaderTestData.GetXMLTestData("FormBlankAutofilling/email");
        string contract = ReaderTestData.GetXMLTestData("FormBlankAutofilling/contract");
        string person = ReaderTestData.GetXMLTestData("FormBlankAutofilling/person");

        bool globalR = true;
        AuthorizationPage ap;


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
            Logger.PrintAction("Переходим на страницу логина для юридических лиц", "");
            rezult = ap.GoToLegalEntity();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при переходе на страницу заполнения заявления: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Успешно перешли на страницу заявления");
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            Logger.PrintAction("Заполняем заявление", "");
            HelpPage helpPage = new HelpPage();
            helpPage.OpenBlankForm();
            string rezult = helpPage.ConstructionPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ошибка при открытии формы");
            }
            Logger.PrintAction("Проверяем обязательные поля формы","");
            helpPage.PressSendButton();
            rezult = helpPage.CheckWarningMessage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Assertion("Ошибка при проверки обязательных полей формы: " + rezult, Assert.Fail);
            }
            else
            {
                Logger.PrintRezult(true, "Обязательные поля проверены успешно");
            }
        }

        [Test]
        public void step_03()
        {
            string[] data = new string[]{name,inn,address,phone,email,contract,person};
            Logger.PrintStepName("Step 3");
            Logger.PrintAction("Заполняем форму и жмем Распечатать", "");
            HelpPage helpPage = new HelpPage();
            string rezult = helpPage.FillForm(data);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ошибка при вводе данных в форму");
            }
            helpPage.PressSendButton();
            rezult = helpPage.CheckPrintForm(data);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Ошибка при проверке печатной формы");
            }

            ap.Close();
            Logger.PrintRezultTest(globalR);
        }
    }
}
