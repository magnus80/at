using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class netfone_add__pl : PageBase
    {
        /// <summary>
        /// создание задания на подключение нетфона
        /// </summary>
        /// <param name="nickname">желаемый никнейм</param>
        /// <param name="service">ид сервиса</param>
        public void AddNetphone(string nickname, string service)
        {
            new WebElement().ByXPath("//input[@value='" + service + "']").Click();
            new WebElement().ByXPath("//input[@name='nickname']").SendKeys(nickname);
            new WebElement().ByXPath("//a[@id='check_nick']/b").Click();
            new WebElement().ByXPath("//input[@name='add_nickname']").Click();
            Browser.AssertDialog();
        }

        public void ChangeNetphone(string service)
        {
            new WebElement().ByXPath("//input[@value='" + service + "']").Click();
            new WebElement().ByXPath("//input[@name='change_netfone']").Click();
            Browser.AssertDialog();
        }

        public void DelNetphone()
        {
            new WebElement().ByXPath("//input[@name='del_nickname']").Click();
        }
    }
}
