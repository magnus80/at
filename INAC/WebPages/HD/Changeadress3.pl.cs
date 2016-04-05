using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class changeadress3__pl : PageBase
    {
        /// <summary>
        /// Ввод квартиры
        /// </summary>
        public string Flat
        {
            set
            {
                new WebElement().ByXPath("//input[@name='flat']").SendKeys(value);
            }
        }

        /// <summary>
        /// Ввод комнаты
        /// </summary>
        public string Room
        {
            set
            {
                new WebElement().ByXPath("//input[@name='room']").SendKeys(value);
            }
        }

        /// <summary>
        /// Нажатие на кнопку "перенос контракта с заявками"
        /// </summary>
        public void Write()
        {
            new WebElement().ByXPath("//input[@name='write']").Click();
        }
    }
}
