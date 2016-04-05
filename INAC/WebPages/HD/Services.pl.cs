using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class services__pl : PageBase
    {

        public void SelectService(string s_id)
        {
            new WebElement().ByXPath("//input[@value='" + s_id + "']").Click();
            new WebElement().ByXPath("//input[@name='changeserv']").Click();
        }
    }
}
