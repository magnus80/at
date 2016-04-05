using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class changeadress__pl : PageBase
    {

        /// <summary>
        /// Ввод названия улицы и нажатие на кнопку "искать"
        /// </summary>
        public string Street
        {
            set
            {
                new WebElement().ByXPath("//input[@name='text']").SendKeys(value);
                new WebElement().ByXPath("//input[@name='check']").Click();
            }
        }
    }
}
