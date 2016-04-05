using AT;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.CommonPages
{
    class SKPPage : PageBase
    {
        public string balance;

        public string GetBalance(string ban)
        {
            WebElement banInp = new WebElement().ByXPath("//*[@id='txtSearchString']");
            banInp.SendKeys(ban);
            new WebElement().ByXPath("//*[@id='ibSearch']").Click();
            new WebElement().ByXPath("//*[@id='table_main']/tbody/tr[2]/td[1]/a").Click();
            balance = new WebElement().ByXPath("//*[@id='ClientInfo1_lblBalance']").Text;
            return "success";
        }

        public string AddPayment(string cost)
        {

            WebElement operation = new WebElement().ByXPath("//a[@id='MainMenu1_hlOperations']");
            operation.Click();
            new WebElement().ByXPath("//input[@id='tbCorrect']").SendKeys(cost);
            new WebElement().ByXPath("//img[@id='ibDoCorrect']").Click();
            SwitchToPopupWindow();
            new WebElement().ByXPath("//input[@id='txtPaymentNum']").SendKeys("1111");
            new WebElement().ByXPath("//textarea[@id='comment']").SendKeys("Автотест");
            new WebElement().ByXPath("//input[@id='btnOk']").Click();
            
            return "success";
        }
    }
}
