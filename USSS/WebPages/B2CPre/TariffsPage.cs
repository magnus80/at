using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2CPre
{
    internal class TariffsPage
    {
        
        #region constructor

        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        private List<string> oldSocs = new List<string>();
        private List<string> newSocs = new List<string>(); 

        public string ConstructionPage()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id = 'preProfile']");


            if (!ProfileWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на профиль"; }
            return "success";
        }

        public string CheckTariffList(string ban, string db_Ans, string db_Ms)
        {
            
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


            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            string atB = "select account_type from ecr9_billing_account where ban = " + ban;
            var qAt = Executor.ExecuteSelect(atB);
            string at = qAt[0, 0];

            string currentTariff = new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;

            var query = @"select to_price_plan, we.entity_id
                            from price_plan_change_valid@" + db_Ans + @" 
                            join web_entity we on trim(ext_entity_code) = trim(to_price_plan)
                            where trim(from_price_plan)= '" + currentTariff + @"' and to_price_plan in 
                            (select s.soc
                            from soc@" + db_Ans + @" s
                            left join market_soc_restrict@" + db_Ans + @" msr
                            on (s.market_restrict_ind = 'Y' 
                            and s.soc = msr.soc 
                            and NVL(TO_CHAR(msr.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>= " + date + " " + @"
                            and to_char(msr.effective_date,'YYYYMMDD')<=" + date + " " + @")
                            join soc_acc_restriction@" + db_Ans + @" sar
                            on (s.soc=sar.soc
                            and sar.account_types=" + at + @")
                            join product_soc_restriction@" + db_Ans + @" psr
                            on (s.soc=psr.soc
                            and psr.product_code='GVOI'
                            and NVL(TO_CHAR(psr.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" + date + " " + @"
                            and to_char(psr.effective_date,'YYYYMMDD')<=" + date + " " + @")
                            join price_plan pp on pp.external_price_plan = s.soc
                            join ecr9_price_plan_ext epp on pp.price_plan_id=epp.price_plan_id
                            where service_type='P' 
                            and soc_status='A'
                            and (epp.NON_PUBLIC_IND in " + status + ") " +
                          @"and NVL(TO_CHAR(TRUNC(s.EXPIRATION_DATE),'YYYYMMDD'),'47001231')>=" + date + " " + @"
                            and to_char(s.effective_date,'YYYYMMDD')<=" + date + " " + @"
                            and (s.market_restrict_ind IS NULL or s.market_restrict_ind ='N' or msr.market_code='VIP')
                            and NVL(TO_CHAR(TRUNC(s.sale_exp_date),'YYYYMMDD'),'47001231')>=" + date + " " + @"
                            and to_char(s.sale_eff_date,'YYYYMMDD')<=" + date + " " + @"
                            and (NOTVIEW_ADD_IND='N'or NOTVIEW_ADD_IND is null)
                            and ADD_IND='Y'
                            and view_ind='Y' 
                            and we.family_id is null)";
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
                    WebElement weTariff = new WebElement().ByXPath("//div[contains(@id,'" + tarriffsDb + "')]");
                    if (!weTariff.Displayed)
                    {
                        return "failed";
                    }
                }
                i = i + 1;
            }

            
            return "success";
        }


        #endregion

        #region managerPage

        public string ChangeOfTariff(string nameTariff)
        {
            try
            {
                if (nameTariff == null)
                {
                    if (new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").Displayed)
                    {
                            new WebElement().ByXPath("//div[contains(@class,'switch')][1]//button").
                                Click
                                ();
                        return "success";
                    }
                    else
                    {
                        return "Не отображается кнопка выбора тарифа";
                    }
                }
                else
                {
                    if (new WebElement().ByXPath("//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[contains(@class,'switch')]//button").Displayed)
                    {
                        new WebElement().ByXPath("//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[contains(@class,'switch')]//button").
                                Click
                                ();
                        return "success";
                    }
                    else
                    {
                        return "Не отображается кнопка выбора тарифа";
                    }
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private DBResult rezultQ;

        private void SaveSF(string ban, string db_Ans, string newTariff)
        {
            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            string atB = "select account_type from ecr9_billing_account where ban = " + ban;
            var qAt = Executor.ExecuteSelect(atB);
            string at = qAt[0, 0];

            string currentTariff = new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;

            var q = @"SELECT S.SOC, esf.feature_code,esf.additional_info
                        FROM SOC@" + db_Ans + @"  S
                        LEFT JOIN MARKET_SOC_RESTRICT@" + db_Ans + @"  MSR 
                        ON (S.MARKET_RESTRICT_IND = 'Y'
                        AND S.SOC = MSR.SOC
                        AND NVL(TO_CHAR(MSR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" + date + " " + @"
                        AND TO_CHAR(MSR.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + " " + @")
                        JOIN SOC_ACC_RESTRICTION@" + db_Ans + @"  SAR 
                        ON (S.SOC=SAR.SOC
                        AND SAR.ACCOUNT_TYPES='"+at+@" ')
                        JOIN SOC_RELATION@" + db_Ans + @"  SR
                        ON (S.SOC=SR.SOC_DEST
                        AND trim(SR.SOC_SRC)='"+newTariff+@"'
                        AND NVL(TO_CHAR(SR.EXPIRATION_DATE,'YYYYMMDD'),'47001231')>=" + date + " " + @")
                        join ecr9_service_feature esf on esf.soc = s.soc 
                        and esf.ban = " +ban +@"
                        WHERE SOC_STATUS='A'
                        AND NVL(TO_CHAR(TRUNC(S.EXPIRATION_DATE),'YYYYMMDD'),'47001231')>=" + date + " " + @"
                        AND TO_CHAR(S.EFFECTIVE_DATE,'YYYYMMDD')<=" + date + " " + @"
                        AND (S.MARKET_RESTRICT_IND IS NULL OR S.MARKET_RESTRICT_IND ='N' OR MSR.MARKET_CODE='VIP')
                        AND NVL(TO_CHAR(TRUNC(S.SALE_EXP_DATE),'YYYYMMDD'),'47001231')>=" + date + " " + @"
                        AND TO_CHAR(S.SALE_EFF_DATE,'YYYYMMDD')<=" + date + " " + @"
                        and s.soc in (select soc from ecr9_service_agreement where ban=" + ban + @" and (expiration_date is null or TO_CHAR(expiration_date,'YYYYMMDD') >= " + date + @"))
                        and (esf.ftr_expiration_date is null or TO_CHAR(esf.ftr_expiration_date,'YYYYMMDD')  >= " + date+")";
            rezultQ = Executor.ExecuteSelect(q);

        }

        public string  CheckRezult(string ban, string db_Ans)
        {
            var q = @"select sa.SOC, sf.feature_code,sf.additional_info from ecr9_service_agreement sa
                                left join ecr9_service_feature sf on sa.soc = sf.soc and sa.ban = sf.ban
                                where sa.ban =  " + ban + " and sa.service_type like 'O'";
            var rezult = Executor.ExecuteSelect(q);
            
            int i = 0;
            int j = 0;
            while (i < rezultQ.Count)
            {
                while (j < rezult.Count)
                {
                    if (rezultQ[i, 0].Trim(' ') == rezult[j, 0].Trim(' ') & rezultQ[i, 1].Trim(' ') == rezult[j, 1].Trim(' ') & rezultQ[i, 2].Trim(' ') == rezult[j, 2].Trim(' '))
                    {
                        break;
                    }
                    else
                    {
                        if (j + 1 == rezult.Count)
                        {
                            return "Перенос услуг некорректен";
                        }
                       
                    }
                    j = j + 1;
                }
                i = i + 1;
            }
            return "success";
        }

        public string SuccessChangeOfTariff(ref string nameTariff, ref string number,string ban, string db_Ans)
        {
                                
            try
            {
                if (nameTariff == null)
                {
                    nameTariff = new WebElement().ByXPath("//div[contains(@id,'tariffsList')]//div[contains(@class,'service-item tariff-item')][1]//div[@class='info ']").Id;
                    WebElement tar = new WebElement().ByXPath("//div[contains(@class,'service-item tariff-item')][1]//button[contains(@id,'changeButton')]");
                   //Заявка №2 147 510 530 на изменение тарифа 
                    if (tar.Displayed)
                    {
                        if (tar.Enabled)
                        {
                            SaveSF(ban, db_Ans, nameTariff);
                            tar.Click();
                            Thread.Sleep(10000);
                            WebElement relsocbutton = new WebElement().ByXPath("//button[contains(@id,'relatedSocsComponent')]");
                            if (relsocbutton.Displayed)
                            {
                                relsocbutton.Click();
                            }
                            string text =
                                new WebElement().ByXPath(
                                    "//div[contains(@class,'service-item tariff-item')][1]//div[contains(@class,'info')]//div[@class='message message-success']")
                                    .Text;
                            if (text.Contains("Заявка") && text.Contains("на изменение тарифа") &&
                                text.Contains("принята, за статусом можно следить в") && text.Contains("истории заявок"))
                            {
                                number = text.Remove(0, text.IndexOf('№') + 1);
                                number = number.Remove(number.IndexOf(" на изменение"),
                                                       number.Length - number.IndexOf(" на изменение"));
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
                        return "Кнопка заблокирована";
                    }
                    return "Кнопка не отображена";
                }
                else
                {
                    WebElement tar = new WebElement().ByXPath("//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//button[contains(@id,'changeButton')]");
                    //Заявка №2 147 510 530 на изменение тарифа 
                    if (tar.Displayed)
                    {
                        if (tar.Enabled)
                        {
                            SaveSF(ban, db_Ans, nameTariff);
                            tar.Click();
                            
                            
                            string text =
                                new WebElement().ByXPath(
                                    "//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[contains(@class,'info')]//div[@class='message message-success']")
                                    .Text;
                            if (text.Contains("Заявка") && text.Contains("на изменение тарифа") &&
                                text.Contains("принята, за статусом можно следить в") && text.Contains("истории заявок"))
                            {
                                number = text.Remove(0, text.IndexOf('№') + 1);
                                number = number.Remove(number.IndexOf(" на изменение"),
                                                       number.Length - number.IndexOf(" на изменение"));
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
                        return "Кнопка заблокирована";
                    }
                    return "Кнопка не отображена";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string ReadMassageTariff()
        {
            WebElement btn =
                new WebElement().ByXPath(
                    "//div[contains(@class,'service-item tariff-item')][1]//button[contains(@id,'changeButton')]");
            string text =
                new WebElement().ByXPath(
                    "//div[contains(@class,'service-item tariff-item')][1]//div[@class='payment-ways']")
                    .Text;
            if (
                text.Contains(
                    "К сожалению, средств на вашем счёте недостаточно. Вы можете пополнить баланс с помощью услуги") && !btn.Enabled)
            {
                return "success";
            }
            return "Текст не корректен";
        }

        public string ViewNewTariff(string nameTariff)
        {

            try
            {
                string currentTariff = new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;
                if (nameTariff == currentTariff)
                {
                    return "success";
                }
                else
                {
                    return "Тариф не отображен как текущий";
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void GetOldSocs()
        {
            oldSocs = null;
        }

        public void GetNewSocs()
        {
        }

        public string CheckSocsTranc()
        {
            return "success";
        }



        public string CheckTariffPrice(string db_Ans, string type)
        {
            if (type == "free")
            {
                int i = 1;
                WebElement price =
                    new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                             "]//div[@class='prices']//span[contains(@class,'price')]");
                while (price.Displayed |
                       new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                "]//h2[@class='family-title']").Displayed)
                {
                    if (price.Text.Replace(" руб.", "") != "0" &
                        !new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                  "]//h2[@class='family-title']").Displayed)
                    {
                        return "Стоимость перехода на тариф не равна нулю";
                    }
                    i = i + 1;
                    price =
                        new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                 "]//div[@class='prices']//span[contains(@class,'price')]");
                    // new WebElement().ByXPath("//div[contains(@class,'service-item tariff-item')][" + i + "]//div[@class='prices']//span[@class='price']");
                }
            }

            if (type == "notfree")
            {
                string currentTariff =
                    new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;
                int i = 1;
                WebElement price =
                    new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                             "]//div[@class='prices']//span[contains(@class,'price')]");
                while (price.Displayed |
                       new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                "]//h2[@class='family-title']").Displayed)
                {
                    string nameTariff =
                        new WebElement().ByXPath(
                            "//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i + "]//div[@class='info ']").Id;
                    var q = @"select nvl(por.rate, 0)
                                      from PRICE_PLAN_CHARGE ppc
                                      left join PP_OC_RATE por
                                      on por.feature_code = ppc.FEATURE_CODE
                                      and (por.expiration_date is NULL or por.expiration_date > trunc(sysdate))
                                      and por.effective_date <= trunc(sysdate)
                                      and por.soc in ('VIPDEF', 'RVIPDEF') 
                                      where ppc.effective_date <= trunc(sysdate)
                                      and (ppc.expiration_date is NULL or ppc.expiration_date > trunc(sysdate))
                                      and from_pp = '" + currentTariff + @"'
                                      and to_pp = '" +
                            nameTariff + "'";
                    var tariffCost = Executor.ExecuteSelect(q);

                    if (price.Text.Replace(" руб.", "") != tariffCost[0, 0] &
                        !new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                  "]//h2[@class='family-title']").Displayed)
                    {
                        return "Стоимость перехода на тариф некорректна";
                    }
                    i = i + 1;

                    price =
                        new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                 "]//div[@class='prices']//span[contains(@class,'price')]");
                }
            }

            if (type == "sale")
            {
                string currentTariff =
                    new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;
                int i = 1;
                WebElement price =
                    new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                             "]//div[@class='prices']//span[contains(@class,'price')]");
                while (price.Displayed |
                       new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                "]//h2[@class='family-title']").Displayed)
                {
                    string nameTariff =
                        new WebElement().ByXPath(
                            "//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i + "]//div[@class='info ']").Id;
                    var q = @"select nvl(por.rate, 0)
                                   from PRICE_PLAN_CHARGE ppc
                                   left join PP_OC_RATE por
                                   on por.feature_code = ppc.GRACE_FTR_CODE
                                   and (por.expiration_date is NULL or por.expiration_date > trunc(sysdate))
                                   and por.effective_date <= trunc(sysdate)
                                   and por.soc in ('VIPDEF', 'RVIPDEF') -- Долларовый и рублевый абоненты
                                   where ppc.effective_date <= trunc(sysdate)
                                   and (ppc.expiration_date is NULL or ppc.expiration_date > trunc(sysdate))
                                   and from_pp = '" + currentTariff + @"'
                                   and to_pp = '" +
                            nameTariff + "'";
                    var tariffCost = Executor.ExecuteSelect(q);

                    if (price.Text.Replace(" руб.", "") != tariffCost[0, 0] &
                        !new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                  "]//h2[@class='family-title']").Displayed)
                    {
                        return "Стоимость перехода на тариф некорректна";
                    }
                    i = i + 1;

                    price =
                        new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                 "]//div[@class='prices']//span[contains(@class,'price')]");
                }
            }

            if (type == "saleActive")
            {
                string currentTariff =
                    new WebElement().ByXPath("//div[@id='currentTariff']//div[contains(@class,'info ')]").Id;
                int i = 1;
                WebElement price =
                    new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                             "]//div[@class='prices']//span[contains(@class,'price')]");
                while (price.Displayed |
                       new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                "]//h2[@class='family-title']").Displayed)
                {
                    string nameTariff =
                        new WebElement().ByXPath(
                            "//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i + "]//div[@class='info ']").Id;
                    var q = @"select nvl(por.rate, 0)
                                  from PRICE_PLAN_CHARGE ppc
                                  left join PP_OC_RATE por
                                  on por.feature_code = ppc.GRACE_FTR_CODE
                                  and (por.expiration_date is NULL or por.expiration_date > trunc(sysdate))
                                  and por.effective_date <= trunc(sysdate)
                                  and por.soc in ('VIPDEF', 'RVIPDEF') -- Долларовый и рублевый абоненты
                                  where ppc.effective_date <= trunc(sysdate)
                                  and (ppc.expiration_date is NULL or ppc.expiration_date > trunc(sysdate))
                                  and from_pp = '" + currentTariff + @"'
                                  and to_pp = '" +
                            nameTariff + "'";
                    var tariffCost = Executor.ExecuteSelect(q);

                    if (price.Text.Replace(" руб.", "") != tariffCost[0, 0] &
                        !new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                  "]//h2[@class='family-title']").Displayed)
                    {
                        return "Стоимость перехода на тариф некорректна";
                    }
                    i = i + 1;

                    price =
                        new WebElement().ByXPath("//form[@action='/c/tariffs/tariffsList.xhtml']/div[" + i +
                                                 "]//div[@class='prices']//span[contains(@class,'price')]");
                }
            }
            return "success";
        }

        #endregion

    }
}
