using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace USSS.WebPages.B2CPost
{
     class RequestHistoryPage
    {

        #region constructor

        //таблица запросов
        private WebElement RequestListWE;


        public string ConstructionPage()
        {
            RequestListWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td");

            if (!RequestListWE.Displayed) { return "Не отображены элементы интерфейса: список заявок"; }

            return "success";
        }


        #endregion


        #region managerPage

        public string CheckStatus(string number)
        {
             WebElement num = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='column-block custom-heading']/a");
             if (number == num.Text)
             {
                 for (int i = 0; i < 10; i++)
                 {
                     WebElement LastRequestStutusWE =
                         new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='status']");
                     if (LastRequestStutusWE.Displayed & LastRequestStutusWE.Text != "Выполняется" &
                         LastRequestStutusWE.Text != "Открыт") return LastRequestStutusWE.Text;
                     Thread.Sleep(5000);
                     WebElement btnRefresh = new WebElement().ByXPath("//div[@class='update-link']//a");
                     btnRefresh.Click();
                 }
             }
            return "Заявка не отображена, либо не обработана более 5 минут";
        }

        public string GetDetails()
        {
            WebElement LastRequestNumberWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='column-block custom-heading']/a");
            LastRequestNumberWE.Click();
            return new WebElement().ByXPath("//div[@class='params-table'][2]/div[@class='row'][2]/div[@class='value']").Text;
        }

        public string CheckStatus()
        {
            for (int i = 0; i < 10; i++)
            {
                WebElement LastRequestStutusWE = new WebElement().ByXPath("//table/tbody[@id='formRequest:requestsTable_data']//tr[1]/td[5]");
                if (LastRequestStutusWE.Displayed & LastRequestStutusWE.Text != "Выполняется" & LastRequestStutusWE.Text != "Открыт") return LastRequestStutusWE.Text;
                Thread.Sleep(5000);
                WebElement btnRefresh = new WebElement().ByXPath("//a[@id='formRequest:j_idt704']");
                btnRefresh.Click();
            }
            return "Заявка не отображена, либо не обработана более 5 минут";
        }

        public string CheckTableField(int i, string text)
        {
            Thread.Sleep(500);
            WebElement we;

            we = new WebElement().ByXPath("//table/tbody[@id='formRequest:requestsTable_data']/tr/td[" + i + "]");
            if (!we.Displayed) return "Не найден элемент таблицы";

            return !we.Text.Contains(text) ? "Не найден текст уведомления в таблице" : "success";
        }

        #endregion

    }
}
