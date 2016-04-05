using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class Bundle_services2__pl : PageBase
    {
        public Bundle_services2__pl()
        {
            //SensReg = true;
        }


        /// <summary>
        /// Подключение бандла с ШПД и wifi арендой (в бандле кроме ШПД и Аренды не должно быть ничего)
        /// </summary>
        /// <param name="bundle">ид бандла</param>
        /// <param name="router">серийник роутера</param>
        public void ConnectBundleVdpnAndWifiRent(string bundle, string router)
        {
            new WebElement().ByXPath("//input[@id='" + bundle + "']").Click();
            new WebElement().ByXPath("//input[@name='" + bundle + "_VPDN']").Click();
            new WebElement().ByXPath("//input[@name='" + bundle + "_W_STOPPABLE_RENT']").Click();
            new WebElement().ByXPath("//input[@name='" + bundle + "_W_STOPPABLE_RENT_own_text']").SendKeys(router);
            new WebElement().ByXPath("//input[@name='try_bundle']").Click();
            new WebElement().ByXPath("//input[@name='confirm']").Click();
        }
    }
}
