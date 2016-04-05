using AT;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using USSS.WebPages.CommonPages;

namespace USSS.WebPages.B2BPle
{
    internal class ManagerContractPage:PageBase
    {

        #region constructor

        //таблица абонентов
        private WebElement AbonentsNumberWE;
        private WebElement DistributionPaymentWE;
        public DistributionPaymentPage DistributionPaymentPage;
        public AbonentProfilePage abonentProfilePage;

        public string ConstructionPage()
        {
            Thread.Sleep(10000);
            AbonentsNumberWE = new WebElement().ByXPath("//div[@id = 'mobileDataForm:abonents']//table");
            DistributionPaymentWE = new WebElement().ByXPath("//*[contains(@id,'mobileButtons') and contains(text(),'Распределение баланса')][1]");

            if (!AbonentsNumberWE.Displayed) { return "Не отображены элементы интерфейса: список абонентов"; }
            if (!DistributionPaymentWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Распределить баланс"; }

            return "success";
        }


        #endregion

        public string GoToSubscription()
        {
            try
            {
                new WebElement().ByXPath("//a[contains(@href,'/faces/info/subscriberDetail.html?objId')][1]").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SelectNumbers()
        {
            try
            {
                new WebElement().ByXPath("//div[contains(@class,'ui-chkbox-box ui-widget ui-corner-all ui-state-default')][1]").Click();
                Thread.Sleep(10000);
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToProfileNumber(string number)
        {
            try
            {
                WebElement numberTb = new WebElement().ByXPath("//tr[@data-rk='"+number+"']/td[@class='custom-heading column-block']/a");
                if (numberTb.Displayed)
                {
                    numberTb.Click();
                    abonentProfilePage = new AbonentProfilePage();
                    abonentProfilePage.ConstructionPage();
                }
                else
                {
                    return "Абонент отсутствует в таблице";
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

       // 


        public string onClickDistributionPayment()
        {
            try
            {
                DistributionPaymentWE = new WebElement().ByXPath("//a[contains(@id,'mobileButtons') and contains(text(),'Распределение баланса')]");
                if (!DistributionPaymentWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Распределить баланс"; }
                DistributionPaymentWE.Click();
                if (new WebElement().ByXPath("//div[@id='unavaibleForDistributionAbonentsListDialog']").Displayed)
                {
                    return "lookusers";
                }
                DistributionPaymentPage = new DistributionPaymentPage();
                return DistributionPaymentPage.ConstructionPage();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void SelectPeriod(string period)
        {
            new WebElement().ByXPath("//div[contains(@class,'select-one-button')]//a[contains(text(),'"+ period+"')]").Click();
        }

        public void GetReport()
        {
              //new WebElement().ByXPath("//button[contains(@id,'pleCallDetalizationForm')]//span[contains(text(),'Заказать детализацию')]").Click();
              //new WebElement().ByXPath("//input[@id='notificationReportDetalization:requestUserServiceParamsForm:email']").SendKeys("KPodberezin@bellintegrator.ru");
              //new WebElement().ByXPath("//input[@id='notificationReportDetalization:requestUserServiceParamsForm:smsInput']").SendKeys("9272882753");
              //new WebElement().ByXPath("//button[contains(@id,'notificationReportDetalization:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog')]").Click();
              //new WebElement().ByXPath("//a[@href='/faces/info/requests/requests.html']").Click();
              //RequestHistoryPage rhp = new RequestHistoryPage();
              //int i = 0;
              //while (rhp.ChangeOfStatus("Обработан") != "success")
              //{
              //    i = i + 1;
              //    new WebElement().ByXPath("//a[contains(@onclick,'update')]").Click();
              //    Thread.Sleep(5000);
              //    if (i > 2)
              //    {
              //        return;
              //    }
              //}
              //new WebElement().ByXPath("//table/tbody//tr[@data-ri = 0]//a[contains(@onclick,'PrimeFaces.addSubmitParam')]").Click();
        }
    }
}
