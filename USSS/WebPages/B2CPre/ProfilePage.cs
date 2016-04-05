using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace USSS.WebPages.B2CPre
{
    class ProfilePage
    {
        public FinansAndDetalizationPage finansAndDetalizationPage;
        public RequestHistoryPage requestHistoryPage;
        public TariffsPage tariffsPage;
        public ServicesPage servicesPage;
        public string balance;

        #region constructor


        #region navigation WE

        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        //ссылка на уведомления в навигации
        private WebElement NoticeWE;
        //ссылка на настройки в навигации
        private WebElement SettingsWE;
        //ссылка на тарифы в навигации
        private WebElement TariffsWE;
        //ссылка на услуги в навигации
        private WebElement ServicesWE;
        //ссылка на финансы детализацию в навигации
        private WebElement FinanceWE;
        //ссылка на историю запросов в навигации
        private WebElement RequestHistoryWE;
        //ссылка на обратную связь в навигации
        private WebElement FeedbackWE;
        //ссылка на способы оплаты в навигации
        private WebElement PaymentMethods;

        #endregion

        #region FinInfoBlock

        //кнопка заказа отчета в виджите
        private WebElement GetReportWE;
        //ссылка на историю отчетов в виджите
        private WebElement GetHistoryReportsWE;

        #endregion

        public string ConstructionPage()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id = 'preProfile']");
            NoticeWE = new WebElement().ByXPath("//a[contains(@onclick,'notice')]");
            SettingsWE = new WebElement().ByXPath("//a[contains(@onclick,'settings')]");
            TariffsWE = new WebElement().ByXPath("//a[contains(@onclick,'tariffs')]");
            ServicesWE = new WebElement().ByXPath("//a[contains(@onclick,'services')]");
            FinanceWE = new WebElement().ByXPath("//a[contains(@onclick,'finance')]");
            RequestHistoryWE = new WebElement().ByXPath("//a[contains(@onclick,'operationsHistory')]");
            FeedbackWE = new WebElement().ByXPath("//a[contains(@onclick,'feedback')]");
            PaymentMethods = new WebElement().ByXPath("//a[contains(@onclick,'paymentMethods')]");

            GetReportWE = new WebElement().ByXPath("//form[contains(@id,'finInfoIndexPage')]//button[contains(@id,'finInfoIndexPage')]");
            GetHistoryReportsWE = new WebElement().ByXPath("//form[contains(@id,'finInfoIndexPage')]//a");

            if (!ProfileWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на профиль"; }
            if (!NoticeWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на уведомления"; }
            if (!SettingsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на настройки"; }
            if (!TariffsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на тарифы"; }
            if (!ServicesWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на услуги"; }
            if (!FinanceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на финансы и информацию"; }
            if (!RequestHistoryWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на историю запросов"; }
            if (!FeedbackWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на обратную связь"; }
            if (!PaymentMethods.Displayed) { return "Не отображены элементы интерфейса: ссылка на способы оплаты"; }

            return "success";
        }

        public string ConstructionWidget()
        {
            if (!GetReportWE.Displayed) { return "Не отображены элементы интерфейса: кнопка заказа детализации в виджите детализации"; }
            if (!GetHistoryReportsWE.Displayed) { return "Не отображены элементы интерфейса: ссылки на историю заказанных отчетов в виджите детализации"; }

            return "success";
        }


        public string CheckBalance()
        {
            WebElement BalanceUpdateWE = new WebElement().ByXPath("//form[contains(@id,'balanceBlock')]//a[contains(@id,'updateBalanceId')]");
            if (!BalanceUpdateWE.Displayed) { return "Не отображены элементы интерфейса: ссылка обновления баланса"; }
            BalanceUpdateWE.Click();
            Thread.Sleep(10000);
            WebElement BalanceWE = new WebElement().ByXPath("//form[contains(@id,'balanceBlock')]//span[contains(@class,'price')]");
            if (!BalanceWE.Displayed) { return "Не отображены элементы интерфейса: баланс"; }
            balance = BalanceWE.Text;
            
            return "success";
        }

        #endregion


        #region managerPage

        public string GoToFinanceAndDetalization()
        {
            if (FinanceWE.Displayed) FinanceWE.Click();
            else
            {
                return "Не отображены элементы интерфейса: ссылка на финансы и информацию";
            }
            finansAndDetalizationPage = new FinansAndDetalizationPage();
            return finansAndDetalizationPage.ConstructionPage();

        }

        public string GoToSubscription()
        {
            try
            {
                new WebElement().ByXPath("//a[contains(text(),'Подписки')]").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToDetalization()
        {
            try
            {
                if (
                    new WebElement().ByXPath(
                        "//form[contains(@id,'finInfoIndexPage')]//button[contains(@id,'finInfoIndexPage')]").Displayed)
                {
                    new WebElement().ByXPath(
                        "//form[contains(@id,'finInfoIndexPage')]//button[contains(@id,'finInfoIndexPage')]").Click();
                    return "success";
                }
                return "Не отображены элементы интерфейса";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToDetalizationHistory()
        {
            try
            {
                if (new WebElement().ByXPath("//form[contains(@id,'finInfoIndexPage')]//a").Displayed)
                {
                    new WebElement().ByXPath("//form[contains(@id,'finInfoIndexPage')]//a").Click();
                    return "success";
                }
                return "Не отображены элементы интерфейса";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToRequestHistoryPage()
        {
            RequestHistoryWE = new WebElement().ByXPath("//a[contains(@onclick,'operationsHistory')]");
            if (RequestHistoryWE.Displayed) RequestHistoryWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на историю заявок"; }
            requestHistoryPage = new RequestHistoryPage();
            return requestHistoryPage.ConstructionPage();
        }

        public string GoToTariff()
        {
            TariffsWE = new WebElement().ByXPath("//a[contains(text(),'Тарифы')]");
            if (!TariffsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на тарифы";
            }
            TariffsWE.Click();
            tariffsPage = new TariffsPage();
            return tariffsPage.ConstructionPage();
        }

        public string GoToService()
        {
            ServicesWE = new WebElement().ByXPath("//a[contains(@onclick,'services')]");
            if (!ServicesWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на услуги";
            }
            ServicesWE.Click();
            servicesPage = new ServicesPage();
            return servicesPage.ConstructionPage();
        }

        public string GetCurrentTariff(string db_Ans, string number)
        {
            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            var q = @"select soc from ecr9_service_agreement where subscriber_no = '" + number + @"'
                        and service_type = 'P'
                        and (TO_CHAR(TRUNC(EXPIRATION_DATE),'YYYYMMDD')>=" +
                    date + @" or EXPIRATION_DATE is null) 
                        and TO_CHAR(TRUNC(EFFECTIVE_DATE),'YYYYMMDD')<=" + date;
            var socB = Executor.ExecuteSelect(q);
            if (socB.Count != 0)
            {
                string soc = socB[0, 0];
                return soc;
            }

            return "Тариф отсутствует";
        }

        #endregion

       
    }
}
