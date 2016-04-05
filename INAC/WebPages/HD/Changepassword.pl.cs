using System;
using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class changepassword__pl : PageBase
    {
        #region elements


        #endregion

        #region element_value

        public string Email
        {
            set
            {
                new WebElement().ByXPath("//input[@name='emailstaff']").Click();
                new WebElement().ByXPath("//input[@name='emailstaff']").Clear();
                new WebElement().ByXPath("//input[@name='emailstaff']").SendKeys(value);
            } 
        }

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string Password
        {
            set
            {
                new WebElement().ByXPath("//input[@id='passwordPwd']").Clear();
                new WebElement().ByXPath("//input[@id='passwordPwd']").SendKeys(value);
            }
        }

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        public string PasswordConfirm
        {
            get { return new WebElement().ByXPath("//input[@name='passwordconfirm']").Text; }
            set
            {
                new WebElement().ByXPath("//input[@name='passwordconfirm']").Clear();
                new WebElement().ByXPath("//input[@name='passwordconfirm']").SendKeys(value);
            }
        }

        #endregion

        #region actions

        /// <summary>
        /// Клик по кнопке "сменить пароль"
        /// </summary>
        public void ChangepasswordClick()
        {
            new WebElement().ByXPath("//input[@name='changepassword']").Click();
        }

        /// <summary>
        /// Создание нового пароля
        /// </summary>
        public string NewPassword()
        {
            var salt = new Random().Next(1000, 9999).ToString();
            var password = "AT_PS_" + salt;
            Password = password;
            PasswordConfirm = password;
            Email = "AT_" + salt + "@beeline.ru";
            ChangepasswordClick();
            Browser.AssertDialog();

            return password;

        }


        #endregion
    }
}
