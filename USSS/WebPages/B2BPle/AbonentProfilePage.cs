using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT;
using AT.WebDriver;
using System.Threading;

namespace USSS.WebPages.B2BPle
{
    class AbonentProfilePage: PageBase
    {
        #region constructor

        // поле ввода логина
        private WebElement AbonentInfoWE;
        private WebElement DetalizationWE;
        private WebElement ServicesWE;
        private WebElement ReportsWE;
        private WebElement PaymentsWE;


        public string ConstructionPage()
        {
            AbonentInfoWE = new WebElement().ByXPath("//div[@id='contactInformation']");
            DetalizationWE = new WebElement().ByXPath("//div[@id='pleCallDetalizationForm']");
            ServicesWE = new WebElement().ByXPath("//div[@id='manageServs']");
            ReportsWE = new WebElement().ByXPath("//form[@id='detailsTableForm']");
            PaymentsWE = new WebElement().ByXPath("//form[@id='payments:payments']");

            if (!AbonentInfoWE.Displayed) { return "Не отображены элементы интерфейса: Информация об абоненте"; }
            if (!DetalizationWE.Displayed) { return "Не отображены элементы интерфейса: Блок Деталицации"; }
            if (!ServicesWE.Displayed) { return "Не отображены элементы интерфейса: Блок Подключенные услуги"; }
            if (!ReportsWE.Displayed) { return "Не отображены элементы интерфейса: Блок Отчеты"; }
            if (!PaymentsWE.Displayed) { return "Не отображены элементы интерфейса: Блок Платежи"; }

            return "success";
        }

        #endregion

        #region managerPage

        public string SelectPeriod(string namePeriod)
        {
            WebElement periodWE = new WebElement().ByXPath("//a[text()='" + namePeriod + "']");
            if (periodWE.Displayed)
            {
                periodWE.Click();
            }
            else
            {
                periodWE = new WebElement().ByXPath("//span[text()='" + namePeriod + "']");
                if(!periodWE.Displayed)
                {
                    return "Не отображается период" + namePeriod;
                }
            }
            return "success";
        }

        public string CheckPeriod(string namePeriod)
        {
            WebElement periodWE = new WebElement().ByXPath("//a[text()='" + namePeriod + "']");
            if (periodWE.Displayed)
            {
                return "Не задан период " + namePeriod;
            }
            else
            {
                periodWE = new WebElement().ByXPath("//span[text()='" + namePeriod + "']");
                if (!periodWE.Displayed)
                {
                    return "Не отображается период" + namePeriod;
                }
            }
            return "success";
        }

        public string SelectTypeDetalization(string nameType)
        {
            WebElement typeWE = new WebElement().ByXPath("//div[contains(@id,'formatSelect')]//div[contains(@class,'ui-selectonemenu-trigger ui-state-default ui-corner-right')]");
            if (typeWE.Displayed)
            {
                typeWE.Click();
            }
            else
            {
                    return "Не отображается выбор формата детализации";
            }
            WebElement type = new WebElement().ByXPath("//li[@data-label='"+nameType+"']");
            if (type.Displayed)
            {
                type.Click();
            }
            else
            {
                return "Не отображается формат " + nameType;
            }
            Thread.Sleep(5000);
            return "success";
        }

        public string CheckDatePeriod(string namePeriod)
        {
            WebElement periodWE = new WebElement().ByXPath("//a[text()='" + namePeriod + "']");
            if (periodWE.Displayed)
            {
                periodWE.Click();
            }
            else
            {
                periodWE = new WebElement().ByXPath("//span[text()='" + namePeriod + "']");
                if (!periodWE.Displayed)
                {
                    return "Не отображается период" + namePeriod;
                }
            }
            Thread.Sleep(7000);
            if ( namePeriod =="День")
            {
                
                string date = new WebElement().ByXPath("//input[contains(@id,'minDatePeriod_input')]").Value;
                if (date != DateTime.Now.ToString("dd.MM.yy"))
                {
                    return "Дата некорректна";
                }
            }

            if (namePeriod == "Неделя")
            {
               
                string date = new WebElement().ByXPath("//input[contains(@id,'minDatePeriod_input')]").Value;
                if (date != DateTime.Now.AddDays(-6).ToString("dd.MM.yy"))
                {
                    return "Дата некорректна";
                }
            }

            if (namePeriod == "Две недели")
            {
               
                string date = new WebElement().ByXPath("//input[contains(@id,'minDatePeriod_input')]").Value;
                if (date != DateTime.Now.AddDays(-13).ToString("dd.MM.yy"))
                {
                    return "Дата некорректна";
                }
            }

            if (namePeriod == "Месяц")
            {
               
                string date = new WebElement().ByXPath("//input[contains(@id,'minDatePeriod_input')]").Value;
                if (date != DateTime.Now.AddDays(-30).ToString("dd.MM.yy"))
                {
                    return "Дата некорректна";
                }
               
            }
            return "success";
        }

        public string CheckFormatDetalization(string startDay, string startMount, string startYear, string endDay, string endMount,
                              string endYear)
        {
            SetDatePeriod(startDay, startMount, startYear, endDay, endMount, endYear);

            WebElement btnOrderDetalization = new WebElement().ByXPath("//button[contains(@id,'pleCallDetalizationForm')]");
            if (!btnOrderDetalization.Displayed) { return "Не отображается кнопка заказа детализации"; }
            btnOrderDetalization.Click();
            Thread.Sleep(100000);
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(10000);
                if (new WebElement().ByXPath("//span[text()='Сервис']").Displayed) break;
            }
            if (!new WebElement().ByXPath("//span[text()='Сервис']").Displayed) { return "Не отображается колонка Сервис";}
            if (!new WebElement().ByXPath("//span[text()='Дата и время']").Displayed) { return "Не отображается колонка Дата и время"; }
            if (!new WebElement().ByXPath("//span[text()='Номер телефона']").Displayed) { return "Не отображается колонка Номер телефона"; }
            if (!new WebElement().ByXPath("//span[text()='Мобильный оператор']").Displayed) { return "Не отображается колонка Мобильный оператор"; }
            if (!new WebElement().ByXPath("//span[text()='Объём услуг']").Displayed) { return "Не отображается колонка Объём услуг"; }
            if (!new WebElement().ByXPath("//span[text()='Баланс']").Displayed) { return "Не отображается колонка Баланс"; }
            if (!new WebElement().ByXPath("//span[text()='Изменение баланса']").Displayed) { return "Не отображается колонка Изменение баланса"; }
            if (!new WebElement().ByXPath("//span[text()='Конечный баланс']").Displayed) { return "Не отображается колонка Конечный баланс"; }

            return "success";
        }

        #endregion

        #region Calendar

        private String XPATH_DATE_POPUP = "//div[contains(@id,'ui-datepicker-div')]";

        public string SetDatePeriod(string startDay, string startMount, string startYear, string endDay, string endMount,
                              string endYear)
        {

            new WebElement().ByXPath("//input[contains(@id,'minDatePeriod_input')]").Click();
            Thread.Sleep(5000);
            WebElement n = new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next ui-corner-all ui-state-disabled')]");
            int fuse = 0;
            while (!n.Displayed & fuse < 10)
            {
                fuse = fuse + 1;
                ClickNext();
                Thread.Sleep(1000);
                n = new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next ui-corner-all ui-state-disabled')]");
            }
            SetStartDate(startDay, startMount, startYear);

            new WebElement().ByXPath("//input[contains(@id,'maxDatePeriod_input')]").Click();
            n = new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next ui-corner-all ui-state-disabled')]");
            fuse = 0;
            while (!n.Displayed & fuse < 10)
            {
                fuse = fuse + 1;
                ClickNext();
                Thread.Sleep(1000);
                n = new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next ui-corner-all ui-state-disabled')]");
            }
            Thread.Sleep(5000);
            SetEndDate(endDay, endMount, endYear);
            Thread.Sleep(5000);
           
            Thread.Sleep(10000);
            return "success";
        }

        private void SetStartDate(string day, string mount, string year)
        {
            String firstDate = GetMount();

            if (firstDate.Contains(mount.Trim()))
                ClickDateMount(day);
            else
            {
                ClickPrev();
                SetStartDate(day, mount, year);
            }
        }

        private void SetEndDate(string day, string mount, string year)
        {
            String firstDate = GetMount();

            if (firstDate.Contains(mount.Trim()))
                ClickDateMount(day);
            else
            {

                ClickPrev();
                SetEndDate(day, mount, year);
            }
        }

        private void ClickDateMount(string day)
        {
            new WebElement().ByXPath(XPATH_DATE_POPUP +
                                     "//table//a[text() = '" + day + "']")
                .Click();
        }


        private void ClickPrev()
        {
            new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-prev')]").Click();
        }

        private void ClickNext()
        {
            new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next')]").Click();
        }

        private String GetMount()
        {
            return
                new WebElement().ByXPath(XPATH_DATE_POPUP +
                                         "//select[contains(@class,'ui-datepicker-month')]//option[contains(@selected,'selected')]")
                    .Text;
        }

        #endregion
    }
}
