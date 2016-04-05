using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2BPle
{
    class HomePage
    {
        public ManagerContractPage managerContractPage;
        public RequestHistoryPage requestHistoryPage;
        public ContractPage contractPage;
        public string balance;
        #region constructor

        WebElement StructBtnWE;
        WebElement SearchWE;
        WebElement ContractBtnWE;
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
            FinanceWE = new WebElement().ByXPath("//a[contains(@id,'corpPrepaid')]");
            ReportsWE = new WebElement().ByXPath("//a[contains(@id,'reports')]");
            FeedbackWE = new WebElement().ByXPath("//a[contains(@id,'feedback')]");
            RequestHistoryWE = new WebElement().ByXPath("//a[@id='navRequests']");
            StructBtnWE = new WebElement().ByXPath("//a[@onclick='toggleHierarchyPopup()']");
            SearchWE = new WebElement().ByXPath("//input[@id='q']");
            ContractBtnWE = new WebElement().ByXPath("//span[@class='expandable' and text()='Договоры']");
            if (!HomeWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на главную страницу"; }
            if (!ManagerContactWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на управление контрактом"; }
            if (!SevicesWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на каталог услуг"; }
            if (!FinanceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на корпоративную предоплату"; }
            if (!ReportsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на отчеты"; }
            if (!RequestHistoryWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на историю запросов"; }
            if (!FeedbackWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на обратную связь"; }
            if (!StructBtnWE.Displayed) { return "Не отображены элементы интерфейса: кнопка структуры"; }
            if (!SearchWE.Displayed) { return "Не отображены элементы интерфейса: поле поиска"; }
            if (!ContractBtnWE.Displayed) { return "Не отображены элементы интерфейса: кнопка Договоры"; }

            return "success";
        }


        #endregion

        #region managerPage

        public string GoToManagerContractPage()
        {
            ManagerContactWE = new WebElement().ByXPath("//a[@id = 'navCatalog']");
            if (ManagerContactWE.Displayed) ManagerContactWE.Click();
            else{ return "Не отображены элементы интерфейса: ссылка на управление контрактом";}
            managerContractPage = new ManagerContractPage();
            return managerContractPage.ConstructionPage();  

        }

        public string GoToRequestHistoryPage()
        {
            RequestHistoryWE = new WebElement().ByXPath("//a[@id='navRequests']");
            if (RequestHistoryWE.Displayed) RequestHistoryWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на историю заявок"; }
            requestHistoryPage = new RequestHistoryPage();
            return requestHistoryPage.ConstructionPage();
        }

        public string BoxContactInfoOpen()
        {
            if (ContractBtnWE.Displayed) ContractBtnWE.Click();
            else { return "Не отображены элементы интерфейса: кнопка Договоры"; }
            return CheckContractList();
        }

        private string CheckContractList()
        {
            WebElement table = new WebElement().ByXPath("//div[@class='mobile-contracts-list']");
            if (!table.Displayed) { return "Не отображены элементы интерфейса: таблица договоров"; }
            if (!table.ByXPath("//span[text()='Номер договора']").Displayed) { return "Не отображены элементы интерфейса: столбец Номер договора"; }
            if (!table.ByXPath("//span[text()='Название договора']").Displayed) { return "Не отображены элементы интерфейса: столбец Название договора"; }
            if (!table.ByXPath("//span[text()='Количество абонентов']").Displayed) { return "Не отображены элементы интерфейса: столбец Количество абонентов"; }
            if (!table.ByXPath("//span[text()='Юридический адрес']").Displayed) { return "Не отображены элементы интерфейса: столбец Юридический адрес"; }
            if (!table.ByXPath("//span[text()='Регион']").Displayed) { return "Не отображены элементы интерфейса: столбец Регион"; }
            if (!table.ByXPath("//span[text()='Баланс корпоративной предоплаты, руб']").Displayed) { return "Не отображены элементы интерфейса: столбец Баланс корпоративной предоплаты, руб"; }
            balance = table.ByXPath("//tr/td[6]/span").Text;//*[@id="j_idt324:j_idt326:j_idt327_data"]/tr/td[6]/span
            return "success";
        }

        public string GoToContract()
        {
            WebElement table = new WebElement().ByXPath("//div[@class='mobile-contracts-list']");
            table.ByXPath("//tr/td[1]/a").Click();
            contractPage = new ContractPage();
            return contractPage.ConstructionPage();
        }


        #endregion

    }
}
