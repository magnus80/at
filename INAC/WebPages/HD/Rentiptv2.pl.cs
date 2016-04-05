using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class rentiptv2__pl : PageBase
    {
        /// <summary>
        /// Сдать в аренду IPTV приставку
        /// </summary>
        /// <param name="guid"></param>
        public void RenIptv(string guid)
        {
            new WebElement().ByXPath("//input[@name='guid_scanner']").SendKeys(guid);
            new WebElement().ByXPath("//input[@value='Сделать']").Click();
        }

        /// <summary>
        /// Принять из аренды IPTV приставку
        /// </summary>
        public void UnrentIptv()
        {
            new WebElement().ByXPath("//input[@value='Сделать']").Click();
        }
    }
}
