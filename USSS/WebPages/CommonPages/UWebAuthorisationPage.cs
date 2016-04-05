using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT;
using AT.WebDriver;

namespace USSS.WebPages.CommonPages
{
    class UWebAuthorisationPage:PageBase
    {
        #region constructor
        // поле ввода логина
        private WebElement LoginWE;
        //поле ввода пароля
        private WebElement PasswordWE;
        //кнопка "Войти"
        private WebElement LogonWE;
        //сообщение об ошибки 
        private WebElement ErrorWE;
        
        public string ConstructionPage()
        {
            LoginWE = new WebElement().ByXPath("//input[@id='login']");
            PasswordWE = new WebElement().ByXPath("//input[@id='password-lk']");
            LogonWE = new WebElement().ByXPath("//button[contains(@id,'submitLabel')]");
            ErrorWE = new WebElement().ByXPath("//div[@class='form-tip italic']");

            if (!LoginWE.Displayed) { return "Не отображены элементы интерфейса: поле ввода логина"; }
            if (!PasswordWE.Displayed) { return "Не отображены элементы интерфейса: поле ввода пароля"; }
            if (!LogonWE.Displayed) { return "Не отображены элементы интерфейса: кнопка 'Войти'"; }
            return "success";
        }

        private void ReInitError()
        {
            ErrorWE = new WebElement().ByXPath("//div[@class='form-tip italic'");
        }

        #endregion

        #region managerPage
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
