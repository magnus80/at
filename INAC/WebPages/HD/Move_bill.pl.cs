using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class move_bill__pl: PageBase
    {
        #region elements 
        
        #endregion


        #region element_value

        public string Summ
        {
            get
            {
                return new WebElement().ByXPath("//input").Text;
            }
            set
            {
                new WebElement().ByXPath("//input").SendKeys(value);
            }
        }


        public string Tlogin
        {
            set
            {
                new WebElement().ByXPath("//tr[4]/td/input").Click();
                new WebElement().ByXPath("//tr[4]/td[2]/input").SendKeys(value);
            }
        }

        public string Comment
        {
            set
            {
                new WebElement().ByXPath("//textarea").SendKeys(value);
            }
        }

        #endregion


        #region actions

        public void MoveBill()
        {
            new WebElement().ByXPath("//tr[6]/td/input").Click();
        }

        public void Confirm()
        {
            new WebElement().ByXPath("//input[8]").Click();
        }

        #endregion
    }
}
