using AT;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace USSS.WebPages.B2BPost
{
     class RequestHistoryPage :PageBase
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

        public string CheckStatus()
        {
            for (int i = 0; i < 10; i++)
            {
                WebElement LastRequestStutusWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='status']");
                if (LastRequestStutusWE.Displayed & LastRequestStutusWE.Text != "Выполняется" & LastRequestStutusWE.Text != "Открыт") return LastRequestStutusWE.Text;
                Thread.Sleep(5000);
                WebElement btnRefresh = new WebElement().ByXPath("//a[@id='formRequest:refreshRequestsTableButton']");
                btnRefresh.Click();
            }
            return "Заявка не отображена, либо не обработана более 5 минут";
        }

         public string GoToRequest()
         {
             WebElement RequestHref = new WebElement().ByXPath("//tbody[@id='formRequest:requestsTable_data']/tr[1]/td[1]/a");

             if(RequestHref.Displayed)
             {
                 RequestHref.Click();
                 return "success";
             }
             else
             {
                 return "Не отображена ссылка на заявку";
             }
         }
         public string CheckTariffChangeRequest(string old_soc,string new_soc)
         {
             string New_tariffWE = new WebElement().ByXPath("//form[@id='j_idt311']/div[1]/div[2]/div/div[1]/div[2]/div[2]/div[2]").Text;
             string Old_tariffWE = new WebElement().ByXPath("//tbody[@id='j_idt311:massRequestTable_data']/tr/td[2]").Text;
             WebElement date = new WebElement().ByXPath("//form[@id='j_idt311']/div[1]/div[2]/div/div[1]/div[1]/div[5]/div[2]");


             var new_ent_name = Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code = '"+new_soc.Replace(" ","")+"'");
             var old_ent_name = Executor.ExecuteSelect("select entity_name from web_entity where ext_entity_code = '" + old_soc.Replace(" ","") + "'");

             if(!New_tariffWE.Contains(new_ent_name[0,0]))
             {
                 return "Не отображена информация о новом тарифе";
             }
             if(!Old_tariffWE.Contains(old_ent_name[0,0]))
             {
                 return "Не отобрадена информация о старом тарифе";
             }
             if(!date.Displayed)
             {
                 return "Не отображена дата";
             }
             return "success";
         }
        public string GetDetails()
        {
            WebElement LastRequestNumberWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='column-block custom-heading']/a");
            LastRequestNumberWE.Click();
            return new WebElement().ByXPath("//div[@class='params-table'][2]/div[@class='row'][2]/div[@class='value']").Text;
        }

        public string getLastRequestId(ref string ReqID)
        {
            WebElement LastRequestStutusWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[1]");
            ReqID = LastRequestStutusWE.Text;
            return "success";
        }
        public string CheckStatus(ref string ReqID)
        {
            RequestListWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td/a");

            if (!RequestListWE.Displayed) { return "Не отображены элементы интерфейса: список заявок"; }
            if (RequestListWE.Text != ReqID) { return "Не отображена созданная заявка"; }

            for (int i = 0; i < 10; i++)
            {
                WebElement LastRequestStutusWE = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//td[@class='status']");
                if (LastRequestStutusWE.Displayed & LastRequestStutusWE.Text != "Выполняется" & LastRequestStutusWE.Text != "Открыт") return LastRequestStutusWE.Text;
                Thread.Sleep(5000);
                WebElement btnRefresh = new WebElement().ByXPath("//a[@id='formRequest:refreshRequestsTableButton']");
                btnRefresh.Click();
            }
            return "Заявка не отображена, либо не обработана более 5 минут";
        }

        public string CheckDownloadReportLink()
        {
            WebElement weDownloadLink = new WebElement().ByXPath("//a[contains(@id,'formRequest:requestsTable:0')]");

            if (!weDownloadLink.Displayed) return "Не отображена ссылка загрузить отчёт";
            weDownloadLink.Click();

            Thread.Sleep(1500);
            SendKeys.SendWait("{DOWN}");
            Thread.Sleep(200);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            string path = Environment.GetEnvironmentVariable("HOMEPATH") + "\\Downloads";
            Directory.SetCurrentDirectory(path);


            path = Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(path, "Otchet_po_*.CSV");

            if (files.Length == 0)
            {
                return "Файл не скачан";
            }

            foreach (var file in files)
            {
                FileInfo f = new FileInfo(file);
                long vLength = f.Length;
                File.Delete(file);
                if (vLength <= 0)
                {
                    return "Файл отчета пустой";
                }

            }


            return "success";
        }
        
        #endregion

    }
}
