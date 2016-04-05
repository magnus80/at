using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace USSS.WebPages.CommonPages
{
    internal class RequestHistoryPage
    {

        #region managerPage

        public string ChangeOfStatus(string state)
        {
            try
            {
                if (!new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td").Displayed)
                {
                    return "Не отображены элементы интерфейса: таблица Истории заявок";
                }
                    string status = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[contains(text(),'" + state + "')]").Text;
                    if (status == state)
                        return "success";
                
                return "failed";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string CheckFiltr()
        {
            WebElement TypeRequestWE = new WebElement().ByXPath("//div[@id='formRequest:requestsTable:requestTypeId']//label[@id='formRequest:requestsTable:requestTypeId_label']");
            if (!TypeRequestWE.Displayed) { return "Фильтр не выбран"; }
            return TypeRequestWE.Text;
        }


        
        #endregion

    }
}
