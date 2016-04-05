using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2BPost
{
    class HomePage
    {
        public ManagerContractPage managerContractPage;
        public RequestHistoryPage requestHistoryPage;
        public ReportsPage reportsPage;
        public FinancePage financePage;

        #region constructor

        WebElement StructBtnWE;
        WebElement SearchWE;
        #region navigation WE

        //ссылка на профиль в навигации
        private WebElement HomeWE;
        //ссылка на уведомления в навигации
        private WebElement ManagerContactWE;
        //ссылка на настройки в навигации
        private WebElement SevicesWE;
        //ссылка на финансы детализацию в навигации
        private WebElement FinanceWE;
        //ссылка на историю запросов в навигации
        private WebElement ReportsWE;
        //ссылка на обратную связь в навигации
        private WebElement FeedbackWE;
        //ссылка на способы оплаты в навигации
        private WebElement RequestHistoryWE;

        #endregion

        public string ConstructionPage()
        {
            HomeWE = new WebElement().ByXPath("//a[@id = 'navHome']");
            ManagerContactWE = new WebElement().ByXPath("//a[@id = 'navCatalog']");
            SevicesWE = new WebElement().ByXPath("//a[contains(@id,'navProducts')]");
            FinanceWE = new WebElement().ByXPath("//a[contains(@id,'finance')]");
            ReportsWE = new WebElement().ByXPath("//a[contains(@id,'reports')]");
            FeedbackWE = new WebElement().ByXPath("//a[contains(@id,'feedback')]");
            RequestHistoryWE = new WebElement().ByXPath("//a[contains(@id,'navRequests')]");
            StructBtnWE = new WebElement().ByXPath("//a[@onclick='toggleHierarchyPopup()']");
            SearchWE = new WebElement().ByXPath("//input[@id='q']");
            if (!HomeWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на главную страницу"; }
            if (!ManagerContactWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на управление контрактом"; }
            if (!SevicesWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на каталог услуг"; }
            if (!FinanceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на финансовую информацию"; }
            if (!ReportsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на отчеты"; }
            if (!RequestHistoryWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на историю запросов"; }
            if (!FeedbackWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на обратную связь"; }
            if (!StructBtnWE.Displayed) { return "Не отображены элементы интерфейса: кнопка структуры"; }
            if (!SearchWE.Displayed) { return "Не отображены элементы интерфейса: поле поиска"; }

            return "success";
        }

        public string GoToReportsPage()
        {
            ReportsWE = new WebElement().ByXPath("//a[contains(@id,'reports')]");
            if (ReportsWE.Displayed) ReportsWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на управление контрактом"; }
            reportsPage = new ReportsPage();
            return reportsPage.ConstructionPage();
        }

        public string ConstructionPage(string AT)
        {
            if (AT == "105")
            {
                HomeWE = new WebElement().ByXPath("//a[@id = 'navHome']");
                ManagerContactWE = new WebElement().ByXPath("//a[@id = 'navCatalog']");
                SevicesWE = new WebElement().ByXPath("//a[contains(@id,'navProducts')]");
                FinanceWE = new WebElement().ByXPath("//a[contains(@id,'finance')]");
                ReportsWE = new WebElement().ByXPath("//a[contains(@id,'reports')]");
                FeedbackWE = new WebElement().ByXPath("//a[contains(@id,'feedback')]");
                RequestHistoryWE = new WebElement().ByXPath("//a[@id='navRequests']");
                StructBtnWE = new WebElement().ByXPath("//a[@onclick='toggleHierarchyPopup()']");
                SearchWE = new WebElement().ByXPath("//input[@id='q']");

                WebElement we;
                we = new WebElement().ByXPath("//a[contains(@id,'guiRefunds:billAnalyzer')]");
                if (!we.Displayed) { return "Не отображены элементы интерфейса: ссылка на Анализатор счета"; }

                we = new WebElement().ByXPath("//a[contains(@id,'guiSMS:sms')]");
                if (!we.Displayed) { return "Не отображены элементы интерфейса: ссылка на Отправка СМС"; }


                if (!HomeWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на главную страницу"; }
                if (!ManagerContactWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на управление контрактом"; }
                if (!SevicesWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на каталог услуг"; }
                if (!FinanceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на финансовую информацию"; }
                if (!ReportsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на отчеты"; }
                if (!RequestHistoryWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на историю запросов"; }
                if (!FeedbackWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на обратную связь"; }
                if (!StructBtnWE.Displayed) { return "Не отображены элементы интерфейса: кнопка структуры"; }
                if (!SearchWE.Displayed) { return "Не отображены элементы интерфейса: поле поиска"; }
            }
            return "success";
        }

        #endregion

        #region managerPage

        public string GoToManagerContractPage()
        {
            if (ManagerContactWE.Displayed) ManagerContactWE.Click();
            else{ return "Не отображены элементы интерфейса: ссылка на управление контрактом";}
            managerContractPage = new ManagerContractPage();
            return managerContractPage.ConstructionPage();  
        }

        public string GoToRequestHistoryPage()
        {
            requestHistoryPage = new RequestHistoryPage();
            RequestHistoryWE = new WebElement().ByXPath("//a[contains(@id,'navRequests')]");
            if (RequestHistoryWE.Displayed) RequestHistoryWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на историю заявок"; }
            return requestHistoryPage.ConstructionPage();
        }

        public string GoToFinancePage()
        {
            FinanceWE = new WebElement().ByXPath("//a[contains(@id,'finance')]");
            if (FinanceWE.Displayed) FinanceWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка Финансовая информация"; }
            financePage = new FinancePage();
            return financePage.ConstructionPage();
        }
        #endregion

    }
}
