using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class find_name__pl : PageBase
    {
        #region elements

       
        #endregion

        #region element_values

        /// <summary>
        /// поле "Фамилия"
        /// </summary>
        public string Surname
        {
            get { return new WebElement().ByXPath("//input").Text; }
            set { new WebElement().ByXPath("//input").SendKeys(value); }
        }

        /// <summary>
        /// поле "Город"
        /// </summary>
        public string City
        {
            get { return new WebElement().ByXPath("//select").Text; }
            set { new WebElement().ByXPath("//select").SendKeys(value); }
        }


        #endregion

        #region actions

        /// <summary>
        /// нажатие на кнопку "Искать"
        /// </summary>
        public void Search()
        {
            new WebElement().ByXPath("//td[3]/input").Click();
        }

        #endregion

    }
}
