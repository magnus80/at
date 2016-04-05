using AT;
using AT.Service;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.WebPages.HD
{
    public class login__pl : PageBase
    {
        private string login;

        #region element_values


        /// <summary>
        /// поле "Логин"
        /// </summary>
        public string Login
        {
            set
            {
                login = value;
                var el = new WebElement().ByXPath("//input[@name='login']");
                el.SendKeys(value); 
            
            }
        }

        /// <summary>
        /// поле "Пароль"
        /// </summary>
        public string Password
        {
            set { new WebElement().ByXPath("//input[@name='password']").SendKeys(value); }
        }

        #endregion

        #region private_methods

        private bool CheckAuth()
        {
            return (Equals(Browser.Url.Substring(Browser.Url.LastIndexOf('/') + 1), "queues.pl"));
        }

        #endregion

        #region actions

      
        /// <summary>
        /// Авторизация под пользователем с правами GOD
        /// </summary>
        /// <returns>логин пользователя, под которым авторизовались</returns>
        public string LoginAsGod()
        {
            Pages.HD.Logout.Logout();

            var list = Helpers.HD_Users.Queries.GetHdUser(1);

            login = list[0];
            var password = list[1];

            Open();
            Login = login;
            Password = password;

            Submit();
            return login;
        }

      
        /// <summary>
        /// Нажатие на кнопку "Войти" с проверкой авторизации
        /// </summary>
        public bool Submit()
        {
            new WebElement().ByXPath("//input[@value='Войти']").Click();
            if (Browser.Url.IndexOf("changepassword.pl") > -1)
            {
                Pages.HD.Changepassword.Open();
                string psw = Pages.HD.Changepassword.NewPassword();
                Open();
                Login = login;
                Password = psw;
                new WebElement().ByXPath("//input[@value='Войти']").Click();
            }

            return CheckAuth();
        }


        #endregion
    }
}
