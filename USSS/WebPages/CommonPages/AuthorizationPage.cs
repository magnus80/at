using AT;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USSS.WebPages.CommonPages
{
    class AuthorizationPage : PageBase
    {
        #region constructor

        // поле ввода логина
        private  WebElement LoginWE;
        //поле ввода пароля
        private  WebElement PasswordWE;
        //кнопка "Войти"
        private  WebElement LogonWE;
        //сообщение об ошибки 
        private  WebElement ErrorWE;

        public string ConstructionPage()
        {
            LoginWE = new WebElement().ByXPath("//input[@name='loginFormB2C:loginForm:login']");
            PasswordWE = new WebElement().ByXPath("//input[@name='loginFormB2C:loginForm:passwordPwd']");
            LogonWE = new WebElement().ByXPath("//button[contains(@id,'loginFormB2C:loginForm:j_idt')]");
            ErrorWE = new WebElement().ByXPath("//div[@class='ui-messages-error ui-corner-all']//span[@class='ui-messages-error-summary']");

            if (!LoginWE.Displayed) { return "Не отображены элементы интерфейса: поле ввода логина"; }
            if (!PasswordWE.Displayed) { return "Не отображены элементы интерфейса: поле ввода пароля"; }
            if (!LogonWE.Displayed) { return "Не отображены элементы интерфейса: кнопка 'Войти'"; }
            return "success";
        }

        //Обновление сообщения об ошибке
        private void ReInitError()
        {
            ErrorWE = new WebElement().ByXPath("//div[@class='ui-messages-error ui-corner-all']//span[@class='ui-messages-error-summary']");
        }

        #endregion

        #region managerPage

        //Авторизация, введение логина и пароля, проверка сообщения об ошибке
        public string Logon(string login, string password)
        {
            try
            { 
               LoginWE.SendKeys(login);
               PasswordWE.SendKeys(password);
               LogonWE.Click();
               ReInitError();
               if (!ErrorWE.Displayed)
               {
                   return "success";
               }
               else
               {
                   return ErrorWE.Text;
               }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }
}
