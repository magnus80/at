using System;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD.ptn.iptv
{
    public class archive_rent_fld: PageBase
    {
        /// <summary>
        /// подключение архивного сервиса аренды
        /// </summary>
        /// <param name="guid">ид приставки, которая была арендована ранее</param>
        public void ConnectArchRent(string guid)
        {
            new WebElement().ByXPath("//input[@id='guid']").SendKeys(guid);
            new WebElement().ByXPath("//a[@id='showRents']").Click();
            new WebElement().ByXPath("//input[@name='service']").Click();
            new WebElement().ByXPath("//textarea[@id='comment']").SendKeys("autotest_" + DateTime.Now.ToShortDateString());
            new WebElement().ByXPath("//input[@id='rentConsole']").Click();
        }
    }
}
