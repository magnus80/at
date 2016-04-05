using AT;
using AT.WebDriver;

namespace INAC.WebPages.OSE.services
{
    public class form__py : PageBase
    {
        
        #region vpdn

        public void OpenVpdnTab()
        {
            new WebElement().ByXPath("//a[contains(@href, '#internet')]").Click();
        }

        public string VpdnAction
        {
            get { return new WebElementSelect().ByXPath("//select[@name='action']").GetSelectedValue(); }
            set { new WebElementSelect().ByXPath("//select[@name='action']").SelectByValue(value); }
        }

        public string VpdnLid
        {
            set { new WebElement().ByXPath("//input[@name='lid']").SendKeys(value); }
        }

        public string VpdnTechlist
        {
            set { new WebElement().ByXPath("//input[@name='file']").LoadFile(value); }
        }

        public string VpdnStartDate
        {
            set { new WebElement().ByXPath("//input[@name='start_date']").SendKeys(value); }
        }

        public void VpdnSubmit()
        {
            new WebElement().ByXPath("//input[@value='Применить']").Click();
        }

        #endregion

        #region iptv

        public void OpenIptvTab()
        {
            new WebElement().ByXPath("//a[contains(@href, '#iptv')]").Click();
        }

        public string IptvAction
        {
            get { return new WebElementSelect().ByXPath("(//select[@name='action'])[2]").GetSelectedValue(); }
            set { new WebElementSelect().ByXPath("(//select[@name='action'])[2]").SelectByValue(value); }
        }

        public string IptvLid
        {
            set { new WebElement().ByXPath("(//input[@name='lid'])[2]").SendKeys(value); }
        }

        public string IptvTechlist
        {
            set { new WebElement().ByXPath("(//input[@name='file'])[2]").LoadFile(value); }
        }

        public string IptvStartDate
        {
            set { new WebElement().ByXPath("(//input[@name='start_date'])[2]").SendKeys(value); }
        }

        public void IptvSubmit()
        {
            new WebElement().ByXPath("(//input[@value='Применить'])[2]").Click();
        }

        #endregion

        #region bundles

        public void OpenBundleTab()
        {
            new WebElement().ByXPath("//a[contains(@href, '#load_bundles')]").Click();
        }

        public string BundleAction
        {
            get { return new WebElementSelect().ByXPath("(//select[@name='action'])[3]").GetSelectedValue(); }
            set { new WebElementSelect().ByXPath("(//select[@name='action'])[3]").SelectByValue(value); }
        }

        public string BundleLid
        {
            set { new WebElement().ByXPath("(//input[@name='lid'])[4]").SendKeys(value); }
        }

        public string BundleTechlist
        {
            set { new WebElement().ByXPath("(//input[@name='file'])[4]").LoadFile(value); }
        }

        public string BundleStartDate
        {
            set { new WebElement().ByXPath("(//input[@name='start_date'])[4]").SendKeys(value); }
        }

        public void BundleSubmit()
        {
            new WebElement().ByXPath("(//input[@value='Применить'])[4]").Click();
        }

        #endregion

        #region annual

        public void OpenAnnualTab()
        {
            new WebElement().ByXPath("(//a[contains(@href, '#internet')])[2]").Click();
        }

        public string AnnualAction
        {
            get { return new WebElementSelect().ByXPath("(//select[@name='action'])[4]").GetSelectedValue(); }
            set { new WebElementSelect().ByXPath("(//select[@name='action'])[4]").SelectByValue(value); }
        }

        public string AnnualLid
        {
            set { new WebElement().ByXPath("(//input[@name='lid'])[5]").SendKeys(value); }
        }

        public string AnnualTechlist
        {
            set { new WebElement().ByXPath("(//input[@name='file'])[5]").LoadFile(value); }
        }

        public string AnnualStartDate
        {
            set { new WebElement().ByXPath("(//input[@name='start_date'])[5]").SendKeys(value); }
        }

        public void AnnualSubmit()
        {
            new WebElement().ByXPath("(//input[@value='Применить'])[5]").Click();
        }

        #endregion


        #region annual

        public void OpenNetphoneTab()
        {
            new WebElement().ByXPath("//a[contains(@href, '#load_netphone')]").Click();
        }

        public string NetphoneAction
        {
            get { return new WebElementSelect().ByXPath("(//select[@name='action'])[5]").GetSelectedValue(); }
            set { new WebElementSelect().ByXPath("(//select[@name='action'])[5]").SelectByValue(value); }
        }

        public string NetphoneLid
        {
            set { new WebElement().ByXPath("(//input[@name='lid'])[6]").SendKeys(value); }
        }

        public string NetphoneTechlist
        {
            set { new WebElement().ByXPath("(//input[@name='file'])[6]").LoadFile(value); }
        }

        public string NetphoneStartDate
        {
            set { new WebElement().ByXPath("(//input[@name='start_date'])[6]").SendKeys(value); }
        }

        public void NetphoneSubmit()
        {
            new WebElement().ByXPath("(//input[@value='Применить'])[6]").Click();
        }

        #endregion

    }
}
