using System.IO;
using System.Threading;
using System.Windows.Forms;
using AT.WebDriver;
using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;


namespace USSS.WebPages.B2CPost
{
    internal class FinansAndDetalizationPage
    {


        #region constructor


        #region navigation WE

        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        private WebElement InvoiceWE;
        private WebElement PaymentWE;
        private WebElement ExportInvoiceWE;
        private WebElement MakeReportWE;
        private WebElement AllPaymentsWE;
        private WebElement ExportPaymentsWE;
        //ссылка на уведомления в навигации
      

        #endregion



        public string ConstructionPage()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id = 'postProfile']");
            InvoiceWE = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget invoices'][1]");
            PaymentWE = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget payments']");
            ExportInvoiceWE = InvoiceWE.ByXPath("//span[contains(text(),'Выгрузить в Excel')]");
            MakeReportWE = new WebElement().ByXPath("//span[contains(text(),'Составить отчет')]");
            AllPaymentsWE = new WebElement().ByXPath("//span[contains(text(),'Показать все платежи')]");

            if (!ProfileWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на профиль"; }
            if (!InvoiceWE.Displayed) { return "Не отображены элементы интерфейса: блок Счета"; }
            if (!PaymentWE.Displayed) { return "Не отображены элементы интерфейса: блок Платежи"; }
            if (!ExportInvoiceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Выгрузить счета"; }
            if (!MakeReportWE.Displayed) { return "Не отображены элементы интерфейса: кнопка Составить отчет"; }
            if (!AllPaymentsWE.Displayed) { return "Не отображены элементы интерфейса: кнопка Все платежи"; }

            AllPaymentsWE.Click();
            ExportPaymentsWE = PaymentWE.ByXPath("//span[contains(text(),'Выгрузить в Excel')]");
            if (!ExportPaymentsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Выгрузить платежи"; }
            return "success";
        }

        #endregion

        public string ConstructionPageDiagramm()
        {
            WebElement we;

            we = new WebElement().ByXPath("//h1[contains(text(),'Финансы и детализация')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: хедер Финансы и детализация";
            }

            we = new WebElement().ByXPath("//div[@id='reportPanel']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: блок детализации.";
            }

            we =
                new WebElement().ByXPath(
                    "//div[@class='actions block-padding']/button[@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only big big-bold']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: кнопка Обновить отчет.";
            }

            we = new WebElement().ByXPath("//a[@class='ui-commandlink ui-widget xls-link']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: ссылка Выгрузить в EXCEL";
            }

            we = new WebElement().ByXPath("//canvas[@class='jqplot-event-canvas']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Диаграмма";
            }



            return "success";
        }

        public string CheckPayments(int number)
        {
            WebElement paymentOne = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//tbody[1]/tr[@data-ri='" + number + "']/td[1]/a");
            if (!paymentOne.Displayed)  { return "Отсутвуют платежи"; }
            paymentOne.Click();
            Thread.Sleep(10000);
            WebElement invoice = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]//th[1]/span");//td[@class='added-td data"+number+"']//thead/tr[1]//th[1]/span");
            if (!invoice.Text.Contains("Номер счёта")) { return "Отсутвуют номер счета"; }
            WebElement dateTo = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]/th[2]/span");
            if (!dateTo.Text.Contains("Оплатить до")) { return "Отсутвуют Оплатить до"; }
            WebElement total = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]/th[3]/span");
            if (!total.Text.Contains("Всего по счёту с НДС, руб.")) { return "Отсутвуют Всего по счёту с НДС, руб."; }
            WebElement amount = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]/th[4]/span");
            if (!amount.Text.Contains("Зачислено, руб. (из платежа)")) { return "Отсутвуют Зачислено, руб. (из платежа)"; }
            WebElement notAmount = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]/th[5]/span");
            if (!notAmount.Text.Contains("Не оплачено")) { return "Отсутвуют Не оплачено"; }
            WebElement status = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:" + number + ":allocationsTable')]//thead/tr[1]/th[6]/span");
            if (!status.Text.Contains("Статус счёта")) { return "Отсутвуют Статус счёта"; }
            return "success";
        }


        public string CheckPayments(string dbAns, string ban)
        {
            var q = "select logical_date from logical_date@" + dbAns + @" where rownum<2";
            var qm = Executor.ExecuteSelect(q);
            string dateF = (Convert.ToDateTime(qm[0, 0]).AddMonths(-3).AddDays(-1)).ToShortDateString().ToString();

            var t = @"select actv_amt from payment_activity@" + dbAns + @" pa
                        left join payment_type@" +dbAns+@" pt on pa.actv_reason_code = pt.pym_type
                        where ban = '"+ban+@"'
                        and actv_date > to_date('"+dateF+@"','dd.mm.yyyy')
                        order by actv_amt";
            var tm = Executor.ExecuteSelect(t);
            int i = 0;
            while (i < tm.Count)
            {
                try
                {
                    string cost = new WebElement().ByXPath("//tbody[contains(@id,'payments_table_data')]/tr[@data-ri='" + i + "']/td[3]").Text;
                    if (Convert.ToDouble(tm[i, 0]) != Convert.ToDouble(cost))
                    {
                        return "Платежи отображены некорректно";
                    }
                    i++;
                }
                catch (Exception)
                {

                    return "Платежи отображены некорректно";
                }

            }
            if (new WebElement().ByXPath("//div[conteins(@id,'payments_table')]//tr[@data-ri='" + i + "']//td[3]").Displayed)
            {
                return "Платежи отображены некорректно";
            }

            return "success";
           
        }

        public string CheckDefaulFilter(string db_Ans)
        {
            WebElement calendar = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[@class='calendar-filter']/b");
            string date = calendar.Text;

            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();

            string dateF = (Convert.ToDateTime(tm[0, 0]).AddDays(-90)).ToShortDateString().ToString();
            dateF = dateF + " – " + dateB;
            if (date != dateF) return "Фильтр по времени некорректен";
            WebElement type = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]/label");
            string typeDef = type.Text;
            if (typeDef != "Все каналы") return "Фильтр по статусу не верен";
            return "success";
        }

        public string CheckUnRezultFilter()
        {
            if (new WebElement().ByXPath("//td[contains(text(),'Наличный платеж')][1]").Displayed)
            {
                new WebElement().ByXPath(
                    "//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//div[@class='ui-selectonemenu-trigger ui-state-default ui-corner-right']")
                    .Click();
                Thread.Sleep(1000);
                new WebElement().ByXPath("//div[contains(@id,'payments_table')]//li[@data-label='Наличный платеж']").
                    Click();
                Thread.Sleep(1000);
                if (new WebElement().ByXPath("//td[text()='Платежи отсутствуют']").Displayed)
                {
                    return "Фильтр по статусу некорректен";
                }
                int i = 0;
                WebElement st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                while (st.Displayed)
                {
                    if (st.Text != "Наличный платеж")
                    {
                        return "Фильтр по статусу некорректен";
                    }
                    i++;
                    st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                }
            }
            else
            {
                if (new WebElement().ByXPath("//td[contains(text(),'Кредитная карта')][1]").Displayed)
                {
                    new WebElement().ByXPath(
                        "//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//div[@class='ui-selectonemenu-trigger ui-state-default ui-corner-right']")
                        .Click();
                    Thread.Sleep(1000);
                    new WebElement().ByXPath("//div[contains(@id,'payments_table')]//li[@data-label='Кредитная карта']").
                        Click();
                    Thread.Sleep(1000);
                    if (new WebElement().ByXPath("//td[text()='Платежи отсутствуют']").Displayed)
                    {
                        return "Фильтр по статусу некорректен";
                    }
                    int i = 0;
                    WebElement st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                    while (st.Displayed)
                    {
                        if (st.Text != "Кредитная карта")
                        {
                            return "Фильтр по статусу некорректен";
                        }
                        i++;
                        st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                    }
                }
                else
                {
                    if (new WebElement().ByXPath("//td[contains(text(),'Безналичный платеж')][1]").Displayed)
                    {
                        new WebElement().ByXPath(
                            "//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//div[@class='ui-selectonemenu-trigger ui-state-default ui-corner-right']")
                            .Click();
                        Thread.Sleep(1000);
                        new WebElement().ByXPath("//div[contains(@id,'payments_table')]//li[@data-label='Безналичный платеж']").
                            Click();
                        Thread.Sleep(1000);
                        if (new WebElement().ByXPath("//td[text()='Платежи отсутствуют']").Displayed)
                        {
                            return "Фильтр по статусу некорректен";
                        }
                    }
                    int i = 0;
                    WebElement st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                    while (st.Displayed)
                    {
                        if (st.Text != "Безналичный платеж")
                        {
                            return "Фильтр по статусу некорректен";
                        }
                        i++;
                        st = new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[contains(@id,'payments_table:paymentType')]//tr[@data-ri='" + i + "']/td[2]");
                    }

                }
            }

            WebElement calendar =
                new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[@class='calendar-filter']/b");
            calendar.Click();
            WebElement dateTo =
                new WebElement().ByXPath("//input[contains(@id,'payments_table:filterTripDateTo_input')]");
            dateTo.Click();
            Thread.Sleep(1000);
            WebElement f = new WebElement().ByXPath("//a[@class='ui-datepicker-prev ui-corner-all']");
            if (!f.Displayed)
            {
                Console.WriteLine("");//ХЗ зачем оно надо, но работает только так(магия)
            }
            f.Click();
            Thread.Sleep(5000);
            f = new WebElement().ByXPath("//span[@class='ui-icon ui-icon-circle-triangle-w']");
            if (!f.Displayed)
            {
                Console.WriteLine("");
            }
            f.Click();
            Thread.Sleep(5000);
            f = new WebElement().ByXPath("//span[@class='ui-icon ui-icon-circle-triangle-w']");
            if (!f.Displayed)
            {
                Console.WriteLine("");
            }
            f.Click();
            Thread.Sleep(1000);
            WebElement d =
                new WebElement().ByXPath(
                    "//table[@class='ui-datepicker-calendar']//td[not(contains(@class,' ui-datepicker-unselectable ui-state-disabled '))][1]/a");
            d.Click();
            Thread.Sleep(1000);
            if (!new WebElement().ByXPath("//td[text()='Платежи отсутствуют']").Displayed){  return "Фильтр по времени некорректен"; }
            return "success";
        }

        public string CheckDateFilterValid()
        {
            WebElement calendar =
               new WebElement().ByXPath("//div[contains(@id,'payments_table')]//div[@class='calendar-filter']/b");
            calendar.Click();
            WebElement dateFrom =
                new WebElement().ByXPath("//input[contains(@id,'payments_table:filterTripDateFrom_input')]");
            dateFrom.Click();
            WebElement d =
              new WebElement().ByXPath(
                  "//table[@class='ui-datepicker-calendar']//td[not(contains(@class,' ui-datepicker-unselectable ui-state-disabled '))][2]/a");
            if (d.Displayed) return "Фильтр по времени некорректен";
            Thread.Sleep(1000);
            return "success";
        }

        public string ClickAllPayments(string dbAns)
        {
            AllPaymentsWE = new WebElement().ByXPath("//span[contains(text(),'Показать все платежи')]");
            if (!AllPaymentsWE.Displayed) { return "Не отображены элементы интерфейса: кнопка Все платежи"; }
            AllPaymentsWE.Click();
            string rezult;
            rezult = CheckPayments(0);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                return rezult;
            }
            rezult = CheckPayments(1);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                return rezult;
            }
            rezult = CheckPayments(2);
            if (rezult != "success" & rezult != "Отсутвуют платежи")
            {
                return rezult;
            }
            return CheckDefaulFilter(dbAns);
        }

        public string ClickExportPayments(string temp)
        {
            ExportPaymentsWE = PaymentWE.ByXPath("//span[contains(text(),'Выгрузить в Excel')]");
            if (!ExportPaymentsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Выгрузить платежи"; }
            ExportPaymentsWE.Click();
            DateTime d = DateTime.Now;
            Thread.Sleep(2000);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);

            string path = temp;
            string[] files = Directory.GetFiles(path, "schet_*");
            if (files.Length == 0)
            {
                return "Файл не скачен";
            }

            System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
            int count = local_procs.Count(p => p.ProcessName == "EXCEL");
            try
            {
                for (int i = 0; i < count; i++)
                {
                    System.Diagnostics.Process target_proc = (local_procs.Where(p => p.ProcessName == "EXCEL").ToList())[i];
                    target_proc.Kill();
                }
            }
            catch (InvalidOperationException)
            {
            }
            return "success";
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

        public void SaveReport(string formatReport)
        {
            new WebElement().ByXPath("//div[contains(@class,'report-download-button')]/button").Click();
            WebElementTable wb =  new WebElementTable();
            wb.GetAllData("//div[contains(@id,'billsForm:b2cpostbillstable')]//div[@class = 'ui-datatable-tablewrapper']//table");
            new WebElement().ByXPath("//input[contains(@value, '"+ formatReport +"')]").Click();
            new WebElement().ByXPath("//div[contains(@class,'submit-or-cancel')]/button").Click();

            new WebElement().ByXPath("//a[contains(text(),'Отчет ')]").Click();
            Thread.Sleep(5000);
            SendKeys.SendWait("{ENTER}");
        }

        public void SavePayments()
        {
            new WebElement().ByXPath("//span[contains(text(),'Показать все платежи')]").Click();
            new WebElement().ByXPath("//span[contains(text(),'Выгрузить в Excel')]").Click();
            //new WebElement().ByXPath("//input[contains(@value, '" + formatReport + "')]").Click();
            //new WebElement().ByXPath("//div[contains(@class,'submit-or-cancel')]/button").Click();

            //new WebElement().ByXPath("//a[contains(text(),'Отчет ')]").Click();
            //Thread.Sleep(5000);
            //SendKeys.SendWait("{ENTER}");
        }

        public void GetReportCurrentPeriod()
        {
            new WebElement().ByXPath("//a[contains(#href,'/c/post/fininfo/report/charge.html')]").Click();
            new WebElement().ByXPath("//a[contains(#id,'exportReportButton')]").Click();
        }

        #region Calendar

        private String XPATH_DATE_POPUP = "//div[contains(@id,'ui-datepicker-div')]";

        public void SetPeriod(int startDay, String startMount, String startYear, int endDay, String endMount,String endYear)
        {
            new WebElement().ByXPath("//input[contains(@id,'startDate_input')]").Click();
            SetStartDate(startDay, startMount, startYear);
            new WebElement().ByXPath("//input[contains(@id,'endDate_input')]").Click();
            SetEndDate(endDay, endMount, endYear);
            new WebElement().ByXPath("//form[contains(@id, 'billsForm')]//button").Click();        
        }

        private void SetStartDate(int day, String mount, String year)
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

        private void SetEndDate(int day, String mount, String year)
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

        private void ClickDateMountFirst(int day)
        {
            new WebElement().ByXPath(XPATH_DATE_POPUP +
                                     "//table//a[text() = '" + day + "']")
                .Click();
        }

        private void ClickDateMountLast(int day)
        {
            new WebElement().ByXPath(XPATH_DATE_POPUP +
                                     "//table//a[text() = '" + day + "']").
                Click();
        }

        private void ClickPrev()
        {
            new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-prev ui-corner-all')]").Click();
        }

        private void ClickNext()
        {
            new WebElement().ByXPath("//a[contains(@class,'ui-datepicker-next ui-corner-all')]").Click();
        }

        private String GetFirstMount()
        {
            return
                new WebElement().ByXPath(XPATH_DATE_POPUP +
                                         "//div[contains(@class,'ui-datepicker-title')]")
                    .Text;
        }

        private String GetLastMount()
        {
            return
                new WebElement().ByXPath(XPATH_DATE_POPUP +
                                         "//div[contains(@class,'ui-datepicker-title')]").Text;
        }


        #endregion

        public string CheckOperationsHistoryTab()
        {
            Thread.Sleep(1000);
            WebElement we;
            //  we = new WebElement().ByXPath("//div[@class='actions block-padding']/button[@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only big big-bold']");
            //   if (!we.Displayed) { return "Не отображен элемент: кнопка Обновить отчет."; }

            we = new WebElement().ByXPath("//div[@class='highcharts-container']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: график";
            }

            //   we = new WebElement().ByXPath("//div[@class='popup filters-popup']");
            //   if (!we.Displayed) { return "Не отображен элемент: фильтры"; }

            we = new WebElement().ByXPath("//div[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: таблицы с детализацией под графиком";
            }

            return "success";
        }

        public string GoToRequestHistoryTab()
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//div[@class='select-one-button']/a/span[contains(text(),'История операций')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Вкладка история операций";
            }
            we.Click();

            return CheckOperationsHistoryTab();

        }

        public string CheckSortTable()
        {
            WebElement weTableElement, we;
            string TableElement, TableElementCompare;

            we = new WebElement().ByXPath("//span[contains(text(),'Дата звонка')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Дата звонка";
            }
            //   we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[1]");
            TableElement = weTableElement.Text;

            //   Thread.Sleep(1500);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[1]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (1)";

            we = new WebElement().ByXPath("//span[contains(text(),'Время звонка')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Время звонка";
            }


            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[2]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[2]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (2)";





            we = new WebElement().ByXPath("//span[contains(text(),'Инициатор звонка')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Инициатор звонка";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[3]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[3]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (3)";





            we = new WebElement().ByXPath("//span[contains(text(),'Набранный номер')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Набранный номер";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[4]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[4]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (4)";

            we = new WebElement().ByXPath("//span[contains(text(),'Тип звонка')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Тип звонка";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[5]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[5]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (5)";

            we = new WebElement().ByXPath("//span[contains(text(),'Услуга')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Услуга";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[6]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[6]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (6)";

            we = new WebElement().ByXPath("//span[contains(text(),'Предварительная стоимость (без НДС)')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Предварительная стоимость (без НДС)";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[7]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[7]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (7)";

            we = new WebElement().ByXPath("//span[contains(text(),'Продолжительность')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Продолжительность";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[8]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[8]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (8)";

            we = new WebElement().ByXPath("//span[contains(text(),'Объем (MB)')]");
            if (!we.Displayed)
            {
                return "Не отображен столбец таблицы Объем (MB)";
            }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[9]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                "//tbody[@id='unbilledLoader:reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[9]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (9)";



            return "success";

        }


        public string DownloadUnbilledDetails()
        {
            WebElement WEDownloadXLSReportLink = new WebElement().ByXPath("//div[@id='xlsUnbilledExporter']/a");
            if (!WEDownloadXLSReportLink.Displayed) return "Не найдена псевдоссылка Выгрузить в Excel";

            WEDownloadXLSReportLink.Click();

            Thread.Sleep(1500);
            SendKeys.SendWait("{DOWN}");
            Thread.Sleep(200);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            string path = Environment.GetEnvironmentVariable("HOMEPATH") + "\\Downloads";
            Directory.SetCurrentDirectory(path);


            path = Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(path, "call_details_unbilled.xls");

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

        public string SelectPeriodDetails()
        {
            WebElement we;
            we =
                new WebElement().ByXPath(
                    "//div[@class='ui-selectonemenu-trigger ui-state-default ui-corner-right']/span");
            if (!we.Displayed)
            {
                return "Не отображен селектор периода";
            }
            we.Click();

            we = new WebElement().ByXPath("//div[@class='ui-selectonemenu-items-wrapper']/ul/li[2]");
            if (!we.Displayed)
            {
                return "Нет доступных периодов";
            }
            we.Click();

            //ui-selectonemenu-trigger ui-state-default ui-corner-right
            return CheckOperationsHistoryTabWithPeriod();
        }

        public string CheckOperationsHistoryTabWithPeriod()
        {
            Thread.Sleep(1000);
            WebElement we;

            we =
                new WebElement().ByXPath(
                    "//div[@class='actions block-padding']/button[@class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only big big-bold']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: кнопка Обновить отчет.";
            }

            we = new WebElement().ByXPath("//div[@class='highcharts-container']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: график";
            }

            we = new WebElement().ByXPath("//div[@class='popup filters-popup']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: фильтры";
            }

            we = new WebElement().ByXPath("//div[@id='b2cPostCallDetailPanel']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: таблицы с детализацией под графиком";
            }

            return "success";
        }

        public string CheckBillTable()
        {
            WebElement we;

            we = new WebElement().ByXPath("//thead[contains(@id,'billsForm:b2cpostbillstable_head')]/tr[2]");

            if (!we.Displayed)
            {
                return "Не отображен элемент: таблица со счетом за текущий период";
            }

            we = new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: таблица со счетами!";
            }


            int i;
            for (i = 1; i <= 100; i++)
            {
                we = new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[" + i + "]");
                if (!we.Displayed) break;
            }
            i--;

            for (int j = 0; j < i; j++)
            {
                we =
                    new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td/div[@class='ui-chkbox ui-widget']");
                if (!we.Displayed)
                {
                    return "Не отображен элемент: Чекбокс у счета";
                }

                we =
                    new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[2]/div[@class='bill-date']");
                if (!we.Displayed)
                {
                    return "Не отображен элемент: Дата выставление счета";
                }
                string billPeriod = we.Text;
                string[] Split = billPeriod.Split(new Char[] { '—' });


                we = new WebElement().ByXPath("//input[contains(@id,'startDate_input')]");
                if (!we.Displayed) return "Не найден элемент: Селектор периода С";
                //string dateFrom = we.Value;
                //dateFrom = dateFrom.Replace(dateFrom.Substring(dateFrom.IndexOf(".", 3) + 1, 2),
                //                            "20" + dateFrom.Substring(dateFrom.IndexOf(".", 3) + 1, 2));

                DateTime SelectorPeriodFrom =
                    Convert.ToDateTime(
                        we.Value.Replace(we.Value.Substring(we.Value.IndexOf(".", 3) + 1, 2),
                                         "20" + we.Value.Substring(we.Value.IndexOf(".", 3) + 1, 2)).Replace(".", "/"));
                //dateFrom = dateFrom.Replace(".", "/");
                we = new WebElement().ByXPath("//input[contains(@id,'endDate_input')]");
                if (!we.Displayed) return "Не найден элемент: Селектор периода ПО";
                // string dateTo = we.Value;
                DateTime SelectorPeriodTo =
                    Convert.ToDateTime(
                        we.Value.Replace(we.Value.Substring(we.Value.IndexOf(".", 3) + 1, 2),
                                         "20" + we.Value.Substring(we.Value.IndexOf(".", 3) + 1, 2)).Replace(".", "/"));
                //dateTo = dateTo.Replace(dateTo.Substring(dateTo.IndexOf(".", 3) + 1, 2),
                //                             "20" + dateTo.Substring(dateTo.IndexOf(".", 3) + 1, 2));
                //dateTo = dateTo.Replace(".", "/");
                //  DateTime a1= Convert.ToDateTime(dateTo);
                //       if (!billPeriod.Contains(dateFrom) && !billPeriod.Contains(dateTo))

                for (int z = 0; z < 2; z++)
                {
                    Split[z] = Split[z].Replace(".", "/");
                }

                DateTime BillPeriodFrom = Convert.ToDateTime(Split[0]);
                DateTime BillPeriodTo = Convert.ToDateTime(Split[1]);
                if (BillPeriodFrom < SelectorPeriodFrom || BillPeriodFrom > SelectorPeriodTo)
                {
                    return "Период Счета не попадает в диапазон селектора";
                }

                we =
                    new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[2]/a");
                if (!we.Displayed)
                {
                    return "Не отображен элемент: Ссылка-номер счета.";
                }

                if (new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[3]")
                        .Text == "") return "Не отображен элемент: Оплатить до";
                if (new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[4]")
                        .Text == "") return "Не отображен элемент: Сумма по всем номерам";
                if (new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[5]")
                        .Text == "") return "Не отображен элемент: Оплачено";
                if (new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[6]")
                        .Text == "") return "Не отображен элемент: к Оплате";

                if (
                    !new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                              j + "]/td[7]/a")
                         .Enabled) return "Не отображен элемент: к структуре расходов";
                if (
                    !new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                              j + "]/td[8]/a")
                         .Enabled) return "Не отображен элемент: к истории расходов";
                if (new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                             j + "]/td[@class='payment-status']")
                        .Text == "") return "Не отображен элемент: статус оплаты";
                if (
                    !new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" +
                                              j + "]/td[10]/a")
                         .Enabled) return "Не отображен элемент: отправка по почте.";
            }

            return "success";

        }

        public string ClickOnBill()
        {

            new WebElement().ByXPath(
                "//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri='0']/td[2]/a").Click();



            return CheckSelectedBillPage();
        }

        public string ClickOnBill(int i, ref string BillNumber)
        {
            string text =
                new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri='" + i +
                                         "']/td[2]/a").Text;
            BillNumber = text;

            new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri='" + i +
                                     "']/td[2]/a").Click();




            return CheckSelectedBillPage();
        }

        public string CheckSelectedBillPage()
        {
            WebElement we;



            we =
                new WebElement().ByXPath(
                    "//ul[@class='ben']/li[@class='active']/h2/span[@class='selected']");
            if (!we.Displayed) return "Не найден элемент: выбранная группа счетов.";

            we =
                new WebElement().ByXPath(
                    "//div[@id='changeDetalizationTypeForm:changeDetalizationTypePanel']/div[@class='select-one-button ongrey-sob']/span/span[contains(text(),'Структура расходов')]");
            if (!we.Displayed) return "Не найден элемент:Вкладка Структура расходов";

            we =
                new WebElement().ByXPath(
                    "//div[@id='changeDetalizationTypeForm:changeDetalizationTypePanel']/div[@class='select-one-button ongrey-sob']/a[@id='changeDetalizationTypeForm:callHistory']");
            if (!we.Displayed) return "Не найден элемент:Вкладка История операций.";

            we =
                new WebElement().ByXPath("//div[@id='detalization-menu-buttons:filters']");
            if (!we.Displayed) return "Не найден элемент: Блок с фильтрами.";

            we =
                new WebElement().ByXPath("//div[@class='filter-col']/div/span[@class='chkbox active']");
            if (!we.Displayed) return "Не найден элемент: чекбокс фильтра.";

            we =
                new WebElement().ByXPath("//button/span[contains(text(),'Обновить отчет')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка Обновить отчет";

            we =
                new WebElement().ByXPath("//button/span[contains(text(),'Выгрузить в Excel')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка Выгрузить в Excel";
            //

            we =
                new WebElement().ByXPath("//a[contains(text(),'Отправить на email')]");
            if (!we.Displayed) return "Не найден элемент: псевдоссылка Отправить на email";

            we =
                new WebElement().ByXPath("//div[@class='select-one-button']/span/span[contains(text(),'Графиком')]");
            if (!we.Displayed) return "Не найден элемент:  активная вкладка Графиком";

            we =
                new WebElement().ByXPath("//div[@class='select-one-button']/a/span[contains(text(),'Таблицей')]");
            if (!we.Displayed) return "Не найден элемент:  вкладка Таблицей";

            we = new WebElement().ByXPath("//div[contains(text(),'Расходы по всем номерам')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Расходы по всем номерам";

            we = new WebElement().ByXPath("//div[contains(text(),'Тарифный план')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Тарифный план";

            we = new WebElement().ByXPath("//div[contains(text(),'Скидки руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Скидки руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Звонки, руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Звонки, руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'SMS/MMS руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы SMS/MMS руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Мобильный интернет руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Мобильный интернет руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Абонентская плата руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Абонентская плата руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Подключение услуг и изменения сервиса руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Подключение услуг и изменения сервиса руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Пени руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Пени руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Звонки за счет собеседника руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Звонки за счет собеседника руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Сумма по номеру без НДС руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Сумма по номеру без НДС руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Сумма счёта с НДС руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Сумма счёта с НДС руб.";
            //
            we = new WebElement().ByXPath("//table[@class='jqplot-table-legend']");
            if (!we.Displayed) return "Не найден элемент: Легенда круговой диаграммы";

            we = new WebElement().ByXPath("//canvas[@class='jqplot-base-canvas']");
            if (!we.Displayed) return "Не найден элемент: круговая диаграмма";

            we =
                new WebElement().ByXPath(
                    "//div[contains(text(),'Период выставления счёта:') and contains(text(),'Договор')]");
            if (!we.Displayed) return "Не найден элемент: текст Договор№... Период выставления счета:...";

            return "success";
        }

        public string ClickShowByTable()
        {
            WebElement we;

            new WebElement().ByXPath("//div[@class='select-one-button']/a/span[contains(text(),'Таблицей')]").Click();
            Thread.Sleep(999);

            we = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget viewport']");
            if (!we.Displayed) return "Не найден элемент: таблица";

            we =
                new WebElement().ByXPath(
                    "//div[@class='ui-outputpanel ui-widget viewport']/div/div/div[@class='th all-contract-padding']");
            if (!we.Displayed) return "Не найден элемент: таблица";

            we =
                new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget viewport']/div/div/div[@class='tbody']");
            if (!we.Displayed) return "Не найден элемент: таблица";

            for (int i = 1; i <= 17; i++)
            {
                we =
                    new WebElement().ByXPath(
                        "//div[@class='ui-outputpanel ui-widget viewport']/div/div/div[@class='tbody']/div[" + i + "]");
                if (!we.Displayed) return "Не найден элемент: пункт таблицы: " + i;
                if (we.Text == "") return "Не найден элемент: пустая строка в таблице" + i;


            }
            return "success";
        }

        public string UncheckRefreshReport()
        {
            WebElement we;

            we = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget sms-charges']/div/span");
            if (!we.Displayed) return "Не найден элемент: чекбокс СМС";
            we.Click();

            we =
                new WebElement().ByXPath(
                    "//div[@class='actions block-padding']/button/span[contains(text(),'Обновить отчет')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: кнопка Обновить отчет.";
            }
            we.Click();

            we = new WebElement().ByXPath("//div[@class='select-one-button']/a/span[contains(text(),'Графиком')]");
            if (!we.Displayed) return "Не найден элемент:  вкладка Графиком";
            we.Click();

            return CheckSelectedBillPage();
        }

        public string GoToHistoryOperationsTab()
        {
            WebElement we;
            we =
                new WebElement().ByXPath(
                    "//div[@id='changeDetalizationTypeForm:changeDetalizationTypePanel']/div[@class='select-one-button ongrey-sob']/a[@id='changeDetalizationTypeForm:callHistory']");
            if (!we.Displayed) return "Не найден элемент:Вкладка История операций.";
            we.Click();

            return CheckHistoryOperationTabPage();
        }

        public string CheckHistoryOperationTabPage()
        {
            Thread.Sleep(500);
            WebElement we;
            we = new WebElement().ByXPath("//span[@id='detalization-menu-buttons:startDate']");
            if (!we.Displayed) return "Не найден элемент: Селектор периода С";
            //
            we = new WebElement().ByXPath("//span[@id='detalization-menu-buttons:endDate']");
            if (!we.Displayed) return "Не найден элемент: Селектор периода ПО";

            we = new WebElement().ByXPath("//div[@class='popup filters-popup']");
            if (!we.Displayed) return "Не найден элемент: блок с фильтрами.";

            we = new WebElement().ByXPath("//span[@class='ui-slider-handle ui-state-default ui-corner-all']");
            if (!we.Displayed) return "Не найден элемент: бегунки.";

            we = new WebElement().ByXPath("//button/span[contains(text(),'Обновить детализацию')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: кнопка Обновить детализацию.";
            }
            // we.Click();

            we = new WebElement().ByXPath("//a/span[contains(text(),'Выгрузить в Excel')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: псевдоссылка Выгрузить в Excel.";
            }

            we = new WebElement().ByXPath("//div[@class='highcharts-container']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: график-гистограмма";
            }

            we = new WebElement().ByXPath("//tbody[@id='detalization-menu-buttons:b2cPostCallDetail_data']");
            if (!we.Displayed)
            {
                return "Не отображен элемент: таблица с детализацией расходов.";
            }

            string TableXpath = "//thead[@id='detalization-menu-buttons:b2cPostCallDetail_head']";
            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Телефон')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Телефон";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Дата и время')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Дата и время";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Исходящий номер')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Исходящий номер";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Входящий номер')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Входящий номер";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Услуга')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Услуга";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Описание услуги')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Описание услуги";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Тип услуги')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Тип услуги";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Длительность, мин сек')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Длительность, мин сек";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Стоимость, руб')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Стоимость, руб";
            }

            we = new WebElement().ByXPath(TableXpath + "/tr/th/span[contains(text(),'Размер сессии, МБ')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: Заголовок таблицы - Размер сессии, МБ";
            }
            //
            return "success";
        }

        public string SetFiltersMoveSlider()
        {
            //TODO: не могу найти юзера с такими штуками так что сорян, надо доделать


            return "Fail шаг не сделан! - нужен юзер.";
        }

        public string CheckFilterDate()
        {

            WebElement we;
            we = new WebElement().ByXPath("//span[@id='detalization-menu-buttons:startDate']/input");
            if (!we.Displayed) return "Не найден элемент: Селектор периода С";
            string dateFrom = we.Value;
            dateFrom = dateFrom.Replace(dateFrom.Substring(dateFrom.IndexOf(".", 3) + 1, 2),
                                        "20" + dateFrom.Substring(dateFrom.IndexOf(".", 3) + 1, 2));
            we = new WebElement().ByXPath("//span[@id='detalization-menu-buttons:endDate']/input");
            if (!we.Displayed) return "Не найден элемент: Селектор периода ПО";
            string dateTo = we.Value;
            dateTo = dateTo.Replace(dateTo.Substring(dateTo.IndexOf(".", 3) + 1, 2),
                                    "20" + dateTo.Substring(dateTo.IndexOf(".", 3) + 1, 2));

            we =
                new WebElement().ByXPath(
                    "//div[contains(text(),'Период выставления счёта:') and contains(text(),'Договор')]");
            if (!we.Displayed) return "Не найден элемент: текст Договор№... Период выставления счета:...";
            string BillDates = we.Text;

            if (!BillDates.Contains(dateFrom) && !BillDates.Contains(dateTo))
                return "Даты в фильтрах не соответствуют периоду, за который выставлен счет";

            return "success";
        }


        public string GoToChargesStructure(int i)
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" + i +
                                         "]/td[7]/a");
            if (!we.Enabled) return "Не отображен элемент: к структуре расходов";
            we.Click();
            return CheckChargesStructurePage();
        }

        public string CheckChargesStructurePage()
        {

            WebElement we;



            //we =
            //    new WebElement().ByXPath(
            //        "//ul[@class='ben']/li[@class='active']/h2/span[@class='selected']");
            //if (!we.Displayed) return "Не найден элемент: выбранная группа счетов.";


            we = new WebElement().ByXPath("//div[@class='popup filters-popup']");
            if (!we.Displayed) return "Не найден элемент: Блок с фильтрами.";

            we =
                new WebElement().ByXPath("//span[contains(text(),'По счету')]");
            if (!we.Displayed) return "Не найден элемент: вкладка";

            we =
                new WebElement().ByXPath("//span[contains(text(),'За текущий период')]");
            if (!we.Displayed) return "Не найден элемент: За текущий период";

            we =
                new WebElement().ByXPath(
                    "//div[@class='value']/div[@class='ui-selectonemenu ui-widget ui-state-default ui-corner-all']");
            if (!we.Displayed) return "Не найден элемент: селектор выбора расчетного периода";

            we = new WebElement().ByXPath("//button/span[contains(text(),'Обновить отчет')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка Обновить отчет";

            we = new WebElement().ByXPath("//a[contains(text(),'Выгрузить в Excel')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка Выгрузить в Excel";
            //


            we = new WebElement().ByXPath("//div[@class='select-one-button']/span/span[contains(text(),'Графиком')]");
            if (!we.Displayed) return "Не найден элемент:  активная вкладка Графиком";

            we = new WebElement().ByXPath("//div[@class='select-one-button']/a/span[contains(text(),'Таблицей')]");
            if (!we.Displayed) return "Не найден элемент:  вкладка Таблицей";

            we = new WebElement().ByXPath("//div[contains(text(),'Расходы по всем номерам')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Расходы по всем номерам";

            we = new WebElement().ByXPath("//div[contains(text(),'Тарифный план')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Тарифный план";

            we = new WebElement().ByXPath("//div[contains(text(),'Скидки руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Скидки руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Звонки, руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Звонки, руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'SMS/MMS руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы SMS/MMS руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Мобильный интернет руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Мобильный интернет руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Абонентская плата руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Абонентская плата руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Подключение услуг и изменения сервиса руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Подключение услуг и изменения сервиса руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Пени руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Пени руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Звонки за счет собеседника руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Звонки за счет собеседника руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Сумма по номеру без НДС руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Сумма по номеру без НДС руб.";

            we = new WebElement().ByXPath("//div[contains(text(),'Сумма счёта с НДС руб.')]");
            if (!we.Displayed) return "Не найден элемент: таблицы Сумма счёта с НДС руб.";
            //
            we = new WebElement().ByXPath("//table[@class='jqplot-table-legend']");
            if (!we.Displayed) return "Не найден элемент: Легенда круговой диаграммы";

            we = new WebElement().ByXPath("//canvas[@class='jqplot-base-canvas']");
            if (!we.Displayed) return "Не найден элемент: круговая диаграмма";


            return "success";
        }

        public string GoToChargesHistory(int i)
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" + i +
                                         "]/td[8]/a");
            if (!we.Enabled) return "Не отображен элемент: к истории расходов";
            we.Click();

            return CheckHistoryOperationTabPage();
        }

        public string ClickOnSummByBill(int i)
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=" + i +
                                         "]/td[4]/a");
            if (!we.Enabled) return "Не отображен элемент: псведоссылка для счета Сумма по всем номерам";
            we.Click();

            return CheckChargesStructurePage();
        }

        public string ClickOnDownloadXLS()
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//button/span[contains(text(),'Выгрузить в Excel')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка Выгрузить в Excel";
            we.Click();

            Thread.Sleep(1500);
            SendKeys.SendWait("{DOWN}");
            Thread.Sleep(200);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            string path = Environment.GetEnvironmentVariable("HOMEPATH") + "\\Downloads";
            Directory.SetCurrentDirectory(path);


            path = Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(path, "bill_charges.xls");

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

            //TODO : проверить файл.
            return "success";
        }

        public string ClickSentOnEmail(string BillNumber, string BAN)
        {
            WebElement we;
            we =
                new WebElement().ByXPath("//a[contains(text(),'Отправить на email')]");
            if (!we.Displayed) return "Не найден элемент: псевдоссылка Отправить на email";
            we.Click();

            return CheckSentOnEmailPage(BillNumber, BAN);


        }

        public string CheckSentOnEmailPage(string BillNumber, string BAN)
        {
            WebElement we;

            we = new WebElement().ByXPath("//h1[contains(text(),'Счета')]");
            if (!we.Displayed) return "Не найден элемент: Хедер - Счета.";

            we = new WebElement().ByXPath("//input[contains(@id,':invoiceMailSendEmail')]");
            if (!we.Displayed) return "Не найден элемент: инпут ввода почты";

            we = new WebElement().ByXPath("//div[contains(text(),'" + BillNumber + "')]");
            if (!we.Displayed) return "Не найден элемент: Номер счета";

            we = new WebElement().ByXPath("//div[contains(text(),'" + BAN + "')]");
            if (!we.Displayed) return "Не найден элемент: БАН ";

            we = new WebElement().ByXPath("//div/div[contains(text(),'Группа счетов')]");
            if (!we.Displayed) return "Не найден элемент: Группа счетов ";

            we =
                new WebElement().ByXPath(
                    "//button[contains(@onclick,':invoiceMailSendEmailMessage')]/span[contains(text(),'Отправить')]");
            if (!we.Displayed) return "Не найден элемент: Кнопка отправить";

            return "success";
        }


        public string SentDetailsOnMail(ref string reqID)
        {
            WebElement we;
            new WebElement().ByXPath("//input[contains(@id,':invoiceMailSendEmail')]").SendKeys(
                "dabolenikhin@beeline.ru");
            new WebElement().ByXPath(
                "//button[contains(@onclick,':invoiceMailSendEmailMessage')]/span[contains(text(),'Отправить')]").Click();

            we = new WebElement().ByXPath("//h3[@class='mail-success']");
            if (!we.Displayed) return "Не найден элемент: Уведомление о зарегистрированной заявке";

            we =
                new WebElement().ByXPath(
                    "//h3[contains(text(),'сформирован. Счёт в формате pdf будет сформирован и отправлен на указанный Вами E-mail')]");
            if (!we.Displayed) return "Не найден элемент: Уведомление о зарегистрированной заявке_текст";
            string reqId = we.Text;
            int ptr = reqId.IndexOf("№");
            reqID = reqId.Substring(ptr + 1, 10);

            return "success";
        }

        public string HistoryOperationPageClickDownloadXLS()
        {
            WebElement we;
            we = new WebElement().ByXPath("//a/span[contains(text(),'Выгрузить в Excel')]");
            if (!we.Displayed)
            {
                return "Не отображен элемент: псевдоссылка Выгрузить в Excel.";
            }
            we.Click();


            Thread.Sleep(1500);
            //  SendKeys.SendWait("{DOWN}");
            // Thread.Sleep(200);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            string path = Environment.GetEnvironmentVariable("HOMEPATH") + "\\Downloads";
            Directory.SetCurrentDirectory(path);


            path = Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(path, "Detalizatciia_zvonkov*.xls");

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

            //TODO : проверить файл.
            return "success";
        }

        public string BillPageClickSentOnEmail(string BillNumber, string BAN)
        {
            WebElement we, we1, we3;

            string text =
                new WebElement().ByXPath(
                    "//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=0]/td[2]/a").Text;
            BillNumber = text;

            we =
                new WebElement().ByXPath(
                    "//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=0]/td[@class='send-to-email more-on-hover']");
            we1 =
                new WebElement().ByXPath(
                    "//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=0]/td[@class='send-to-email more-on-hover']/a[1]");
            we3 =
                new WebElement().ByXPath(
                    "//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=0]/td[@class='send-to-email more-on-hover']/a[2]");
            //if (!we.Displayed) return "Не найден элемент: псевдоссылка Отправить на email";
            Thread.Sleep(500);
            string a = we.GetAttribute("href");
            string b = we1.GetAttribute("href");

            // Thread.Sleep(3000);

            //     Thread.Sleep(7000);
            //  Browser.MoveToElement("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=1]/td[10]/a[@class='ui-icon ui-icon-mail-closed hide-on-hover']");
            //  if (we1.Enabled) we1.Click();
            for (int i = 0; i < 2; i++)
            {
                if (we.Displayed)
                {
                    we.Click();
                }
                // Browser.MoveToElement("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=1]/td[10]/a[1]");
                if (we3.Displayed)
                {
                    we3.Click();
                }
                //  Browser.MoveToElement("//tbody[contains(@id,'billsForm:b2cpostbillstable_data')]/tr[@data-ri=1]/td[10]/a[1]");
                if (we1.Displayed)
                {
                    we1.Click();
                }
            }
            return CheckSentOnEmailPage(BillNumber, BAN);

        }

        public string DownloadExportInvoiceWE()
        {
            InvoiceWE = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget invoices'][1]");
            ExportInvoiceWE = InvoiceWE.ByXPath("//span[contains(text(),'Выгрузить в Excel')]");

            ExportInvoiceWE.Click();
            Thread.Sleep(1500);
            //  SendKeys.SendWait("{DOWN}");
            // Thread.Sleep(200);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            string path = Environment.GetEnvironmentVariable("HOMEPATH") + "\\Downloads";
            Directory.SetCurrentDirectory(path);


            path = Environment.CurrentDirectory;
            string[] files = Directory.GetFiles(path, "schet_*.xls");

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
            //Запрос №2147683166 сформирован. Счёт в формате pdf будет сформирован и отправлен на указанный Вами E-mail
            //h3 class='mail-success'
        }
    }
}

