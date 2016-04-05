using System.Threading;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.WebPages.CommonPages;

namespace USSS.WebPages.B2BPost
{
    class ReportsPage
    {
        private WebElement ReportsTable;
        private WebElement ReportAnalyser;
        private WebElement ReadyReports;
        private WebElement chargesReport;

        public string ConstructionPage()
        {
            ReportsTable = new WebElement().ByXPath("//div[@class = 'reports-main-block']");
            if (!ReportsTable.Displayed) { return "Не отображены элементы интерфейса: список отчетов"; }

            ReportAnalyser = new WebElement().ByXPath("//a[@href='/b/refund/preliminaryCostReport.xhtml']");
            if (!ReportAnalyser.Displayed) { return "Не отображены элементы интерфейса: Анализатор счета"; }

            ReadyReports = new WebElement().ByXPath("//div[@class='content-block ready-reports-block']");
            if (!ReadyReports.Displayed) { return "Не отображены элементы интерфейса: Готовые отчеты."; }


            return "success";
        }


        public string GoReportDetailsOfInvoice()
        {
            try
            {
                new WebElement().ByXPath("//a[@href='detail.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoReportAccrualsInvoice()
        {
            chargesReport = new WebElement().ByXPath("//a[@href='/b/report/charge.xhtml']");
            if (!chargesReport.Displayed) { return "Не отображены элементы интерфейса: Отчет по начислениям выставленного счёта"; }
            chargesReport.Click();
            string result = CheckAccrualsInvoicePage();

            return result;

            // return "success";

        }

        public string CheckDetailReportPage()
        {
            ReadyReports = new WebElement().ByXPath("//div[contains(@id,'formRequest:requestsTable')]");
            if (!ReadyReports.Displayed) { return "Не отображены элементы интерфейса: таблица с готовыми отчетами по детализации выставленнго счета"; }

            WebElement webutton = new WebElement().ByXPath("//button[(@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only orange with-last-date')]");
            if (!webutton.Displayed) { return "Не отображены элементы интерфейса: кнопка Заказать отчёт"; }

            WebElement weSelector = new WebElement().ByXPath("//div[(@class='ui-selectonemenu ui-widget ui-state-default ui-corner-all')]");
            if (!weSelector.Displayed) { return "Не отображены элементы интерфейса: селектор выбора периода"; }
            //weSelector.Click();
            //*[@id="form:j_idt825:j_idt837_panel"]/div/ul/li[2]

            return "success";
        }
        public string CheckAccrualsInvoicePage()
        {

            ReadyReports = new WebElement().ByXPath("//div[contains(@id,':formRequest:requestsTable')]");
            if (!ReadyReports.Displayed) { return "Не отображены элементы интерфейса: таблица с готовыми отчетами по начислениям выставленнго счета"; }

            WebElement webutton = new WebElement().ByXPath("//button[(@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only orange')]");
            if (!webutton.Displayed) { return "Не отображены элементы интерфейса: кнопка Заказать отчёт"; }

            WebElement weSelector = new WebElement().ByXPath("//div[(@class='ui-selectonemenu ui-widget ui-state-default ui-corner-all')]");
            if (!weSelector.Displayed) { return "Не отображены элементы интерфейса: селектор выбора периода"; }
            //weSelector.Click();
            //*[@id="form:j_idt825:j_idt837_panel"]/div/ul/li[2]

            return "success";
        }

        public string SelectAccrualsPeriod()
        {
            WebElement selector = new WebElement().ByXPath("//span[(@class='ui-icon ui-icon-triangle-1-s ui-c')]");
            if (!selector.Displayed) { return "Не отображены элементы интерфейса: стрелки селектора."; }
            selector.Click();

            WebElement Period = new WebElement().ByXPath("//div[contains(@id,'_panel')]/div/ul/li[2]");
            if (!Period.Displayed) { return "Не отображены элементы интерфейса: Нет расчетных периодов."; }
            Period.Click();

            return "success";
        }

        public string GoReportCurrentPeriodAccruals()
        {
            try
            {
                new WebElement().ByXPath("//a[@href='/faces/report/unbilledCallsOfflineReport.xhtml']").Click();
                new WebElement().ByXPath("//div[@class='content-block']//button").Click();
                Thread.Sleep(2000);
                new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:email']").SendKeys("dabolenikhin@beeline.ru");
                new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:smsInput']").SendKeys("9272882753");
                new WebElement().ByXPath("//button[contains(@id,'notification:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog')]").Click();
                new WebElement().ByXPath("//a[@href='/faces/info/requests/requests.xhtml']").Click();
                RequestHistoryPage rhp = new RequestHistoryPage();
                //while (rhp.ChangeOfStatus("Обработан") != "success")
                //{
                //    new WebElement().ByXPath("//a[contains(@onclick,'update')]").Click();
                //    Thread.Sleep(5000);
                //}
                new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//div[contains(@class,'file-link')]/a").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string GoReportDetail()
        {
            WebElement weReportDetail = new WebElement().ByXPath("//a[@href='/b/report/detail.xhtml']");
            if (!weReportDetail.Displayed) { return "Не отображены элементы интерфейса: Отчет по детализации выставленного счёта"; }
            weReportDetail.Click();
            string result = CheckDetailReportPage();

            return result;
        }

        public string GoReportServicesAndTariffPlans()
        {
            try
            {
                new WebElement().ByXPath("//a[@href='/faces/report/tariffsAndServices.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoReportAbonentOverLimit()
        {
            try
            {
                new WebElement().ByXPath("//a[@href='/faces/report/abonentOverLimit.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string ClickOrderReport(string param)
        {
            if (param == "Accruals")
            {
                WebElement webutton = new WebElement().ByXPath("//button[(@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only orange')]");
                if (!webutton.Displayed) { return "Не отображены элементы интерфейса: кнопка Заказать отчёт"; }
                webutton.Click();
            }
            if (param == "Details")
            {
                WebElement webutton1 = new WebElement().ByXPath("//button[(@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only orange with-last-date')]");
                if (!webutton1.Displayed) { return "Не отображены элементы интерфейса: кнопка Заказать отчёт"; }
                webutton1.Click();
            }
            return "success";
        }

        public string CheckSubmitNotifWindow(ref string ReqID)
        {
            Thread.Sleep(5000);
            WebElement weEmail =
                new WebElement().ByXPath("//input[@name='notification:requestUserServiceParamsForm:email']");
            //WebElement weEmail2 =
            //     new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:email']");
            //WebElement weEmail3 =
            //     new WebElement().ByXPath("//input[@class='ui-inputfield ui-inputtext ui-widget ui-state-default ui-corner-all']");

            if (!weEmail.Displayed) { return "Не отображены элементы интерфейса: поле Емейл"; }

            weEmail.SendKeys("kiryshkov@bellintegrator.ru");

            WebElement weSMS =
                new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:smsInput']");
            if (!weSMS.Displayed) { return "Не отображены элементы интерфейса: поле ввода номера телефона"; }
            weSMS.SendKeys("9272882753");

            WebElement weButton = new WebElement().ByXPath("//button[contains(@id,'notification:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog')]");
            if (!weButton.Displayed) { return "Не отображены элементы интерфейса: кнопка подтверждения"; }
            weButton.Click();
            Thread.Sleep(1000);
            WebElement ReqNumber =
                new WebElement().ByXPath("//div[@class='request-created']/a");
            if (!ReqNumber.Displayed) { return "Не отображены элементы интерфейса: страница с информацией по созданному запросу"; }
            ReqID = ReqNumber.Text;

            //    new WebElement().ByXPath("//a[@href='/faces/info/requests/requests.xhtml']").Click();
            return "success";
        }
        public string CheckSubmitNotifWindow()
        {
            Thread.Sleep(1500);
            WebElement weEmail =
                new WebElement().ByXPath("//input[@name='notification:requestUserServiceParamsForm:email']");
            //WebElement weEmail2 =
            //     new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:email']");
            //WebElement weEmail3 =
            //     new WebElement().ByXPath("//input[@class='ui-inputfield ui-inputtext ui-widget ui-state-default ui-corner-all']");

            if (!weEmail.Displayed) { return "Не отображены элементы интерфейса: поле Емейл"; }

            weEmail.SendKeys("dabolenikhin@beeline.ru");

            WebElement weSMS =
                new WebElement().ByXPath("//input[@id='notification:requestUserServiceParamsForm:smsInput']");
            if (!weSMS.Displayed) { return "Не отображены элементы интерфейса: поле ввода номера телефона"; }
            weSMS.SendKeys("9272882753");

            WebElement weButton = new WebElement().ByXPath("//button[contains(@id,'notification:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog')]");
            if (!weButton.Displayed) { return "Не отображены элементы интерфейса: кнопка подтверждения"; }
            weButton.Click();
            Thread.Sleep(1000);

            //    new WebElement().ByXPath("//a[@href='/faces/info/requests/requests.xhtml']").Click();
            return "success";
        }
    }
}
