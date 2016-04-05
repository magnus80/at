using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class changeiptv__pl : PageBase
    {
        public void ChangeIptv(string base_id, List<string> packets)
        {
            new WebElement().ByXPath("//input[@name='ig_id']").Click();
            new WebElement().ByXPath("//input[@id='" + base_id + "']").Click();

            foreach (var packet in packets)
            {
                new WebElement().ByXPath("//input[@id='" + packet + "']").Click();
            }

            new WebElement().ByXPath("//input[@name='change_iptv_serv']").Click();
        }
    }
}
