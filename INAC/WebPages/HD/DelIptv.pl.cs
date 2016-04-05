using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class deliptv__pl : PageBase
    {
        /// <summary>
        /// Нажатие на кнопку "Деактивировать"
        /// </summary>
        public void DeleteIptv()
        {
            new WebElement().ByXPath("//input[@value='Деактивировать']").Click();
        }
    }
}
