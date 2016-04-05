using AT;
using AT.WebDriver;

namespace INAC.WebPages.OSE
{
    public class inaclogin__pl : PageBase
    {
        public string Login
        {
            set { new WebElement().ByXPath("//input[@name='login']").SendKeys(value); }
        }

        public string Password
        {
            set { new WebElement().ByXPath("//input[@name='password']").SendKeys(value); }
        }

        public void Submit()
        {
            new WebElement().ByXPath("//input[@name='submit']").Click();
        }
    }
}
