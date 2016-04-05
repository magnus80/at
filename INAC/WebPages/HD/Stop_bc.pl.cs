using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class stop_bc__pl : PageBase
    {
        #region elements

       
        #endregion


        #region element_value

        #endregion


        #region actions

        /// <summary>
        /// Нажатие на кнопку "расторгнуть контракт"
        /// </summary>
        public void CancelContract()
        {
            new WebElement().ByXPath("//input").Click();
            Browser.AssertDialog();
        }


        #endregion

    }
}
