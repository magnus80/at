using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using System.Windows.Forms;
using System.IO;


namespace USSS.WebPages.B2BPost
{
    class NumberProfilePage
    {
        public RequestHistoryPage requestHistoryPage;
        string db_Ans = ReaderTestData.ReadCExel(4, 7);
        public string Number;
        #region constructor


        //таблица абонентов
        private WebElement ChangeSimBtnWE;


        #endregion

        public string ConstructionPage()
        {
            string number = new WebElement().ByXPath("//span[@class='has-subheader']").Text;
            number = number.Remove(number.IndexOf('('), 1);
            number = number.Remove(number.IndexOf(')'), 1);
            number = number.Remove(number.IndexOf(' '), 1);
            number = number.Remove(number.IndexOf('-'), 1);
            number = number.Remove(number.IndexOf('-'), 1);
            Number = number;
            var query = @"select REPLACE_SIM_IND from ecr9_billing_account_ext where ban= (select customer_id from ecr9_subscriber where subscriber_no = '" + number + "')";
            var simQ = Executor.ExecuteSelect(query);
            string replace_ind = simQ[0,0];
            
            ChangeSimBtnWE = new WebElement().ByXPath("//button[contains(@id,'simReplaceForm')]");
            if (replace_ind != "Y") { return "Запрещено изменение SIM: REPLACE_SIM_IND = " + replace_ind; }
            if (!ChangeSimBtnWE.Displayed) { return "Не отображены элементы интерфейса: кнопка замены Sim"; }

            return "success";
        }

        public string ConstructionPage(string checkBlocks)
        {
            WebElement WEnumber = new WebElement().ByXPath("//span[@class='has-subheader']");
            if (!WEnumber.Displayed) { return "Не отображен номер пользователя"; }
            string number = WEnumber.Text;
            number = number.Remove(number.IndexOf('('), 1);
            number = number.Remove(number.IndexOf(')'), 1);
            number = number.Remove(number.IndexOf(' '), 1);
            number = number.Remove(number.IndexOf('-'), 1);
            number = number.Remove(number.IndexOf('-'), 1);
            Number = number;
            WebElement WEConnectedServices = new WebElement().ByXPath("//div[@id='manageServs']");
            if (!WEConnectedServices.Displayed) { return "Не отображена таблица Подключенные услуги"; }
            WebElement WEcontactInformation = new WebElement().ByXPath("//div[@id='contactInformation']");
            if (!WEcontactInformation.Displayed) { return "Не отображена таблица Информация о пользователе"; }
            WebElement WEreportDetailPanel = new WebElement().ByXPath("//div[@id='reportDetailPanel']");
            if (!WEreportDetailPanel.Displayed) { return "Не отображена таблица Детализация звонков"; }
            WebElement reportButtonsForm_right = new WebElement().ByXPath("//a[@id='reportButtonsForm:rightButton']");
            if (!reportButtonsForm_right.Displayed) { return "Не отображена кнопка по Счету"; }
            WebElement reportButtonsForm_left = new WebElement().ByXPath("//span[@id='reportButtonsForm:leftButtonLabel']");
            if (!reportButtonsForm_left.Displayed) { return "Не отображена кнопка Текущий период"; }

            WebElement reportDetailUnbilledButtonsForm = new WebElement().ByXPath("//button[@id='reportDetailUnbilledButtonsForm:offlineReportButton']");
            if (!reportDetailUnbilledButtonsForm.Displayed) { return "Не отображена кнопка Получить детализацию"; }

            //TODO: ссылки на ранее заказанные детализации  - ДОБАВИТЬ!!! на ЮЗЕРЕ НЕ БЫЛО
            //

            return "success";
        }
        #region managerPage

        public string GoToChangeSim()
        {
            if (ChangeSimBtnWE.Displayed) ChangeSimBtnWE.Click();
            else { return "Не отображены элементы интерфейса: кнопка замены Sim"; }
            Thread.Sleep(10000);
            WebElement ChangeSimForm = new WebElement().ByXPath("//div[contains(@id,'simReplaceForm')]");
            if (!ChangeSimForm.Displayed) { return "Не отображены элементы интерфейса: форма замены Sim"; }
          
            string NGP = new WebElement().ByXPath("//div[@class='change-pass-form common-form']//div[@class='field text']/span").Text;
            var query = @"select logical_hlr from number_group@" + db_Ans + " where  ngp_id = (select ngp from ecr9_subscriber where subscriber_no = '" + Number + "')";
            var ngpQ = Executor.ExecuteSelect(query);
            string ngbDB = ngpQ[0, 0];
            ngbDB = ngbDB.Remove(ngbDB.IndexOf(' '), 1);
            if (ngbDB != NGP) { return "Отображается не верный NGP"; }
            WebElement BtnCancelChangeWE = new WebElement().ByXPath("//div[contains(@id,'simReplaceForm')]//button[contains(@class,'ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')]");
            if (!BtnCancelChangeWE.Displayed) { return "Не отображены элементы интерфейса: кнопка отмены замены SIM"; }
            string hintSim = new WebElement().ByXPath("//div[@class='change-pass-form common-form']//div[@class='field']").Text;
            if (hintSim.Length != 8) { return "Некорректно отображена подсказка ввода сим"; }
            return "success";
        }


        public string ChangeSim(string sim)
        {
            WebElement NewSimWE = new WebElement().ByXPath("//input[contains(@id,'newSimNumber')]");
            if (NewSimWE.Displayed) NewSimWE.SendKeys(sim);
            else { return "Не отображены элементы интерфейса: поле ввода SIM"; }
            WebElement BtnChangeWE = new WebElement().ByXPath("//button[contains(@class,'orange')]");
            if (BtnChangeWE.Displayed) BtnChangeWE.Click();
            else { return "Не отображены элементы интерфейса: кнопка подтверждения смены sim"; }

            var query = @"select sub_status from ecr9_subscriber where subscriber_no = '" + Number + "'";
            var statusQ = Executor.ExecuteSelect(query);
            string statusDB = statusQ[0, 0];

            if (statusDB == "S")
            {
                var query1 = @"select sub_status_rsn_code from ecr9_subscriber where subscriber_no = '" + Number + "'";
                var statusSubQ = Executor.ExecuteSelect(query1);
                string statusSubDB = statusQ[0, 0];
                {
                    NewSimWE = new WebElement().ByXPath("//input[contains(@id,'newSimNumber')]");
                    if (NewSimWE.Displayed) NewSimWE.SendKeys(sim);
                    else { return "Не отображены элементы интерфейса: поле ввода SIM"; }
                    BtnChangeWE = new WebElement().ByXPath("//button[contains(@class,'orange')]");
                    if (BtnChangeWE.Displayed) BtnChangeWE.Click();
                    ConfirmNotif();
                    if (statusSubDB != "STO")
                    {
                        return ReadErrorMassage("Замена SIM-карты невозможна, необходимо разблокировать абонента");
                    }
                    return ReadErrorMassage("При замене SIM-карты номер будет разблокирован");

                }
            }


            
            return "success";
        }

        public string CheckChangeSim()
        {
            WebElement NewSimWE = new WebElement().ByXPath("//input[contains(@id,'newSimNumber')]");
            if (NewSimWE.Displayed && NewSimWE.Text != "") return "success";
            else { return "Не отображены элементы интерфейса: не отображен введеный номер сим"; }   
        }

        public string ReadErrorMassage()
        {
            for (int i = 0; i < 10; i++)
            {
                WebElement MassageErrorWE = new WebElement().ByXPath("//span[contains(text(),'Поле обязательно')]");
                if (MassageErrorWE.Displayed) return MassageErrorWE.Text.ToString();
                Thread.Sleep(1000);
            }
            return "success";
        }

        public string ReadErrorMassage(string er)
        {
            for (int i = 0; i < 10; i++)
            {
                WebElement MassageErrorWE = new WebElement().ByXPath("//span[contains(text(),'"+er+"')]");
                if (MassageErrorWE.Displayed) return MassageErrorWE.Text.ToString();
                Thread.Sleep(1000);
            }
            return "success";
        }

        public string ConfirmNotif()
        {
            Thread.Sleep(5000);
            WebElement NotifiWE = new WebElement().ByXPath("//div[@class='ui-dialog ui-widget ui-widget-content ui-corner-all ui-shadow request-confirm-dlg ui-draggable ui-overlay-visible']//span[contains(text(),'Запрос на замену сим карты')]");
            if (!NotifiWE.Displayed) return "Не отображены элементы интерфейса: окно подтверждения нотификации";
            Thread.Sleep(5000);
            WebElement ConfirmWE = new WebElement().ByXPath("//span[contains(text(),'Отправить')]");
            WebElement EmailInput = new WebElement().ByXPath("//input[@id = 'j_idt628:requestUserServiceParamsForm:email']");
            WebElement checkbox = new WebElement().ByXPath("//div[contains(@id,'emailCheckbox')]");
            if (!EmailInput.Displayed)
            {
                
                  return "Не отображено поле ввода email";
                
            }
            EmailInput.SendKeys("avyalov@bellintegrator.ru");
            if (ConfirmWE.Displayed) ConfirmWE.Click();
            else { return "Не отображены элементы интерфейса: кнопка подтверждения канала нотификации"; }
            return "success";
        }


        public string ClickDetailsByBill()
        {

            WebElement reportButtonsForm_right = new WebElement().ByXPath("//a[@id='reportButtonsForm:rightButton']");
            reportButtonsForm_right.Click();

            WebElement WETab = new WebElement().ByXPath("//span[contains(text(),'Начисления')]");
            if (!WETab.Displayed) return "Не отображена вкладка Начисления";
            WETab = new WebElement().ByXPath("//a[contains(text(),'Звонки')]");
            if (!WETab.Displayed) return "Не отображена вкладка Звонки";

            WebElement WEPeriodSelector = new WebElement().ByXPath("//div[@id='reportDetailBilledForm:billDates']");
            if (!WEPeriodSelector.Displayed) { return "Не отображен выпадающий список Выберите расчетный период"; }


            return "success";
        }

        public string SelectBillDate()
        {
            //Thread.Sleep(1500);

            WebElement WESelector = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:billDates_label']");
            if (!WESelector.Displayed) return "Не найдены расчетные периоды";
            WESelector.Click();
            WebElement WEBillDate = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:billDates_panel']/div/ul/li[2]");
            if (!WEBillDate.Displayed) return "Не найдены расчетные периоды";
            WebElement WEBillDateSelected = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:billDates_label']");
            string CurrentBillDate = WEBillDateSelected.Text;
            WEBillDate.Click();
            if (CurrentBillDate == new WebElement().ByXPath("//*[@id='reportDetailBilledForm:billDates_label']").Text)
                return "После клика по селектору, значение не поменялось";
            return "success";
        }

        public string CheckDetailsTableCharges()
        {
            string ErrorMessageCheck = "Не найдено: ";
            int counterErr = 0;
            WebElement WEReportDetailsTable = new WebElement().ByXPath("//*[@id='reportDetailPanel']");
            if (!WEReportDetailsTable.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена таблица детализации", ref counterErr);

            //   return "Не найдена таблица детализации";

            WebElement WETableHeader = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargesTotal']/table/tbody/tr[1]/th[contains(text(),'Вид начислений')]");
            if (!WETableHeader.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Вид начислений", ref counterErr);
            //return "Не найдена строка Вид начислений";

            WebElement WETablerow = new WebElement().ByXPath("//*[contains(text(),'Суммарные начисления')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Суммарные начисления", ref counterErr);
            //return "Не найдена строка Суммарные начисления";

            WebElement WETablerowDiscounts = new WebElement().ByXPath("//a[contains(text(),'Скидки')]");
            if (!WETablerowDiscounts.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Скидки", ref counterErr);
            //return "Не найдена строка Скидки";

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Абонентская плата')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Абонентская плата", ref counterErr);
            //return "Не найдена псевдоссылка Абонентская плата";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Звонки')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Звонки", ref counterErr);
            //return "Не найдена строка Звонки";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Международные звонки')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Международные звонки", ref counterErr);
            //return "Не найдена строка Международные звонки";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Междугородние звонки ')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Междугородние звонки", ref counterErr);
            //return "Не найдена строка Междугородние звонки ";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Местные звонки')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Местные звонки", ref counterErr);
            //return "Не найдена строка Местные звонки";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'SMS/MMS сообщения')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка SMS/MMS сообщения", ref counterErr);
            //return "Не найдена строка SMS/MMS сообщения";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'GPRS')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка GPRS", ref counterErr);
            //return "Не найдена строка GPRS";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Роуминг')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Роуминг", ref counterErr);
            //return "Не найдена строка Роуминг";

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Международный роуминг')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Международный роуминг", ref counterErr);
            //return "Не найдена псевдоссылка Международный роуминг";

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Национальный и внутрисетевой роуминг')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Национальный и внутрисетевой роуминг", ref counterErr);
            //return "Не найдена псевдоссылка Национальный и внутрисетевой роуминг";

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Включенные минуты')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Включенные минуты", ref counterErr);
            //return "Не найдена псевдоссылка Включенные минуты";

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Разовые начисления')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Разовые начисления", ref counterErr);
            //return "Не найдена псевдоссылка Разовые начисления";
            Thread.Sleep(500);
            WebElement WEChart = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:callcostpiechartpanel']");
            if (!WEChart.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Диаграмма", ref counterErr);
            //return "Не найдена Диаграмма";

            WETablerow = new WebElement().ByXPath("//*[contains(text(),'Сумма начислений, руб (без НДС)')]");
            if (!WETablerow.Displayed) Logger.ErrorAccumulate(ref ErrorMessageCheck, " Не найдена строка Сумма начислений, руб (без НДС)", ref counterErr);
            //return "Не найдена строка Сумма начислений, руб (без НДС)";


            for (int i = 2; i < 13; i++)
            {
                WETablerow = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargesTotal']/table/tbody/tr[" + i + "]/td[2]");
                //  WETablerow = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargesTotal']/table/tbody/tr[" + i + "]/td[2]");
                if (!WETablerow.Displayed) return "Не найдена строка с суммой начислений по " + new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargesTotal']/table/tbody/tr[" + i + "]/td[1]").Text;
            }

            WETablerowDiscounts.Click();
            Thread.Sleep(300);
            WebElement DetailTableOnClick = new WebElement().ByXPath("//*[@class='ui-widget-content ui-datatable-even']");
            if (!DetailTableOnClick.Displayed) return "Не найдена таблица после клика по пвседоссылке.";

            WebElement WEDownloadXLSReportLink = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargeTypeDetailsPanel']/div[1]/a");
            if (!WEDownloadXLSReportLink.Displayed) return "Не найдена псевдоссылка Выгрузить в Excel";

            return Logger.ErrorReturn(ref ErrorMessageCheck, ref counterErr);



            //  return "success";
        }


        public string ClickDetailsBilled()
        {
            WebElement WETab = new WebElement().ByXPath("//a[contains(text(),'Звонки')]");
            if (!WETab.Displayed) return "Не отображена вкладка Звонки";
            WETab.Click();
            Thread.Sleep(1500);
            WETab = new WebElement().ByXPath("//a[contains(text(),'Звонки')]");
            if (WETab.Displayed) return "вкладка Звонки Активна";

            return "success";
        }

        public string CheckDetailsTableBilled()
        {

            string ErrorMessageCheck = "Не найдены: ";
            int counterErr = 0;

            WebElement WEReportDetailsTable = new WebElement().ByXPath("//*[@id='reportDetailBilledForm']");
            if (!WEReportDetailsTable.Displayed)
            {
                ErrorMessageCheck += " Не найдена таблица детализации ";
                counterErr++;
            }


            //----
            WebElement WETableHeader = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:detailsTotal']/table/tbody/tr[1]/th[contains(text(),'Вид звонков')]");
            if (!WETableHeader.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка Вид звонков ";
                counterErr++;
            }

            WebElement WETablerow = new WebElement().ByXPath("//*[contains(text(),'Суммарные начисления')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка Суммарные начисления ";
                counterErr++;
            }


            //
            //Тут некоторые из них должны быть типа как ссылки, но т.к. детализации по ним нет, такую проверку не сделал для них.
            //


            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Международные звонки')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Международные звонки ";
                counterErr++;
            }


            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Междугородние звонки ')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Междугородние звонки ";
                counterErr++;
            }


            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Местные звонки')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Местные звонки ";
                counterErr++;
            }


            WebElement WETablerowSMS = new WebElement().ByXPath("//a[contains(text(),'SMS/MMS сообщения')]");
            if (!WETablerowSMS.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка SMS/MMS сообщения ";
                counterErr++;
            }


            WETablerow = new WebElement().ByXPath("//*[contains(text(),'GPRS')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка GPRS ";
                counterErr++;
            }

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Роуминговые звонки и SMS')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка Роуминг ";
                counterErr++;
            }

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Роуминговые GPRS-сессии')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена строка Роуминговые GPRS-сессии ";
                counterErr++;
            }


            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Роуминговые звонки (Начисления ВымпелКом)')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Роуминговые звонки (Начисления ВымпелКом) ";
                counterErr++;
            }


            WETablerow = new WebElement().ByXPath("//a[contains(text(),'Роуминговые GPRS сессии  (Начисления ВымпелКом)')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Роуминговые GPRS сессии  (Начисления ВымпелКом) ";
                counterErr++;
            }

            WETablerow = new WebElement().ByXPath("//a[contains(text(),'RUS-RPP/TPP Charges')]");
            if (!WETablerow.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка RUS-RPP/TPP Charges ";
                counterErr++;
            }

            //  WebElement WEChart = new WebElement().ByXPath("//*[contains(id(),'reportDetailBilledForm')]/canvas[7]");
            WebElement WEChart = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:piechartpanel']");
            if (!WEChart.Displayed)
            {
                ErrorMessageCheck += " Не найдена Диаграмма ";
                counterErr++;
            }



            for (int i = 2; i < 10; i++)
            {
                WETablerow = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:detailsTotal']/table/tbody/tr[" + i + "]/td[2]");
                //  WETablerow = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:chargesTotal']/table/tbody/tr[" + i + "]/td[2]");
                if (!WETablerow.Displayed)
                {
                    ErrorMessageCheck += " Не найдена строка с суммой начислений по " + new WebElement().ByXPath("//*[@id='reportDetailBilledForm:detailsTotal']/table/tbody/tr[" + i + "]/td[1]").Text;
                    counterErr++;
                }


            }

            WETablerowSMS.Click();

            Thread.Sleep(1500);
            WebElement DetailTableOnClick = new WebElement().ByXPath("//*[@class='ui-outputpanel ui-widget call-type-details-panel']");
            if (!DetailTableOnClick.Displayed)
            {
                ErrorMessageCheck += " Не найдена таблица после клика по пвседоссылке. ";
                counterErr++;
            }

            WebElement WEDownloadXLSReportLink = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:callTypeDetailsPanel']/div[1]/a");
            if (!WEDownloadXLSReportLink.Displayed)
            {
                ErrorMessageCheck += " Не найдена псевдоссылка Выгрузить в Excel ";
                counterErr++;
            }

            if (counterErr != 0) return ErrorMessageCheck;

            return "success";

            //--
        }

        public string ClickButtonDetailsbyBill()
        {

            WebElement weButtonDetailsByBill = new WebElement().ByXPath("//span[contains(text(),'Детализация по счету')]");
            if (!weButtonDetailsByBill.Displayed) return "Не найдена кнопка  Детализация по счету";

            weButtonDetailsByBill.Click();
            Thread.Sleep(1500);

            return "success";
        }


        public string ClickButtonUnbilledDetails()
        {

            WebElement weButtonDetailsByBill = new WebElement().ByXPath("//a[contains(text(),'Текущий период')]");
            if (!weButtonDetailsByBill.Displayed) return "Не найдена кнопка  Текущий период";

            weButtonDetailsByBill.Click();
            Thread.Sleep(1500);

            return "success";
        }

        public string CheckTableDetailsByBill()
        {
            WebElement weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Дата')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Дата  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Время')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Время  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Номер/e-mail')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Номер/e-mail  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Продолжительность')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Продолжительность  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Объем данных, Мб')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Объем данных, Мб  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Страна, сеть роуминг-партнера')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Страна, сеть роуминг-партнера  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Тип звонков')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Тип звонков  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Сумма')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Сумма  ";

            weTableHeader = new WebElement().ByXPath("//span[contains(text(),'Номер базовой станции')]");
            if (!weTableHeader.Displayed) return "Не найдена колонка таблицы Номер базовой станции  ";

            WebElement WEDownloadXLSReportLink = new WebElement().ByXPath("//*[@id='reportDetailBilledForm:callTypeDetailsPanel']/div[1]/a");
            if (!WEDownloadXLSReportLink.Displayed) return "Не найдена псевдоссылка Выгрузить в Excel";

            string ErrorMessageCheck = "Не найдены данные в столбцах #";
            int counterErr = 0;
            for (int i = 1; i <= 9; i++)
            {
                WebElement TableDetailsRow =
                    new WebElement().ByXPath(
                        "//*[@class='ui-outputpanel ui-widget call-type-details-panel']/div[@class='ui-datatable ui-widget']/div[@class='ui-datatable-tablewrapper']/table/tbody/tr[@data-ri='1']/td[" + i + "]");
                if (!TableDetailsRow.Displayed || TableDetailsRow.Text == "")
                {
                    ErrorMessageCheck += " " + i + ",";
                    counterErr++;
                }

            }
            if (counterErr != 0) return ErrorMessageCheck;

            return "success";
        }

        public string GoToChargesTab()
        {
            WebElement weTabCharges = new WebElement().ByXPath("//*[@id='reportButtonsForm:buttonsPanel']/div[2]/a[contains(text(),'Начисления')]");
            if (!weTabCharges.Displayed) return "Не найдена вкладка Начисления ";

            weTabCharges.Click();

            return "success";
        }
        public string ClickGetUnbilled()
        {
            WebElement reportDetailUnbilledButtonsForm = new WebElement().ByXPath("//button[@id='reportDetailUnbilledButtonsForm:offlineReportButton']");
            if (!reportDetailUnbilledButtonsForm.Displayed) { return "Не отображена кнопка Получить детализацию"; }
            reportDetailUnbilledButtonsForm.Click();
            Thread.Sleep(1500);

            return "success";
        }

        public string CheckDetailsUnbilled()
        {
            WebElement we;
            we = new WebElement().ByXPath("//div[@id='reportDetailUnbilledButtonsForm:subscriberUnbilledCharges']");
            if (!we.Displayed)
            {
                for (int i = 0; i < 65; i++)
                {
                    we = new WebElement().ByXPath("//div[@id='reportDetailUnbilledButtonsForm:subscriberUnbilledCharges']");
                    Thread.Sleep(300);
                    if (we.Displayed) { break; }
                }
            }

            if (!we.Displayed) { return "Не отображены данные за текущий период"; }

            we = new WebElement().ByXPath("//div[@id='reportDetailUnbilledForm:unbilledEntries']");
            if (!we.Displayed)
            {
                for (int i = 0; i < 65; i++)
                {
                    we = new WebElement().ByXPath("//div[@id='reportDetailUnbilledForm:unbilledEntries']");
                    Thread.Sleep(300);
                    if (we.Displayed) { break; }
                }
            }

            if (!we.Displayed) { return "Не отображена таблица детализации за текущий период"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Дата звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Дата звонка"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Время звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Время звонка"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Инициатор звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Инициатор звонка"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Набранный номер')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Набранный номер"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Тип звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Тип звонка"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Услуга')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Услуга"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Предварительная стоимость (без НДС)')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Предварительная стоимость (без НДС)"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Продолжительность')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Продолжительность"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Объем (MB)')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Объем (MB)"; }

            we = new WebElement().ByXPath("//span[contains(text(),'Номер БС')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Номер БС"; }

            we = new WebElement().ByXPath("//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']");
            if (!we.Displayed) { return "Не отображены данные в таблице(строки)"; }

            string[] testArray = new string[10];
            for (int i = 1; i <= 9; i++)
            {
                testArray[i] = new WebElement().ByXPath(
                     "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[" + i + "]").Text.ToString();
                if (testArray[i] == "")
                {
                    return "Пустые данные в таблице - столбец: " + i;
                }
            }


            return "success";
        }
        #endregion

        public string CheckSortDetails()
        {
            WebElement we;
            WebElement weTableElement;
            string TableElement;
            string TableElementCompare;
            we = new WebElement().ByXPath("//span[contains(text(),'Дата звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Дата звонка"; }
            //   we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[1]");
            TableElement = weTableElement.Text;

            //   Thread.Sleep(1500);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[1]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (1)";

            we = new WebElement().ByXPath("//span[contains(text(),'Время звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Время звонка"; }


            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[2]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[2]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (2)";





            we = new WebElement().ByXPath("//span[contains(text(),'Инициатор звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Инициатор звонка"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[3]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[3]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (3)";





            we = new WebElement().ByXPath("//span[contains(text(),'Набранный номер')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Набранный номер"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[4]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[4]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (4)";

            we = new WebElement().ByXPath("//span[contains(text(),'Тип звонка')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Тип звонка"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[5]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[5]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (5)";

            we = new WebElement().ByXPath("//span[contains(text(),'Услуга')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Услуга"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[6]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[6]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (6)";

            we = new WebElement().ByXPath("//span[contains(text(),'Предварительная стоимость (без НДС)')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Предварительная стоимость (без НДС)"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[7]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[7]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (7)";

            we = new WebElement().ByXPath("//span[contains(text(),'Продолжительность')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Продолжительность"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[8]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[8]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (8)";

            we = new WebElement().ByXPath("//span[contains(text(),'Объем (MB)')]");
            if (!we.Displayed) { return "Не отображен столбец таблицы Объем (MB)"; }

            we.Click();
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[9]");
            TableElement = weTableElement.Text;

            Thread.Sleep(2000);
            we.Click();
            Thread.Sleep(2000);
            weTableElement = new WebElement().ByXPath(
                        "//tbody[@id='reportDetailUnbilledForm:unbilledEntries_data']/tr[@data-ri='0']/td[9]");
            TableElementCompare = weTableElement.Text;
            if (TableElement == TableElementCompare) return "Ошибка работы сортировки столбца (9)";


            return "success";
        }
        public string CancelChangeTariff()
        {
            Browser.MoveToElement("//div[contains(text(),'Тариф:')]");
            Browser.MoveToElement("//a[contains(text(),'Отмена смены тарифного плана')]");
            WebElement cancelhref = new WebElement().ByXPath("//a[contains(text(),'Отмена смены тарифного плана')]");
            WebElement cancelWindow;
            if (cancelhref.Enabled)
            {
                cancelhref.Click();
                cancelWindow = new WebElement().ByXPath("//div[@id='notificationRevertTarif:notificationConfirmComponentDialog']");
                for(int i = 0;i<5;i++)
                {
                    if(cancelWindow.Displayed)
                    {
                        i = 5;
                    }
                    Thread.Sleep(3000);
                    cancelWindow = new WebElement().ByXPath("//div[@id='notificationRevertTarif:notificationConfirmComponentDialog']");
                }
                if(cancelWindow.Displayed)
                {
                    WebElement emailInput = new WebElement().ByXPath("//div[@id='notificationRevertTarif:notificationConfirmComponentDialog']//input[@id='notificationRevertTarif:requestUserServiceParamsForm:email']");
                    WebElement numberInput = new WebElement().ByXPath("//div[@id='notificationRevertTarif:notificationConfirmComponentDialog']//input[@id='notificationRevertTarif:requestUserServiceParamsForm:smsInput']");
                    numberInput.SendKeys("9999999999");
                    emailInput.SendKeys("avyalov@bellintegrator.ru");
                    WebElement confirmButton = new WebElement().ByXPath("//button[@id='notificationRevertTarif:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog']");
                    confirmButton.Click();
                    Thread.Sleep(3000);
                    
                    return "success";
                    
                }
                else
                {
                    return "Не отображено окно отмены";
                }
            }
            else
            {
                return "Ссылка на отмену смены тарифа недоступна";
            }
        }
        public string GoToRequestPage()
        {
            requestHistoryPage = new RequestHistoryPage();
            WebElement requestpage = new WebElement().ByXPath("//a[@id='navRequests']");
            if(requestpage.Displayed)
            {
                requestpage.Click();
                return requestHistoryPage.ConstructionPage();
            }
            else
            {
                return "Не удалось открыть страницу запросов";
            }
        }
        public string DownloadUnbilledDetails()
        {
            WebElement WEDownloadXLSReportLink = new WebElement().ByXPath("//*[@id='reportDetailUnbilledExcelButtonForm:excelDetailOnlineButton']/div[1]/a");
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
            string[] files = Directory.GetFiles(path, "unbilled_info.xls");

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
    }
}
