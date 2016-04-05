using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class newglobal__pl: PageBase
    {
        #region elements
        
        #endregion

        #region actions

        public void NewGP_information()
        {
            new WebElement().ByXPath("//input[@name='area']").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();

            new WebElement().ByXPath("//input[@name='resolve_date_dd']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mm']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_yyyy']").SendKeys("2016");
            new WebElement().ByXPath("//input[@name='resolve_date_hh']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mi']").SendKeys("01");

            new WebElementSelect().ByXPath("//select[@id='gp_type']").SelectByText("Информация");

            new WebElement().ByXPath("//input[@name='write_global']").Click();

        }

        public void NewGP_resourse()
        {
            new WebElement().ByXPath("//input[@name='area']").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();

            new WebElement().ByXPath("//input[@name='resolve_date_dd']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mm']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_yyyy']").SendKeys("2016");
            new WebElement().ByXPath("//input[@name='resolve_date_hh']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mi']").SendKeys("01");

            new WebElementSelect().ByXPath("//select[@id='gp_type']").SelectByText("Ресурсы");

            new WebElement().ByXPath("//input[@name='write_global']").Click();

        }

        public void NewGP_connect()
        {
            new WebElement().ByXPath("//input[@name='area']").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();

            
            new WebElement().ByXPath("//input[@name='resolve_date_dd']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mm']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_yyyy']").SendKeys("2016");
            new WebElement().ByXPath("//input[@name='resolve_date_hh']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mi']").SendKeys("01");

            new WebElementSelect().ByXPath("//select[@id='gp_type']").SelectByText("Подключения");
        
            new WebElement().ByXPath("//input[@name='write_global']").Click();

        }

        public void NewGP_crash()
        {
            new WebElement().ByXPath("//input[@name='area']").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();

            
            new WebElement().ByXPath("//input[@name='resolve_date_dd']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mm']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_yyyy']").SendKeys("2016");
            new WebElement().ByXPath("//input[@name='resolve_date_hh']").SendKeys("01");
            new WebElement().ByXPath("//input[@name='resolve_date_mi']").SendKeys("01");

            new WebElementSelect().ByXPath("//select[@id='gp_type']").SelectByText("Авария");
            new WebElement().ByXPath("//input[@id='service_failure']").Click();
            new WebElement().ByXPath("//input[@name='write_global']").Click();
            
        }


        #endregion
    }
}
