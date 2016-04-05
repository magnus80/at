using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2CPost
{
    internal class ServicesPage
    {

        #region constructor

        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        private WebElement AllServices;
        private WebElement AvailableSevices;
        private WebElement ConnectedServices;

        public string ConstructionPage()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id='postProfile']");
            AllServices = new WebElement().ByXPath("//div[@class='service-types']//a[contains(@onclick,'ALL')]");
            AvailableSevices = new WebElement().ByXPath("//div[@class='service-types']//a[contains(@onclick,'AVAILABLE')]");
            ConnectedServices = new WebElement().ByXPath("//div[@class='service-types']//a[contains(@onclick,'CONNECTED')]");

            if (!ProfileWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на профиль"; }
            if (!AllServices.Displayed) { return "Не отображены элементы интерфейса: фильтр Все услуги"; }
            if (!AvailableSevices.Displayed) { return "Не отображены элементы интерфейса: фильтр Доступные услуги"; }
            if (!ConnectedServices.Displayed) { return "Не отображены элементы интерфейса: фильтр Подключенные услуги"; }
         
            return "success";
        }

        public string CheckFiltr()
        {
            WebElement CheckedFiltr = new WebElement().ByXPath("//div[@class='service-types']//a[@class='ui-commandlink ui-widget marked']");
            if (CheckedFiltr.Text != "Все услуги") { return "Не верный фильтр услуг по умолчанию"; }
            return "success";
        }

        #endregion

        #region managerPage

        public string GoToAvailableSevices(string ban, string db_Ans, string currentTariff, string db_Ms)
        {
            AvailableSevices = new WebElement().ByXPath("//div[@class='service-types']//a[contains(@onclick,'AVAILABLE')]");
            if (!AvailableSevices.Displayed) { return "Не отображены элементы интерфейса: фильтр Доступные услуги"; }
            AvailableSevices.Click();
 
            if (!(new WebElement().ByXPath("//form[@class='profile-balance regular-page-balance']")).Displayed) { return "Не отображены элементы интерфейса: Расходы по номеру"; }
            if (!(new WebElement().ByXPath("//input[@id='searchForm:searchInput']")).Displayed) { return "Не отображены элементы интерфейса: Поиск по услугам"; }
            if (!(new WebElement().ByXPath("//div[@id='servicesForm:services']")).Displayed) { return "Не отображены элементы интерфейса: Блок доступных услуг"; }



            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            string atB = "select account_type from ecr9_billing_account where ban = " + ban;
            var qAt = Executor.ExecuteSelect(atB);
            string at = qAt[0, 0];

            var qs = "select public_ind FROM ecr9_billing_account_ext where ban like " + ban;
            var stat = Executor.ExecuteSelect(qs);
            string status = "";
            if (stat.Count == 0)
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
            }

            var q = @"SELECT s.soc, we.entity_id,wc.category_name
                        FROM SOC@" + db_Ans + @"  S
                        LEFT JOIN MARKET_SOC_RESTRICT@" +
                    db_Ans + @"  MSR 
                        ON (S.MARKET_RESTRICT_IND = 'Y'
                        AND S.SOC = MSR.SOC
                        AND NVL(TO_CHAR(MSR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" +
                    date + @"
                        AND TO_CHAR(MSR.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + @" )
                        JOIN SOC_ACC_RESTRICTION@" +
                    db_Ans + @"  SAR 
                        ON (S.SOC=SAR.SOC
                        AND SAR.ACCOUNT_TYPES='" + at + @" ')
                        JOIN SOC_RELATION@" +
                    db_Ans + @"  SR
                        ON (S.SOC=SR.SOC_DEST
                        AND trim(SR.SOC_SRC)='" + currentTariff + @"'
                        AND NVL(TO_CHAR(SR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" +
                    date + @" )
                        join price_plan pp on pp.external_price_plan = s.soc
                        join ecr9_price_plan_ext epp on pp.price_plan_id=epp.price_plan_id
                        join web_entity  we on trim(we.Ext_Entity_Code) = trim(s.soc)
                        join web_category_link  wcl on we.entity_id = wcl.entity_id 
                        join web_category wc on wc.category_id = wcl.category_id
                        WHERE SOC_STATUS='A'
                        AND NVL(TO_CHAR(TRUNC(S.EXPIRATION_DATE),'YYYYMMDD'),'47001231')>=" +
                    date + @"
                        AND TO_CHAR(S.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + @" 
                        AND (S.MARKET_RESTRICT_IND IS NULL OR S.MARKET_RESTRICT_IND ='N' OR MSR.MARKET_CODE='VIP')
                        AND NVL(TO_CHAR(TRUNC(S.SALE_EXP_DATE),'YYYYMMDD'),'47001231')>=" +
                    date + @" 
                        and (NOTVIEW_ADD_IND='N'or NOTVIEW_ADD_IND is null)
                        and ADD_IND='Y'
                        and view_ind='Y'
                        and we.template_id = 
                        (select template_id from web_templates where business_type='B2C' and pay_syst_type='POST' 
                        and entity_type='SOC')
                        and (NON_PUBLIC_IND in " + status + @") 
                        AND TO_CHAR(S.SALE_EFF_DATE,'YYYYMMDD')<=" + date;
                     
            var servicesQ = Executor.ExecuteSelect(q);


            int i = 0;
            while (i < servicesQ.Count)
            {
                string servicesDb = servicesQ[i, 0];
                while (servicesDb.IndexOf(' ') != -1)
                {
                    servicesDb = servicesDb.Remove(servicesDb.IndexOf(' '), 1);
                }
                string serviceDbId = servicesQ[i, 1];
                var qp = @"SELECT  * FROM " + db_Ms + ".WEB_ENTITY_PARAM WHERE param_id =  100000082 and entity_id = " +
                         serviceDbId;
                var serviceV = Executor.ExecuteSelect(qp);
                if (serviceV.Count == 0)
                {
                    string categoryDb = servicesQ[i, 2];
                    if (!(new WebElement().ByXPath("//div[contains(@id,'servicesCategories')]//span[contains(text(),'" + categoryDb + "')]")).Displayed) { return "Не отображены элементы интерфейса: категория " + categoryDb; }
                }
                i = i + 1;
            }

            return "success";
        }

        public string GoTConnectedServices()
        {
            ConnectedServices = new WebElement().ByXPath("//div[@class='service-types']//a[contains(@onclick,'CONNECTED')]");
            if (!ConnectedServices.Displayed) { return "Не отображены элементы интерфейса: фильтр Подключенные услуги"; }
            ConnectedServices.Click();
            if (!(new WebElement().ByXPath("//form[@class='profile-balance regular-page-balance']")).Displayed) { return "Не отображены элементы интерфейса: Расходы по номеру"; }
            if (!(new WebElement().ByXPath("//input[@id='searchForm:searchInput']")).Displayed) { return "Не отображены элементы интерфейса: Поиск по услугам"; }
            if (!(new WebElement().ByXPath("//div[@id='servicesForm:services']")).Displayed) { return "Не отображены элементы интерфейса: Блок доступных услуг"; }

            return "success";
        }

        public string ClickCategory(string ban, string db_Ans, string currentTariff, string db_Ms, string category)
        {
            WebElement mi = new WebElement().ByXPath("//div[contains(@id,'servicesCategories')]//span[contains(text(),'"+ category+"')]");
            if (!mi.Displayed) { return "Не отображены элементы интерфейса: категория " + category; }
            mi.Click();

            return CheckServices(ban, db_Ans, currentTariff, db_Ms, category);
        }


        private string CheckServices(string ban, string db_Ans, string currentTariff, string db_Ms, string category)
        {
            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            string atB = "select account_type from ecr9_billing_account where ban = " + ban;
            var qAt = Executor.ExecuteSelect(atB);
            string at = qAt[0, 0];

            var qs = "select public_ind FROM ecr9_billing_account_ext where ban like " + ban;
            var stat = Executor.ExecuteSelect(qs);
            string status = "";
            if (stat.Count == 0)
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
            }

            var q = @"SELECT s.soc, we.entity_id
                        FROM SOC@" + db_Ans + @"  S
                        LEFT JOIN MARKET_SOC_RESTRICT@" +
                    db_Ans + @"  MSR 
                        ON (S.MARKET_RESTRICT_IND = 'Y'
                        AND S.SOC = MSR.SOC
                        AND NVL(TO_CHAR(MSR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" +
                    date + @"
                        AND TO_CHAR(MSR.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + @" )
                        JOIN SOC_ACC_RESTRICTION@" +
                    db_Ans + @"  SAR 
                        ON (S.SOC=SAR.SOC
                        AND SAR.ACCOUNT_TYPES='" + at + @" ')
                        JOIN SOC_RELATION@" +
                    db_Ans + @"  SR
                        ON (S.SOC=SR.SOC_DEST
                        AND trim(SR.SOC_SRC)='" + currentTariff + @"'
                        AND NVL(TO_CHAR(SR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" +
                    date + @" )
                        join price_plan pp on pp.external_price_plan = s.soc
                        join ecr9_price_plan_ext epp on pp.price_plan_id=epp.price_plan_id
                        join web_entity  we on trim(we.Ext_Entity_Code) = trim(s.soc)
                        join web_category_link  wcl on we.entity_id = wcl.entity_id 
                        join web_category wc on wc.category_id = wcl.category_id
                        WHERE SOC_STATUS='A'
                        AND NVL(TO_CHAR(TRUNC(S.EXPIRATION_DATE),'YYYYMMDD'),'47001231')>=" +
                    date + @"
                        and wc.category_name = '"+category+@"'
                        AND TO_CHAR(S.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + @" 
                        AND (S.MARKET_RESTRICT_IND IS NULL OR S.MARKET_RESTRICT_IND ='N' OR MSR.MARKET_CODE= (select market_code  from ecr9_billing_account where ban ="+ban+@"))
                        AND NVL(TO_CHAR(TRUNC(S.SALE_EXP_DATE),'YYYYMMDD'),'47001231')>=" +
                    date + @" 
                        and (NOTVIEW_ADD_IND='N'or NOTVIEW_ADD_IND is null)
                        and ADD_IND='Y'
                        and view_ind='Y'
                        and we.template_id = 
                        (select template_id from web_templates where business_type='B2C' and pay_syst_type='POST' 
                        and entity_type='SOC') 
                        and (NON_PUBLIC_IND in " + status + @") 
                        AND TO_CHAR(S.SALE_EFF_DATE,'YYYYMMDD')<=" + date;
                     
            var servicesQ = Executor.ExecuteSelect(q);


            int i = 0;
            while (i < servicesQ.Count)
            {
                string servicesDb = servicesQ[i, 0];
                while (servicesDb.IndexOf(' ') != -1)
                {
                    servicesDb = servicesDb.Remove(servicesDb.IndexOf(' '), 1);
                }
                string serviceDbId = servicesQ[i, 1];
                var qp = @"SELECT  * FROM " + db_Ms + ".WEB_ENTITY_PARAM WHERE param_id =  100000082 and entity_id = " + serviceDbId;
                var serviceV = Executor.ExecuteSelect(qp);
                if (serviceV.Count == 0)
                {

                    WebElement weServiceFam = new WebElement().ByXPath("//a[contains(@onclick,'" + servicesDb + "')]");
                    WebElement weServiceTariff = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]");
                    if (!weServiceFam.Displayed && !weServiceTariff.Displayed)
                    {
                        return "Не отображается услуга " + servicesDb;
                    }

                    var qnam = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " +serviceDbId+@"  
                                 and wep.param_name_id = '100000001'";// or wep.param_name_id = '100000002' or wep.param_name_id = '100000003') ";
                    var anam = Executor.ExecuteSelect(qnam);
                  
                    string nam = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//div[@class='ui-outputpanel ui-widget descr']").Text.Trim(' ');
                    while (nam.IndexOf(' ') != -1)
                    {
                        nam = nam.Remove(nam.IndexOf(' '), 1);
                    }
                    if (anam.Count != 0)
                    {
                        string namdb = anam[0, 0];

                        while (namdb.IndexOf(' ') != -1)
                        {
                            namdb = namdb.Remove(namdb.IndexOf(' '), 1);
                        }
                        if (nam != namdb)
                        {
                            return "Неверное описание услуги" + servicesDb;
                        }
                    }
                    var qab = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " + serviceDbId + @"  
                                 and wep.param_name_id = '100000002'";
                    var ab = Executor.ExecuteSelect(qab);
                    WebElement nab = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//span[contains(@class,'price')][1]");
                    string s = nab.Text.Replace(" руб.", "");
                    if (ab.Count != 0)
                    {
                        if (s != ab[0, 0])
                        {
                            return "Неверная абоненская плата услуги" + servicesDb;
                        }
                    }
                    else
                    {
                        if (nab.Displayed)
                        {
                            return "Неверная абоненская плата услуги" + servicesDb;
                        }
                    }
                    var qcon = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " + serviceDbId + @"  
                                 and wep.param_name_id = '100000003'";
                    var acon = Executor.ExecuteSelect(qcon);
                    WebElement ncon = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//div[contains(@class,'ui-outputpanel ui-widget prices')]/div[2]");
                    if (acon.Count != 0)
                    {
                        if (ncon.Text.Replace(" руб. подключение", "") != acon[0, 0])
                        {
                            return "Неверная стоимость подключения услуги" + servicesDb;
                        }
                    }
                    else
                    {
                        if (ncon.Displayed)
                        {
                            return "Неверная стоимость подключения услуги" + servicesDb;
                        }
                    }
                    if (!(new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//a[@class='ui-commandlink ui-widget dynamic description-link']").Displayed))
                    {
                        return "Не отображается ссылка на подробное описание" + servicesDb;
                    }
                }
                i = i + 1;
            }
            return "success";
        }

        public string CheckConnectedServices(string ban, string db_Ans, string db_Ms, string phoneNumber)
        {
            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];
         

            var q = @"SELECT   s.soc, we.entity_id
                        FROM ecr9_service_agreement  S
                        join price_plan pp on pp.external_price_plan = s.soc
                        join ecr9_price_plan_ext epp on pp.price_plan_id=epp.price_plan_id
                        join web_entity  we on trim(we.Ext_Entity_Code) = trim(s.soc)
                        WHERE
                        (TO_CHAR(TRUNC(S.EXPIRATION_DATE),'YYYYMMDD')>="+ date+@" or S.EXPIRATION_DATE is null)
                        and (NOTVIEW_ADD_IND='N'or NOTVIEW_ADD_IND is null)
                        and ADD_IND='Y'
                        and view_ind='Y'
                        and s.subscriber_no =  " + phoneNumber + @"
                        and s.service_type like 'O'
                        and we.template_id = 
                        (select template_id from web_templates where business_type='B2C' and pay_syst_type='POST' 
                        and entity_type='SOC')";

            var servicesQ = Executor.ExecuteSelect(q);


            int i = 0;
            while (i < servicesQ.Count)
            {
                string servicesDb = servicesQ[i, 0];
                while (servicesDb.IndexOf(' ') != -1)
                {
                    servicesDb = servicesDb.Remove(servicesDb.IndexOf(' '), 1);
                }
                string serviceDbId = servicesQ[i, 1];
                var qp = @"SELECT  * FROM " + db_Ms + ".WEB_ENTITY_PARAM WHERE param_id =  100000082 and entity_id = " + serviceDbId;
                var serviceV = Executor.ExecuteSelect(qp);
                if (serviceV.Count == 0)
                {

                    WebElement weServiceFam = new WebElement().ByXPath("//a[contains(@onclick,'" + servicesDb + "')]");
                    WebElement weServiceTariff = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]");
                    if (!weServiceFam.Displayed && !weServiceTariff.Displayed)
                    {
                        return "Не отображается услуга " + servicesDb;
                    }

                    var qnam = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " + serviceDbId + @"  
                                 and wep.param_name_id = '100000001'";// or wep.param_name_id = '100000002' or wep.param_name_id = '100000003') ";
                    var anam = Executor.ExecuteSelect(qnam);

                    string nam = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//div[@class='ui-outputpanel ui-widget descr']").Text.Trim(' ');
                    while (nam.IndexOf(' ') != -1)
                    {
                        nam = nam.Remove(nam.IndexOf(' '), 1);
                    }
                    if (anam.Count != 0)
                    {
                        string namdb = anam[0, 0];
                        while (namdb.IndexOf(' ') != -1)
                        {
                            namdb = namdb.Remove(namdb.IndexOf(' '), 1);
                        }
                        if (nam != namdb)
                        {
                            return "Неверное описание услуги" + servicesDb;
                        }
                    }
                    var qab = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " + serviceDbId + @"  
                                 and wep.param_name_id = '100000002'";
                    var ab = Executor.ExecuteSelect(qab);
                    WebElement nab = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//span[contains(@class,'price')][1]");
                    if (ab.Count != 0)
                        if (nab.Text.Replace(" руб.", "") != ab[0, 0])
                        {
                            return "Неверная абоненская плата услуги" + servicesDb;
                        }
                        else
                        {
                            if (nab.Text.Replace(" руб.", "") != "0")
                            {
                                return "Неверная абоненская плата услуги" + servicesDb;
                            }
                        }
                    else
                    {
                        if (nab.Displayed)
                            return "Неверная стоимость подключения услуги" + servicesDb;
                    }
                    var qcon = @"select param_value from web_entity we
                                 join web_entity_param wep on we.entity_id = wep.entity_id
                                 where we.entity_id = " + serviceDbId + @"  
                                 and wep.param_name_id = '100000003'";
                    var acon = Executor.ExecuteSelect(qcon);
                    WebElement ncon = new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//div[contains(@class,'ui-outputpanel ui-widget prices')]/div[2]");
                    if (acon.Count != 0)
                        if (ncon.Text.Replace(" руб. подключение", "") != acon[0, 0])
                        {
                            return "Неверная стоимость подключения услуги" + servicesDb;
                        }
                        else
                        {
                            if (ncon.Text.Replace(" руб. подключение", "") != "0")
                            {
                                return "Неверная стоимость подключения услуги" + servicesDb;
                            }
                        }
                    else
                    {
                        if (ncon.Displayed)
                        return "Неверная стоимость подключения услуги" + servicesDb;  
                    }
                    if (!(new WebElement().ByXPath("//div[contains(@id,'" + servicesDb + "')]//a[@class='ui-commandlink ui-widget dynamic description-link']").Displayed))
                    {
                        return "Не отображается ссылка на подробное описание" + servicesDb;
                    }
                }
                i = i + 1;
            }
            return "success";
        }

        public string ConnectOfService(string nameService)
        {
            try
            {
                if (nameService == null)
                {
                    if (new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").Displayed)
                    {
                        if (new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").Enabled)
                        {

                            new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").
                                Click
                                ();

                            return "success";
                        }
                        return "Кнопка выбора услуги заблокирована";
                    }
                    else
                    {
                        return "Не отображается кнопка выбора услуги";
                    }
                }
                else
                {
                    if (new WebElement().ByXPath("//div[contains(@id,'"+nameService+"')]//div[contains(@class,'switch')]//button").Displayed)
                    {
                        if (new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'switch')]//button").Enabled)
                        {

                            new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'switch')]//button").
                                Click
                                ();

                            return "success";
                        }
                        return "Кнопка выбора услуги заблокирована";
                    }
                    else
                    {
                        return "Не отображается кнопка выбора услуги";
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SuccessConnectOfService(ref string nameService, ref string number, string db_Ans)
        {
            try
            {
                if (nameService == null)
                {
                    var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
                    var tm = Executor.ExecuteSelect(t);
                    string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
                    var d = dateB.Split('.');
                    string date = d[0] +"."+ d[1] +"."+ d[2].Remove(0,2);

                    WebElement startDate = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//span[contains(@id,'effectiveDate')]//input");
                    WebElement btnCancel = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//a[contains(@id,'notConnectorBtn')]");

                    
                    if (startDate.Displayed)
                    {
                        if (startDate.Value != date)
                        {
                            return "Дата начала периода по умолчанию некорректна";
                        }
                    }
                    if (!btnCancel.Displayed){ return "Не отображается псевдоссылка отмены подключения услуги"; }

                    WebElement btnChange = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//button[contains(@id,'connectorBtn')]");
                    if (btnChange.Displayed)
                    {
                        if (btnChange.Enabled)
                        {
                            btnChange.Click();
                        }
                        else
                        {
                            return "Кнопка подтверждения подключения заблокирована";
                        }

                    }
                    else
                    {
                        return "Кнопка подтверждения смены услуги не отображается";
                    }

                    nameService = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]").Id;
                    string text =
                        new WebElement().ByXPath(
                            "//div[contains(@class,'service-item')][1]//div[@class='ui-outputpanel ui-widget connectorResults message']")
                            .Text;
                   // Запрос 2147677336 на подключение услуги "GPRS-пакет в международном роуминге 100 Мб" успешно отправлен. За статусом запроса следите в истории заявок
                    if (text.Contains("Запрос") && text.Contains("на подключение услуги") && text.Contains("успешно отправлен."))//" За статусом запроса следите в") && text.Contains("истории заявок"))
                    {
                        number = text.Remove(0, text.IndexOf(' ') + 1);
                        number = number.Remove(number.IndexOf(" на подключение"), number.Length - number.IndexOf(" на подключение"));
                        while (number.IndexOf(' ') != -1)
                        {
                            number = number.Remove(number.IndexOf(' '), 1);
                        }
                        return "success";
                    }
                    else
                    {
                        return "Текст не верен";
                    }
                }
                else
                {
                    var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
                    var tm = Executor.ExecuteSelect(t);
                    string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
                    var d = dateB.Split('.');
                    string date = d[0] + "." + d[1] + "." + d[2].Remove(0, 2);

                    WebElement startDate = new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'service-item')]//span[contains(@id,'effectiveDate')]//input");
                    WebElement btnCancel = new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'service-item')]//a[contains(@id,'notConnectorBtn')]");


                    if (startDate.Displayed)
                    {
                        if (startDate.Value != date)
                        {
                            return "Дата начала периода по умолчанию некорректна";
                        }
                    }
                    if (!btnCancel.Displayed) { return "Не отображается псевдоссылка отмены подключения услуги"; }

                    WebElement btnChange = new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'service-item')]//button[contains(@id,'connectorBtn')]");
                    if (btnChange.Displayed)
                    {
                        if (btnChange.Enabled)
                        {
                            btnChange.Click();
                        }
                        else
                        {
                            return "Кнопка подтверждения подключения заблокирована";
                        }

                    }
                    else
                    {
                        return "Кнопка подтверждения смены услуги не отображается";
                    }

                    nameService = new WebElement().ByXPath("//div[contains(@id,'" + nameService + "')]//div[contains(@class,'service-item')]").Id;
                    string text =
                        new WebElement().ByXPath(
                            "//div[contains(@id,'" + nameService + "')]//div[contains(@class,'service-item')]//div[@class='ui-outputpanel ui-widget connectorResults message']")
                            .Text;
                    // Запрос 2147677336 на подключение услуги "GPRS-пакет в международном роуминге 100 Мб" успешно отправлен. За статусом запроса следите в истории заявок
                    if (text.Contains("Запрос") && text.Contains("на подключение услуги") && text.Contains("успешно отправлен. За статусом запроса следите в") && text.Contains("истории заявок"))
                    {
                        number = text.Remove(0, text.IndexOf(' ') + 1);
                        number = number.Remove(number.IndexOf(" на подключение"), number.Length - number.IndexOf(" на подключение"));
                        while (number.IndexOf(' ') != -1)
                        {
                            number = number.Remove(number.IndexOf(' '), 1);
                        }
                        return "success";
                    }
                    else
                    {
                        return "Текст не верен";
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DisconnectOfService(string nameService)
        {
            try
            {
                if (new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").Displayed)
                {
                    if (new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").Enabled)
                    {
                        if (nameService == null)
                            new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").
                                Click
                                ();

                        return "success";
                    }
                    return "Кнопка выбора услуги заблокирована";
                }
                else
                {
                    return "Не отображается кнопка выбора услуги";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SuccessDisconnectOfService(ref string nameService, ref string number, string db_Ans)
        {
            Thread.Sleep(5000);
            try
            {
                if (nameService == null)
                {
                    var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
                    var tm = Executor.ExecuteSelect(t);
                    string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
                    var d = dateB.Split('.');
                    string date = d[0] + "." + d[1] + "." + d[2].Remove(0, 2);

                    WebElement endDate = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//span[contains(@id,'disconnectingSocDate')]//input");
                    WebElement btnCancel = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//a[contains(text(),'Не отключать')]");

                   
                    if (!endDate.Displayed) { return "Не отображается поле ввода даты отключения"; }
                    if (endDate.Value != date) { return "Дата отключения по умолчанию некорректна"; }
                    if (!btnCancel.Displayed) { return "Не отображается псевдоссылка отмены подключения услуги"; }

                    WebElement btnChange = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]//button[contains(@id,'dscntBtn')]");
                    if (btnChange.Displayed)
                    {
                        if (btnChange.Enabled)
                        {
                            btnChange.Click();
                        }
                        else
                        {
                            return "Кнопка подтверждения подключения заблокирована";
                        }

                    }
                    else
                    {
                        return "Кнопка подтверждения смены услуги не отображается";
                    }

                    nameService = new WebElement().ByXPath("//div[contains(@class,'service-item')][1]").Id;
                    string text =
                        new WebElement().ByXPath(
                            "//div[contains(@class,'service-item')][1]//div[@class='ui-outputpanel ui-widget disconnectorResults message']")
                            .Text;
                    // Запрос 2147677532 на отключение услуги "GPRS-пакет в международном роуминге 100 Мб" успешно отправлен. За статусом запроса следите вистории заявок
                    if (text.Contains("Запрос") && text.Contains("на отключение услуги") && text.Contains("успешно отправлен. За статусом запроса следите в") && text.Contains("истории заявок"))
                    {
                        number = text.Remove(0, text.IndexOf(' ') + 1);
                        number = number.Remove(number.IndexOf(" на отключение"), number.Length - number.IndexOf(" на отключение"));
                        while (number.IndexOf(' ') != -1)
                        {
                            number = number.Remove(number.IndexOf(' '), 1);
                        }
                        return "success";
                    }
                    else
                    {
                        return "Текст не верен";
                    }
                }

                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }
}
