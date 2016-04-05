using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.WebPages.B2CPre;

namespace USSS.WebPages.B2BCtn
{
    internal class ProfilePage
    {

        public FinansAndDetalizationPage finansAndDetalizationPage;
        public TariffsPage tariffsPage;

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

            GetReportWE =
                new WebElement().ByXPath(
                    "//form[contains(@id,'finInfoIndexPage')]//button[contains(@id,'finInfoIndexPage')]");
            GetHistoryReportsWE = new WebElement().ByXPath("//form[contains(@id,'finInfoIndexPage')]//a");

            if (!ProfileWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на профиль";
            }
            if (!NoticeWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на уведомления";
            }
            if (!SettingsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на настройки";
            }
            if (!TariffsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на тарифы";
            }
            if (!ServicesWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на услуги";
            }
            if (!FinanceWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на финансы и информацию";
            }
            if (!RequestHistoryWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на историю запросов";
            }
            if (!FeedbackWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на обратную связь";
            }
            if (!PaymentMethods.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на способы оплаты";
            }

            if (!GetReportWE.Displayed)
            {
                return "Не отображены элементы интерфейса: кнопка заказа детализации в виджите детализации";
            }
            if (!GetHistoryReportsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылки на историю заказанных отчетов в виджите детализации";
            }

            return "success";
        }

        public string ConstructionWidget()
        {
            if (!GetReportWE.Displayed)
            {
                return "Не отображены элементы интерфейса: кнопка заказа детализации в виджите детализации";
            }
            if (!GetHistoryReportsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылки на историю заказанных отчетов в виджите детализации";
            }

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

        public string GoToTariff()
        {
            TariffsWE = new WebElement().ByXPath("//a[contains(@onclick,'tariffs')]");
            if (!TariffsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на тарифы";
            }
            TariffsWE.Click();
            tariffsPage = new TariffsPage();
            return tariffsPage.ConstructionPage();
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

        #endregion


    }
}
