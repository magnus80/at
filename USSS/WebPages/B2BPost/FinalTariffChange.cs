using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AT;
using AT.DataBase;
using AT.WebDriver;

namespace USSS.WebPages.B2BPost
{
    class FinalTariffChange:PageBase
    {
        public RequestHistoryPage requestHistoryPage;
        #region construct

        private WebElement h3 = new WebElement().ByXPath("//h3[contains(text(),'Запросы на изменения тарифов отправлены')]");
        private WebElement buttonReturn = new WebElement().ByXPath("//a[contains(@href,'/b/info/abonents/catalog.xhtml')]");
        private WebElement RequestHistoryWE = new WebElement().ByXPath("//a[@id='navRequests']");
        public string Construct()
        {
            if(!h3.Displayed)
            {
                return "Не отображен текст Запросы на изменения тарифов отправлены";
            }
            if(!buttonReturn.Displayed)
            {
                return "Не отображена кнопка возврата";
            }
            return "success";
        }
        #endregion

        #region manager page
        public string GoToRequestPage()
        {
            string result;
            if(RequestHistoryWE.Displayed)
            {
                RequestHistoryWE.Click();
                requestHistoryPage = new RequestHistoryPage();
                result = requestHistoryPage.ConstructionPage();
                return result;
            }
            else
            {
                return "Не отображена ссылка на старинцу запросов";
            }
        }
        #endregion
    }
}
