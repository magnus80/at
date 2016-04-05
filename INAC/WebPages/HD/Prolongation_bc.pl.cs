using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class prolongation_bc__pl : PageBase
    {

        #region elements

      
        #endregion

        #region element_value

        /// <summary>
        /// Количество дней 
        /// </summary>
        public string ShiftDays
        {
            get { return new WebElement().ByXPath("//input").Text; }
            set { new WebElement().ByXPath("//input").SendKeys(value); }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment
        {
            get { return new WebElement().ByXPath("//input").Text; }
            set { new WebElement().ByXPath("//input").SendKeys(value); }
        }

        #endregion

        #region actions

        /// <summary>
        /// Нажатие на кнопку "Проблить Бц"
        /// </summary>
        public void SetProlongation()
        {
            new WebElement().ByXPath("//p/input[2]").Click();
        }

        #endregion
    }
}
