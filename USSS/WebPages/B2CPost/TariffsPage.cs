using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ApplicationServices;

namespace USSS.WebPages.B2CPost
{
    internal class TariffsPage
    {

        #region constructor

        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        public ProfilePage profilePage; 
          
        public string ConstructionPage()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id = 'postProfile']");
            
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
                                and we.template_id = " + at + @" 
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

                    WebElement weTariffFam = new WebElement().ByXPath("//a[contains(@onclick,'" + tarriffsDb + "')]");
                    WebElement weTariff = new WebElement().ByXPath("//div[contains(@id,'" + tarriffsDb + "')]");
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

        #region managerPage

        public string ChangeOfTariff(string nameTariff)
        {
            try
            {
                if (nameTariff == null)
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
                        return "Кнопка выбора тарифа заблокирована";
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
                        if (new WebElement().ByXPath("//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[contains(@class,'switch')]//button").Enabled)
                        {
                            new WebElement().ByXPath("//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[contains(@class,'switch')]//button").
                                    Click
                                    ();

                            return "success";
                        }
                        return "Кнопка выбора тарифа заблокирована";
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

        public string SuccessChangeOfTariff(ref string nameTariff, ref string number)
        {
            try
            {
                number = "0";
                if (nameTariff == null)
                {
                    WebElement btnChange = new WebElement().ByXPath(
                        "//div[contains(@class,'service-item tariff-item')][1]//button[contains(@id,'changeButton')]");
                    if (btnChange.Displayed)
                    {
                        if(btnChange.Enabled)
                        {
                            btnChange.Click();
                        }
                        else
                        {
                            return "Кнопка подтверждения смены тарифа заблокирована";
                        }
                        
                    }
                    else
                    {
                        return "Кнопка подтверждения смены тарифа не отображается";
                    }
                    Thread.Sleep(10000);
                    WebElement relsocbutton = new WebElement().ByXPath("//button[contains(@id,'relatedSocsComponent')]");
                    if (relsocbutton.Displayed)
                    {
                        relsocbutton.Click();
                    }
                            
                    nameTariff = new WebElement().ByXPath("//div[contains(@class,'service-item tariff-item')][1]//div[@class='info ']").Id;
                    string text =
                        new WebElement().ByXPath(
                            "//div[contains(@class,'service-item tariff-item')][1]//div[@class='info ']//div[@class='message message-success']")
                            .Text;
                    if (text.Contains("Заявка") &&text.Contains("на изменение тарифа")&&text.Contains("принята, за статусом можно следить в")&&text.Contains("истории заявок") )
                    {
                        number = text.Remove(0, text.IndexOf('№') + 1);
                        number = number.Remove(number.IndexOf(" на изменение"), number.Length - number.IndexOf(" на изменение"));
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
                    WebElement btnChange = new WebElement().ByXPath(
                       "//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//button[contains(@id,'changeButton')]");
                    if (btnChange.Displayed)
                    {
                        if (btnChange.Enabled)
                        {
                            btnChange.Click();
                        }
                        else
                        {
                            return "Кнопка подтверждения смены тарифа заблокирована";
                        }

                    }
                    else
                    {
                        return "Кнопка подтверждения смены тарифа не отображается";
                    }

                   
                    string text =
                        new WebElement().ByXPath(
                            "//div[contains(@id," + nameTariff + ")]//ancestor::div[1]//div[@class='info ']//div[@class='message message-success']")
                            .Text;
                    if (text.Contains("Заявка") && text.Contains("на изменение тарифа") && text.Contains("принята, за статусом можно следить в") && text.Contains("истории заявок"))
                    {
                        number = text.Remove(0, text.IndexOf('№') + 1);
                        number = number.Remove(number.IndexOf(" на изменение"), number.Length - number.IndexOf(" на изменение"));
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

        public string ViewNewTariff(string nameTariff)
        {
            try
            {
                nameTariff = nameTariff.Replace(" ", "");
                var text = new WebElement().ByXPath("//div[@id='" + nameTariff + "']/div[1]/div[2]/div/div").Text;
                //string text = teext.Text;
                if (text.Contains("Тарифный план будет изменен с") & text.Contains("текущий тариф будет отключен автоматически."))
                {
                    return "success";
                }
                else
                {
                    return "Текст не верен";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
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
                                      and to_pp = '" + nameTariff+"'";
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
                                   and to_pp = '" + nameTariff + "'";
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
                                  and to_pp = '" + nameTariff + "'";
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

        public string GoToProfile()
        {
            ProfileWE = new WebElement().ByXPath("//a[contains(@id,'postProfile')]");
            if (!ProfileWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на профиль";
            }
            ProfileWE.Click();
            profilePage = new ProfilePage();
            return profilePage.ConstructionPage();
        }
        #endregion

    }
}
