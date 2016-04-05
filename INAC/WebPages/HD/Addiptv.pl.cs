using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class addiptv__pl : PageBase
    {
        /// <summary>
        /// подключение иптв пакета
        /// </summary>
        /// <param name="guid">ид приставки</param>
        /// <param name="base_id">ид базового пакета</param>
        /// <param name="packets">список ид пакетов, внутри базового, которые подключаем</param>
        public void ConnectIptv(string guid, string base_id, List<string> packets)
        {

            new WebElement().ByXPath("//input[@name='guid']").SendKeys(guid);
            new WebElement().ByXPath("//input[@id='" + base_id + "']").Click();
            
            foreach (var packet in packets)
            {
                new WebElement().ByXPath("//input[@id='" + packet + "']").Click();            
            }

            new WebElement().ByXPath("//input[@name='addiptv']").Click();
        }
    }
}
