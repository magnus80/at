using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD.ptn.add_speed
{
    public class change_speed_fld : PageBase
    {
        public void LevelUpClick()
        {
            new WebElement().ByXPath("//div[@id='level_up']").Click();
        }

        public void NextClick()
        {
            new WebElement().ByXPath("//input[@id='next']").Click();
        }

        public void AsseptConnectWithPromisedPay()
        {
            new WebElement().ByXPath("//input[@name='pay_flags']").Click();
            new WebElement().ByXPath("//button[@id='next_button']").Click();
        }

        public void WaitOrdering()
        {
            System.Threading.Thread.Sleep(10000);
        }
    }
}
