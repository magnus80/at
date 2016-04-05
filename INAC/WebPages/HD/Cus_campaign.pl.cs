using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class cus_campaign__pl : PageBase
    {
        #region elements


        #endregion

        #region actions

        public bool FindCus(string cus)
        {/*
            foreach (var a in Browser.FindElements(OpenQA.Selenium.By.TagName("a")))
            {
                if (a.Text.Equals(cus))
                {
                    return true;
                }
            }*/
            return false;

        }

        #endregion 



    }
}
