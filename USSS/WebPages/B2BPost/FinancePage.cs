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
    class FinancePage
    {
        private WebElement weTable;
        private WebElement weTab;
        private WebElement weHeader;
        public string ConstructionPage()
        {
            weTable = new WebElement().ByXPath("//div[@id = 'paid-invoice-form:paidInvoicedataTable']");
            if (!weTable.Displayed) { return "Не отображены элементы интерфейса: Таблица неоплаченные счета"; }

            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/span[text()='Счета']");
          
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Счета"; }

            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/span[text()='Неоплаченные']");
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Неоплаченные"; }

            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/a[text()='Оплаченные']");
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Оплаченные"; }

            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/a[text()='Все счета']");
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Все счета"; }

            weHeader = new WebElement().ByXPath("//div[@class='wide-header-inner']/h2[text()='Финансовая информация']");
            if (!weHeader.Displayed) { return "Не отображены элементы интерфейса: Хедер Финансовая информация"; }

            
            return "success";
        }

        public string SelectAllBills()
        {
            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/a[text()='Все счета']");
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Все счета"; }
            weTab.Click();

            return CheckAllBillsTable();
        }
       
        public string CheckAllBillsTable()
        {
            Thread.Sleep(500);
            weTab = new WebElement().ByXPath("//div[@class='select-one-button']/span[text()='Все счета']");
            if (!weTab.Displayed) { return "Не отображены элементы интерфейса: Вкладка Все счета активна после выбора или не найдена"; }

            weTable = new WebElement().ByXPath("//tbody[@id='paid-invoice-form:paidInvoicedataTable_data']");
            if (!weTable.Displayed) { return "Не отображены элементы интерфейса: Таблица с счетами"; }

            weTable = new WebElement().ByXPath("//tbody[@id='paid-invoice-form:paidInvoicedataTable_data']/tr[@data-ri='0']");
           
            if (!weTable.Displayed) { return "Не отображены элементы интерфейса: В таблице нет данных"; }

            weTable = new WebElement().ByXPath("//tbody[@id='paid-invoice-form:paidInvoicedataTable_data']/tr[@data-ri='0']/td[3]/a");
            if (!weTable.Displayed) { return "Не отображены элементы интерфейса: В таблице нет ссылки на счет."; }

            return "success";
        }
        
        public string ClickOnBill()
        {
            weTable = new WebElement().ByXPath("//tbody[@id='paid-invoice-form:paidInvoicedataTable_data']/tr[@data-ri='0']/td[3]/a");
            if (!weTable.Displayed) { return "Не отображены элементы интерфейса: В таблице нет ссылки на счет."; }
            weTable.Click();

            return "success";
        }

        public string CheckBillPage()
        {
            WebElement we;

            we = new WebElement().ByXPath("//div[@class='wide-header-inner']/h2[contains(text(),'Счёт за мобильную связь №')]");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Хедер Счёт за мобильную связь №"; }

            we = new WebElement().ByXPath("//div[@class='subheader']/a[contains(@href,'/b/info/contractDetail.xhtml?objId=')]");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: ссылка на номер договора."; }

            we = new WebElement().ByXPath("//div[@class='subheader']/a[contains(@href,'/b/info/blockNumbersDetail.xhtml?objId=')]");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: ссылка на группу счетов "; }

            we = new WebElement().ByXPath("//div[contains(text(),'Дата выставления счёта')]");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Дата выставления счёта "; }

            we = new WebElement().ByXPath("//div[contains(text(),'Период счёта')]");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Период счёта "; }

            we = new WebElement().ByXPath("//div[@class='bill-total']/b");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Сумма счета "; }

            we = new WebElement().ByXPath("//li[@class='xls']/a[text()='Заказать отчёт по детализации']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Заказать отчёт по детализации "; }

            we = new WebElement().ByXPath("//li[@class='pdf']/a[text()='Заказать PDF-версию']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Заказать отчёт по детализации "; }

            we = new WebElement().ByXPath("//div[@class='bill-status-paid']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Статус счета "; }

            we = new WebElement().ByXPath("//div[@class='address-outline']/div/div[@class='address-padding']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Статус счета "; }

            we = new WebElement().ByXPath("//table[@class='bill-expences']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Структура расходов"; }
            
            //TODO : Выпадающий спиписок - структура расходов по пользователям
            return "success";
        }

        public string ClickOrderXLSReport()
        {
            

            var we = new WebElement().ByXPath("//li[@class='xls']/a[text()='Заказать отчёт по детализации']");
            if (!we.Displayed) { return "Не отображены элементы интерфейса: Заказать отчёт по детализации "; }
            we.Click();

            return "success";
        }
    }
}
