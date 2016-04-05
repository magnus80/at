using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class logout__pl : PageBase
    {
        #region actions

        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        public void Logout()
        {
            Open();

            /*
            Assertion("Ошибка при выходе из системы,  " + Browser.Url + " | " + Browser.Url.IndexOf("login.pl?url=").ToString(),
                                          () => Assert.IsFalse(Equals(Browser.Url.IndexOf("login.pl?url="), -1))); */

        }

        #endregion
    }
}
