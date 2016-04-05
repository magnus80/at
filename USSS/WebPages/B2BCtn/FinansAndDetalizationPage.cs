using System.Threading;
using System.Windows.Forms;
using AT.WebDriver;
using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.WebPages.CommonPages;


namespace USSS.WebPages.B2BCtn
{
    public class FinansAndDetalizationPage
    {

        RequestHistoryPage requestHistoryPage;

        #region constructor

        // блок заказа детализации
        private WebElement BlockFinInfoWE;
        //ссылка на заказанные детализации
        private WebElement OrderedDetalizationWE;
        //блок Платежи
        private WebElement BlockPayments;
        //выпадающий список периодов
        private WebElement PeriodWE;

        private WebElement BlockSpecialServicesWE;

        private WebElement BlockHelpersWE;

        public string ConstructionPage()
        {
            BlockFinInfoWE = new WebElement().ByXPath("//form[@id='finInfoIndexPage']");
            OrderedDetalizationWE = new WebElement().ByXPath("//a[contains(@onclick,'orderedDetalisation')]");
            BlockPayments = new WebElement().ByXPath("//div[@id='paymentsPanel']");
            BlockSpecialServicesWE = new WebElement().ByXPath("//div[@class='right-col-special']");
            BlockHelpersWE = new WebElement().ByXPath("//div[@class='support-request']//a");

            PeriodWE = new WebElement().ByXPath("//div[contains(@class,'fileds-row')]/div[contains(@id,'periodSelec')]//div[contains(@class,'ui-selectonemenu-trigger ui-state-default ui-corner-right')]");

            if (!BlockFinInfoWE.Displayed) { return "Не отображены элементы интерфейса: блок заказа детализации"; }
            if (!OrderedDetalizationWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на ранее заказанные отчеты"; }
            if (!BlockPayments.Displayed) { return "Не отображены элементы интерфейса: блок платежей"; }
            if (!BlockSpecialServicesWE.Displayed) { return "Не отображены элементы интерфейса: блок полезных сервисов"; }
            if (!BlockHelpersWE.Displayed) { return "Не отображены элементы интерфейса: блок помощи"; }

            if (!PeriodWE.Displayed) { return "Не отображены элементы интерфейса: выпадающий список с перодами"; }

            return "success";
        }

        private void RefreshWE()
        {
            PeriodWE = new WebElement().ByXPath("//div[contains(@class,'fileds-row')]/div[contains(@id,'periodSelec')]//div[contains(@class,'ui-selectonemenu-trigger ui-state-default ui-corner-right')]");
        }


        #endregion

        #region managerPage

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
        
        public string SelectPeriod(String period)
        {
            Thread.Sleep(10000);//ожидание отработки анимации
            RefreshWE();
            if (PeriodWE.Displayed) PeriodWE.Click();
            else { return "Не отображены элементы интерфейса: выпадающий список с перодами"; }
            Thread.Sleep(5000);
            WebElement itemPeriodWE = new WebElement().ByXPath("//div[contains(@class,'ui-selectonemenu-items-wrapper')]//li[contains(text(),'" + period + "')]");
            
            itemPeriodWE.Click();
            if (itemPeriodWE.Text == period) { return "Не отображены элементы интерфейса: выбор периода " + period; }
            return "success";
        }


        public string SaveReport(string formatReport)
        {

            new WebElement().ByXPath("//div[contains(@class,'report-download-button')]/button").Click();
            new WebElement().ByXPath("//input[@value='"+ formatReport +"')]").Click();
            new WebElement().ByXPath("//div[contains(@class,'submit-or-cancel')]/button").Click();
            new WebElement().ByXPath("//a[@href='/c/operations/operationsHistory.xhtml']").Click();
            RequestHistoryPage rhp = new RequestHistoryPage();
            int i = 0;
            while (rhp.ChangeOfStatus("Обработан") != "success")
            {
                i = i + 1;
                new WebElement().ByXPath("//a[contains(@onclick,'update')]").Click();
                Thread.Sleep(5000);
                if (i > 2)
                {
                    return "Ожедание статуса 'Выполнен' более 100 секунд";
                }
            }
            new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//div[contains(@class,'file-link')]/a").Click();
            string path = new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//a[contains(@href,'/c/operations/operationDetail')]").Text + ".zip";
            Thread.Sleep(5000);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(10000);
            return path;
            

        }

        public WebElement GetDetal()
        {
            WebElement det = new WebElement().ByXPath("//div[@class='select-one-button']/a");
            det.Click();
            WebElement next = new WebElement().ByXPath("//div[@id='finInfoIndexPage:callDetailsPanel']//button");
            while (next.Displayed)
            {
                next.Click();
                next = new WebElement().ByXPath("//div[@id='finInfoIndexPage:callDetailsPanel']//button");
            }
            WebElement table = new WebElement().ByXPath("//div[@id='finInfoIndexPage:callDetailsPanel']//tbody[contains(@id,'finInfoIndexPage')]");
            return table;
        }

        public string GetOrderedDetails()
        {
            OrderedDetalizationWE = new WebElement().ByXPath("//a[contains(@onclick,'orderedDetalisation')]");
            if (!OrderedDetalizationWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на ранее заказанные отчеты"; }
            OrderedDetalizationWE.Click();
            requestHistoryPage = new RequestHistoryPage();
            string filtr = requestHistoryPage.CheckFiltr();
            if (filtr != "Заказ детализации")
            {
                new WebElement().ByXPath("//a[contains(@onclick,'finance')]").Click();
                return filtr;
            }
            new WebElement().ByXPath("//a[contains(@onclick,'finance')]").Click();
            return "success";
        }

        public string CheckCalendar()
        {
            SelectPeriod("За другие дни");
            WebElement InputDateWE = new WebElement().ByXPath("//div[contains(@class,'fileds-row')]//div[contains(@id,'calendars')]//input");
            if (!InputDateWE.Displayed) { return "Не отображены элементы интерфейса: период детализации"; }
            InputDateWE.Click();
            Thread.Sleep(5000);
            WebElement DatapickerWE = new WebElement().ByXPath("//div[@class='datepick datepick-multi']");
            if (!DatapickerWE.Displayed) { return "Не отображены элементы интерфейса: Календарь"; }
            WebElement n = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-next  datepick-disabled')]");
            int fuse = 0;
            while (!n.Displayed & fuse < 10)
            {
                fuse = fuse + 1;
                ClickNext();
                n = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-next  datepick-disabled')]");
            }
            int i = 1;
            WebElement d = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-prev  datepick-disabled')]");
            while (!d.Displayed & i < 10)
            {
                i = i + 1;
                ClickPrev();
                d = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-prev  datepick-disabled')]");
            }
            if (i != 9) { return "Глубина календаря не равна 9"; }
            return "success";
        }

        public string CheckOverloadedDetails()
        {
            WebElement message = new WebElement().ByXPath("//h3[contains(text(),'Отчёт получился слишком большим для отображения в личном кабинете. Выберите меньший период или воспользуйтесь детализацией в формате PDF или XLSX.')]");
            if (!message.Displayed) { return "Не отображены элементы интерфейса: Предупреждение о слишком большом размере детализации"; }

            WebElement checkLabelDowl = new WebElement().ByXPath("//label[contains(text(),'Сформировать и скачать файл')]");
            if (!checkLabelDowl.Displayed) { return "Не отображены элементы интерфейса: Надпись у чекбокса Сформировать и скачать файл"; }
            WebElement checkDowl = new WebElement().ByXPath("//input[contains(@id,'tooMuchContainer:saveFile_input')]");
            if (!checkDowl.Displayed) { return "Не отображены элементы интерфейса: Чекбокс Сформировать и скачать файл"; }

            WebElement checkLabelMail = new WebElement().ByXPath("//label[contains(text(),'Отправить по электронной почте'])");
            if (!checkLabelMail.Displayed) { return "Не отображены элементы интерфейса: Надпись у чекбокса Отправить по электронной почте"; }
            WebElement checkMail = new WebElement().ByXPath("//input[contains(@id,'tooMuchContainer:sendEmail_input')]");
            if (!checkMail.Displayed) { return "Не отображены элементы интерфейса: Чекбокс Отправить по электронной почте"; }

            WebElement divMail = new WebElement().ByXPath("//div[contains(text(),'Укажите адрес электронной почты в')]");
            if (!divMail.Displayed) { return "Не отображены элементы интерфейса: Укажите адрес электронной почты в"; }
            WebElement setting = new WebElement().ByXPath("//a[contains(text(),'настройках')]");
            if (!setting.Displayed) { return "Не отображены элементы интерфейса: псевдоссылка на Настойки"; }


            WebElement save = new WebElement().ByXPath("//form[@id='finInfoIndexPage']//button[text(),'Сохранить отчёт']");
            if (!save.Displayed) { return "Не отображены элементы интерфейса: Кнопка Сохранить"; }
            WebElement cancel = new WebElement().ByXPath("//form[@id='finInfoIndexPage']//a[text(),'Отменить']");
            if (!cancel.Displayed) { return "Не отображены элементы интерфейса: ссылка Отмена"; }
         
            return "success";
        }


        public void Download(int startDay, String startMount, String startYear, int endDay, String endMount,
                      String endYear)
        {
            SelectPeriod("За период");
            new WebElement().ByXPath("//div[contains(@class,'submit-or-cancel')]//button").Click();
        }
        #region Calendar

        private String XPATH_DATE_POPUP = "//div[contains(@class,'datepick-popup')]";

        public string SetPeriod(string startDay, string startMount, string startYear, string endDay, string endMount,
                              String endYear)
        {

            new WebElement().ByXPath("//div[contains(@class,'fileds-row')]//div[contains(@id,'calendars')]//input").Click();
            Thread.Sleep(5000);
            WebElement n = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-next  datepick-disabled')]");
            int fuse = 0;
            while (!n.Displayed & fuse < 10)
            {
                fuse = fuse + 1;
                ClickNext();
                n = new WebElement().ByXPath("//a[contains(@class,'datepick-cmd datepick-cmd-next  datepick-disabled')]");
            }
            SetStartDate(startDay, startMount, startYear);
            SetEndDate(endDay, endMount, endYear);
            Thread.Sleep(5000);
            WebElement GetReportWE = new WebElement().ByXPath("//div[contains(@class,'fileds-row')]//div[contains(@class,'refresh-data-button')]//button");
            if (GetReportWE.Displayed)
                if (GetReportWE.Enabled)
                {
                    GetReportWE.Click();
                }
                else { return "Заблокирован элементы интерфейса: кнопка 'Составить отчет' Сообшение:'" + new WebElement().ByXPath("//span[@class='ui-message-error-detail']").Text + "'"; }
            else { return "Не отображены элементы интерфейса: кнопка 'Составить отчет'"; }

            return "success";
        }



        private void SetStartDate(string day, string mount, string year)
        {
            
            String firstDate = GetFirstMount();
            String lastDate = GetLastMount();
            if (lastDate.Contains(year.Trim()))
            {
                if (lastDate.Contains(mount.Trim()))
                {
                    ClickDateMountLast(day);
                }
                else if (firstDate.Contains(year.Trim()))
                {
                    if (firstDate.Contains(mount.Trim()))
                        ClickDateMountFirst(day);
                    else
                    {
                        ClickPrev();
                        SetStartDate(day, mount, year);
                    }
                }
                else
                {
                    ClickPrev();
                    SetStartDate(day, mount, year);
                }
            }
            else if (firstDate.Contains(year.Trim()))
            {
                if (GetLastMount().Contains(mount.Trim()))
                {
                    ClickDateMountFirst(day);
                }
                else
                {
                    ClickPrev();
                    SetStartDate(day, mount, year);
                }
            }
            else
            {
                ClickPrev();
                SetStartDate(day, mount, year);
            }
        }

        private void SetEndDate(string day, string mount, string year)
        {
            String firstDate = GetFirstMount();
            String lastDate = GetLastMount();
            if (firstDate.Contains(year.Trim()))
            {
                if (firstDate.Contains(mount.Trim()))
                    ClickDateMountFirst(day);
                else if (lastDate.Contains(year.Trim()))
                {
                    if (lastDate.Contains(mount.Trim()))
                        ClickDateMountLast(day);
                    else
                    {
                        ClickNext();
                        SetEndDate(day, mount, year);
                    }
                }
            }
            else if (lastDate.Contains(year.Trim()))
            {
                if (lastDate.Contains(mount.Trim()))
                    ClickDateMountLast(day);
                else
                {
                    ClickNext();
                    SetEndDate(day, mount, year);
                }
            }
            else
            {
                ClickNext();
                SetEndDate(day, mount, year);
            }
        }

        private void ClickDateMountFirst(string day)
        {
            new WebElement().ByXPath(XPATH_DATE_POPUP +
                                     "//div[contains(@class,'datepick-month first')]//table//a[text() = '" + day + "']")
                .Click();
        }

        private void ClickDateMountLast(string day)
        {
            new WebElement().ByXPath(XPATH_DATE_POPUP +
                                     "//div[contains(@class,'datepick-month last')]//table//a[text() = '" + day + "']").
                Click();
        }

        private void ClickPrev()
        {
            new WebElement().ByXPath("//a[contains(@class,'datepick-cmd-prev')]").Click();
        }

        private void ClickNext()
        {
            new WebElement().ByXPath("//a[contains(@class,'datepick-cmd-next')]").Click();
        }

        private String GetFirstMount()
        {
            return
                new WebElement().ByXPath(XPATH_DATE_POPUP +
                                         "//div[contains(@class,'datepick-month first')]//div[contains(@class,'datepick-month-header')]")
                    .Text;
        }

        private String GetLastMount()
        {
            return
                new WebElement().ByXPath(XPATH_DATE_POPUP +
                                         "//div[contains(@class,'datepick-month last')]//div[contains(@class,'datepick-month-header')]")
                    .Text;
        }

        #endregion

        #endregion

    }
}

