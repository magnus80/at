using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AT;
using AT.DataBase;
using AT.WebDriver;

namespace USSS.WebPages.B2BPost
{
    class TariffChangePage:PageBase
    {
        public string priceplan;
        public FinalTariffChange finalTariffChange;
        #region constructor

        private WebElement tariffTable;// = new WebElement().ByXPath("//form[contains(@id,'changeTariffForm')]");
        private WebElement tariff;// = new WebElement().ByXPath("//div[contains(@id,'avaibleTariffs')]/div/div[1]/h4");

        private WebElement changeButton;

        private WebElement changeDialog;
        private WebElement labelStart;
        private WebElement labelToday;
        private WebElement buttonCancelCD;//CD == change dialog
        private WebElement buttonConfirmCD;

        private WebElement notificationDialog;
        private WebElement emailInput;
        private WebElement smsInput;
        private WebElement buttonCancelND;
        private WebElement buttonConfirmND;
        private WebElement textND;

        public string Construct()
        {
            Thread.Sleep(3000);
            tariffTable = new WebElement().ByXPath("//form[contains(@id,'changeTariffForm')]");
            if(!tariffTable.Displayed)
            {
                return "Не отображен элемент Таблица с тарифами";
            }
            else
            {
                return "success";
            }
        }
        public WebElement getNotificationDialog
        {
            get
            {
                notificationDialog = new WebElement().ByXPath("//div[contains(@id,'notificationMulti:notificationConfirmComponentDialog')]");
                return notificationDialog;
            }
        }
        public WebElement getTextND
        {
            get { textND = new WebElement().ByXPath("//div[contains(text(),'Подтвердите создание запроса для перехода на новый тарифный план')]");
                return textND;
            }
        }
        public WebElement getButtonCancelND
        {
            get
            {
                buttonCancelND = new WebElement().ByXPath("//div[contains(@id,'notificationMulti:notificationConfirmComponentDialog')]//button[contains(@id,'cancelSendRequestButtonNotificationComponentDialog')]");
                return buttonCancelND;
            }
        }
        public WebElement getButtonConfirmND
        {
            get
            {
                buttonConfirmND = new WebElement().ByXPath("//div[contains(@id,'notificationMulti:notificationConfirmComponentDialog')]//button[contains(@id,'sendRequestButtonNotificationComponentDialog')]");
                return buttonConfirmND;
            }
        }
        private WebElement getEmailInput
        {
            get
            {
                emailInput = new WebElement().ByXPath("//div[contains(@id,'notificationMulti:notificationConfirmComponentDialog')]//input[@id='notificationMulti:requestUserServiceParamsForm:email']");
                return emailInput;
            }
        }
        private WebElement getsmsInput
        {
            get
            {
                smsInput = new WebElement().ByXPath("//div[contains(@id,'notificationMulti:notificationConfirmComponentDialog')]//input[@id='notificationMulti:requestUserServiceParamsForm:smsInput']");
                return smsInput;
            }
        }
        private WebElement getTariff
        {
            get { tariff = new WebElement().ByXPath("//div[contains(@id,'avaibleTariffs')]/div/div[1]/h4");
                return tariff;
            }
        }
        private WebElement getChangeButton
        {
            get
            {
                changeButton = new WebElement().ByXPath("//button[contains(@onclick,'confirmChangePricePlan')]");
                return changeButton;
            }
        }
        private WebElement getChangeDialog
        {
            get { changeDialog = new WebElement().ByXPath("//div[@id='confirmChangePricePlanDialog']");
                return changeDialog;
            }
        }
        private WebElement getLabelStart
        {
            get { labelStart = new WebElement().ByXPath("//label[contains(text(),'С начала расчетного периода')]");
                return labelStart;
            }
        }
        private WebElement getLabelToday
        {
            get
            {
                labelToday = new WebElement().ByXPath("//label[contains(text(),'С текущей даты')]");
                return labelToday;
            }
        }
        private WebElement getButtonCancelCD
        {
            get { buttonCancelCD = new WebElement().ByXPath("//a/span[contains(text(),'Отменить')]");
                return buttonCancelCD;
            }
        }
        private WebElement getButtonConfirmCD
        {
            get
            {
                buttonConfirmCD = new WebElement().ByXPath("//button/span[contains(text(),'Сменить тарифный план')]");
                return buttonConfirmCD;
            }
        }
        #endregion

        #region manager page

        public string TariffSelect()
        {
            if(getTariff.Displayed)
            {
                tariff.Click();
                priceplan = tariff.Text;
                Thread.Sleep(3000);
                if(getChangeButton.Displayed)
                {
                    return "success";
                }
                else
                {
                    return "Кнопка перехода на тариф не отображена";
                }
            }
            else
            {
                return "Не отображен элемент Тариф";
            }
        }


        public string GoToNotification()
        {
            buttonConfirmCD.Click();
            
            int i;
            for (i = 0; i<5 ;i++)
            {
                if(getNotificationDialog.Displayed)
                {
                    i=5;
                }
                Thread.Sleep(3000);
            }
            if (notificationDialog.Displayed)
            {
                for (i = 0; i < 5; i++)
                {
                    if (getEmailInput.Displayed)
                    {
                        i = 5;
                    }
                    Thread.Sleep(3000);
                }
                if (!emailInput.Displayed)
                {
                    return "Не отображено поле ввода email";
                }
                if (!getsmsInput.Displayed)
                {
                    return "Не отображено поле ввода номера телефона";
                }
            }
            else
            {
                return "Не отображено окно Нотификаций";
            }

            return "success";
        }
        public string ConfirmTariff()
        {
            smsInput.SendKeys("9999999999");
            emailInput.SendKeys("avyalov@bellintegrator.ru");
            if(getButtonConfirmND.Displayed)
            {
                buttonConfirmND.Click();
                finalTariffChange = new FinalTariffChange();
                if (finalTariffChange.Construct() == "success")
                {
                    return "success";
                }
                else
                {
                    return "Не отображена страница Изменение тарифа";
                }
            }
            else
            {
                return "Не отображено кнопка Подтвердить в окне нотификаций";
            }
        }
        public string NewTariff()
        {
            WebElement newtariff = new WebElement().ByXPath("//div[contains(@id,'avaibleTariffs')]/div/div[1]//h4");
            return newtariff.Text.Replace(" ","");
        }
        public string ClickButtonGoTo()
        {
            if(changeButton.Displayed)
            {
                changeButton.Click();
                while(!getChangeButton.Enabled)
                {
                    Thread.Sleep(3000);
                }
                if(getChangeDialog.Displayed)
                {
                    if(!getLabelStart.Displayed)
                    {
                        return "Не отображен С начала расчетного периода";
                    }
                    if(!getLabelToday.Displayed)
                    {
                        return "Не отображен С текущей даты";
                    }
                    if(!getButtonCancelCD.Displayed)
                    {
                        return "Не отображена кнопка Отмена";
                    }
                    if(!getButtonConfirmCD.Displayed)
                    {
                        return "Не отображена кнопка Сменить тарифный план";
                    }
                    return "success";
                }
                else
                {
                    return "Не отображено диалоговое окно подтверждения смены тарифа";
                }

            }
            else
            {
                return "Не отображена кнопка подтверждения перехода";
            }
        }

        public string CheckTariffList(string ban, string db_Ans, string db_Ms)
        {
            var qs = "select public_ind FROM ecr9_billing_account_ext where ban like " + ban;
            var stat = Executor.ExecuteSelect(qs);
            string status = "('U')";
            /*if (stat.Count == 0)
            {
                status = "('N') or epp.NON_PUBLIC_IND is null";
            }
            else
            {
                if (stat[0, 0] == "U")
                {
                    status = "('N') or epp.NON_PUBLIC_IND is null";
                }

                if (stat[0, 0] == "A")
                {
                    status = "('N', 'Y') or epp.NON_PUBLIC_IND is null";
                }

                if (stat[0, 0] == "R")
                {
                    status = "('Y')";
                }
            }*/

            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            string atB = "select account_type from ecr9_billing_account where ban = " + ban;
            var qAt = Executor.ExecuteSelect(atB);
            string at = qAt[0, 0];

            string temp_id =
                "select * from web_templates where business_type = 'B2B' and pay_syst_type = 'POST' and entity_type = 'PP'";
            var qTemp_id = Executor.ExecuteSelect(temp_id);
            temp_id = qTemp_id[0,0];

            string currentTariff = "select soc from ecr9_service_agreement where ban = 999054597 and service_type = 'P'";
            var qCur_tar = Executor.ExecuteSelect(currentTariff);
            currentTariff = qCur_tar[0, 0];

            var query = @"select s.soc, we.entity_id
                                from soc@" + db_Ans + @" s
                                left join market_soc_restrict@" + db_Ans + @" msr
                                on (s.market_restrict_ind = 'Y' 
                                and s.soc = msr.soc 
                                and NVL(TO_CHAR(msr.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" + date +
                             @" and to_char(msr.effective_date,'YYYYMMDD')<=" + date + @")
                                join soc_acc_restriction@" + db_Ans + @" sar
                                on (s.soc=sar.soc
                                and sar.account_types=" + at + @")
                                join product_soc_restriction@" + db_Ans + @" psr
                                on (s.soc=psr.soc
                                and psr.product_code='GVOI'
                                and NVL(TO_CHAR(psr.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" + date +
                             @" and to_char(psr.effective_date,'YYYYMMDD')<=" + date + @")
                                join price_plan pp on pp.external_price_plan = s.soc
                                join ecr9_price_plan_ext epp on pp.price_plan_id=epp.price_plan_id
                                join web_entity we on trim(ext_entity_code) = trim(s.soc)
                                where service_type='P' 
                                and we.family_id is null
                                and soc_status='A'
                                and we.template_id = " + temp_id + @" 
                                and NVL(TO_CHAR(TRUNC(s.EXPIRATION_DATE),'YYYYMMDD'),'47001231')>=" + date +
                             @" and to_char(s.effective_date,'YYYYMMDD')<=" + date +
                             @" and (s.market_restrict_ind IS NULL or s.market_restrict_ind ='N' or msr.market_code='VIP')
                                and NVL(TO_CHAR(TRUNC(s.sale_exp_date),'YYYYMMDD'),'47001231')>=" + date +
                             @" and to_char(s.sale_eff_date,'YYYYMMDD')<=" + date +
                             @" and ADD_IND='Y'
                                and view_ind='Y' 
                                and (NOTVIEW_ADD_IND='N'or NOTVIEW_ADD_IND is null)
                                and ub_pp_type = (select ub_pp_type from soc@" + db_Ans + @" where trim(soc) like '" + currentTariff + @"' and rownum<2)
                                and (NON_PUBLIC_IND in " + status + ") ";
            var tariffsQ = Executor.ExecuteSelect(query);


            int i = 0;
            while (i < tariffsQ.Count)
            {
                string tarriffsDb = tariffsQ[i, 0];
                while (tarriffsDb.IndexOf(' ') != -1)
                {
                    tarriffsDb = tarriffsDb.Remove(tarriffsDb.IndexOf(' '), 1);
                }
                string tarrifDbId = tariffsQ[i, 1];
                var q = @"SELECT  * FROM " + db_Ms + ".WEB_ENTITY_PARAM WHERE param_id =  100000082 and entity_id = " + tarrifDbId;
                var tariffsV = Executor.ExecuteSelect(q);
                if (tariffsV.Count == 0)
                {

                    WebElement weTariffFam = new WebElement().ByXPath("//h4[contains(@class,'" + tarriffsDb + "')]");
                    WebElement weTariff = new WebElement().ByXPath("//div[contains(@onclick,'" + tarriffsDb + "')]");
                    if (!weTariff.Displayed && !weTariffFam.Displayed)
                    {
                        return "failed";
                    }
                }
                i = i + 1;
            }

            return "success";
        }

        #endregion
    }
}
