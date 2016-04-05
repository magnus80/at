using System.Collections.Generic;
using AT;
using AT.WebDriver;
using INAC;

namespace INAC.WebPages.HD
{
    public class stop_billing__pl: PageBase
    {
        #region elements 
        
        #endregion


        #region element_value

        #endregion


        #region actions

        /// <summary>
        /// Нажатие на кнопку "остановить биллинг цикл"
        /// </summary>
        public void StopBilling()
        {
           new WebElement().ByXPath("//input[@name='stop_billing']").Click();
        }

        #endregion
    }
}
