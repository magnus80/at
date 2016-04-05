using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USSS.WebPages.B2BPle
{
    internal class DistributionPaymentPage
    {
        #region constructor

        //таблица абонентов
        private WebElement EvenlyWE;
        private WebElement DifferentSumWE;
        private WebElement OneSumToAllWE;

        private WebElement DistributionWE;
        private WebElement SaveTemplateWE;
        private WebElement CancelWE;

        private WebElement RepeatDistributionWE;
        private WebElement FinishSumPanelWE;
        private WebElement DistributionBalanceWE;
        private WebElement AbonentsListWE;
        private WebElement EvenlySumInputWE;
        private WebElement ContractInfo;
        private WebElement BackMCP;
        public string ConstructionPage()
        {//Равномерное

            EvenlyWE = new WebElement().ByXPath("//*[text()='Равномерное']");
            DifferentSumWE = new WebElement().ByXPath("//*[text()='Одна сумма для всех']");
            OneSumToAllWE = new WebElement().ByXPath("//*[text()='Различные суммы']");

            DistributionWE = new WebElement().ByXPath("//a/span[contains(text(),'Распределить баланс')]");
            SaveTemplateWE = new WebElement().ByXPath("//a/span[contains(text(),'Сохранить шаблон распределения')]");
            CancelWE = new WebElement().ByXPath("//a/span[contains(text(),'Отменить распределение')]");

            RepeatDistributionWE = new WebElement().ByXPath("//div[contains(@id,'repeatCheckBox')]");
            FinishSumPanelWE = new WebElement().ByXPath("//div[contains(@id,'finishSumPanel')]");
            DistributionBalanceWE = new WebElement().ByXPath("//*[@id='distributionBalance']/div[1]/div[2]/div/span");
            AbonentsListWE = new WebElement().ByXPath("//tbody[@id='distributionBalance:abonents_data']");
            EvenlySumInputWE = new WebElement().ByXPath("//input[contains(@id,'distributionBalance') and contains(@id,'umInput')]");
            ContractInfo = new WebElement().ByXPath("//*[@id='distributionBalance']/div[1]/div[2]");//*[@id="distributionBalance"]/div[1]/div[2]/text()
            BackMCP = new WebElement().ByXPath("//*[@id='distributionBalance']/div[4]/div[2]/a");

            if (!EvenlyWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Равномерное"; }
            if (!DifferentSumWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Одна сумма на всех"; }
            if (!OneSumToAllWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Различные суммы"; }

            if (!DistributionWE.Displayed) { return "Не отображены элементы интерфейса: кнопка Распределить баланс"; }
            if (!SaveTemplateWE.Displayed) { return "Не отображены элементы интерфейса: список Сохранить шаблон"; }
            if (!CancelWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Отменить распределение"; }
            if (!RepeatDistributionWE.Displayed) { return "Не отображены элементы интерфейса: чекбокс Повторять распределение"; }
            if (!FinishSumPanelWE.Displayed) { return "Не отображены элементы интерфейса: Итоговая сумма"; }

            if (!DistributionBalanceWE.Displayed) { return "Не отображены элементы интерфейса: Баланс"; }
            if (!AbonentsListWE.Displayed) { return "Не отображены элементы интерфейса: Список абонентов"; }
            if (!EvenlySumInputWE.Displayed) { return "Не отображены элементы интерфейса: Поле ввода суммы распределения"; }
            if (!ContractInfo.Displayed) { return "Не отображены элементы интерфейса: Информация о договоре"; }
            if (!BackMCP.Displayed) { return "Не отображены элементы интерфейса: ссылка Назад к выбору абонентов"; }

            return "success";
        }


        #endregion

        public string GoEvenlySum()
        {
            try
            {
                new WebElement().ByXPath("//a[@id='distributionBalance:evenly']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoDifferentSum()
        {
            try
            {
                new WebElement().ByXPath("//a[@id='distributionBalance:bills-mobile-all']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoOneSumToAll()
        {
            try
            {
                new WebElement().ByXPath("//a[@id='distributionBalance:bills-mobile-new']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SetSumDistribution(string sum)
        {
            try
            {
                EvenlySumInputWE = new WebElement().ByXPath("//input[contains(@id,'distributionBalance:evenlySumInput')]");// and contains(@id,'umInput')]"); 
                if (!EvenlySumInputWE.Displayed) { return "Не отображены элементы интерфейса: Поле ввода суммы распределения"; }
                EvenlySumInputWE.SendKeys(sum);
                SendKeys.SendWait("{TAB}");
                FinishSumPanelWE = new WebElement().ByXPath("//div[contains(@id,'finishSumPanel')]");
                if (!FinishSumPanelWE.Displayed) { return "Не отображены элементы интерфейса: Итоговая сумма"; }
                if (Convert.ToDouble(sum) - Convert.ToDouble(FinishSumPanelWE.ByXPath("//div[@class='sum']").Text.Replace(" руб.","").Replace(" ","")) > 1)
                {
                    return "Итоговая сумма некорректна";
                }
                int i=0;
                WebElement tb = new WebElement().ByXPath("//tbody[@id='distributionBalance:abonents_data']//tr[@data-ri='0']");
                while (tb.Displayed)
                {
                    i++;
                    tb = new WebElement().ByXPath("//tbody[@id='distributionBalance:abonents_data']//tr[@data-ri='" + i + "']");
                }
                double cost= Convert.ToDouble(sum.Replace(" ",""))/i;
                i = 0;
                tb = new WebElement().ByXPath("//table[@id='distributionBalance:abonents_data']//tr[@data-ri='0']");
                while (tb.Displayed)
                {
                    string s = tb.ByXPath("//td[@class='ui-editable-column']").Text.Replace(" руб.", "").Replace(" ", "");
                    if (cost-Convert.ToDouble(s) > 1)
                    {
                        return "Распределение по абонентам некорректно";
                    }
                    i++;
                    tb = new WebElement().ByXPath("//table[@id='distributionBalance:abonents_data']//tr[@data-ri='" + i + "']");
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SetMaxSumDistribution()
        {
            try
            {
                string sum = "15000";
                EvenlySumInputWE = new WebElement().ByXPath("//input[contains(@id,'distributionBalance') and contains(@id,'umInput')]");
                if (!EvenlySumInputWE.Displayed) { return "Не отображены элементы интерфейса: Поле ввода суммы распределения"; }
                EvenlySumInputWE.SendKeys(sum);
                SendKeys.SendWait("{TAB}");
                FinishSumPanelWE = new WebElement().ByXPath("//div[contains(@id,'finishSumPanel')]");
                if (!FinishSumPanelWE.Displayed) { return "Не отображены элементы интерфейса: Итоговая сумма"; }
                if (Convert.ToDouble(sum) - Convert.ToDouble(FinishSumPanelWE.ByXPath("//div[@class='sum']").Text.Replace(" руб.", "").Replace(" ", "")) > 1)
                {
                    return "Итоговая сумма некорректна";
                }
                int i = 0;
                WebElement tb = new WebElement().ByXPath("//tbody[@id='distributionBalance:abonents_data']//tr[@data-ri='0']");
                while (tb.Displayed)
                {
                    i++;
                    tb = new WebElement().ByXPath("//tbody[@id='distributionBalance:abonents_data']//tr[@data-ri='" + i + "']");
                }
                double cost = Convert.ToDouble(sum.Replace(" ", "")) / i;
                i = 0;
                tb = new WebElement().ByXPath("//table[@id='distributionBalance:abonents_data']//tr[@data-ri='0']");
                while (tb.Displayed)
                {
                    string s = tb.ByXPath("//td[@class='ui-editable-column']").Text.Replace(" руб.", "").Replace(" ", "");
                    if (cost - Convert.ToDouble(s) > 1)
                    {
                        return "Распределение по абонентам некорректно";
                    }
                    i++;
                    tb = new WebElement().ByXPath("//table[@id='distributionBalance:abonents_data']//tr[@data-ri='" + i + "']");
                }
                                     
                if (!new WebElement().ByXPath("//*[contains(text(),'Недостаточно средств для распределения каждому абоненту введённой суммы.')]").Displayed)
                {
                    return "Валидация на превышение баланса некорректна";
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string SetSumDistribution(List<string> sums)
        {
            try
            {
                double d = 0;
                int i = 1;
                foreach (string sum in sums)
                {
                    new WebElement().ByXPath("//div[contains(@class,'ui-cell-editor-output')][" + i + "]").Click();
                    new WebElement().ByXPath("//div[contains(@class,'ui-cell-editor-input')][" + i + "]//input").SendKeys(sum);
                    i = i +1;
                    d = d + Convert.ToDouble(sum);
                }
                if (d - Convert.ToDouble(FinishSumPanelWE.ByXPath("//div[@class='sum']").Text.Replace(" руб.", "").Replace(" ", "")) > 1)
                {
                    return "Итоговая сумма некорректна";
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public string RepeatDistribution()
        {
            RepeatDistributionWE = new WebElement().ByXPath("//div[contains(@id,'repeatCheckBox')]//span");
            //if (!RepeatDistributionWE.Displayed) { return "Не отображены элементы интерфейса: чекбокс Повторять распределение"; }
            RepeatDistributionWE.Click();
            Thread.Sleep(5000);
            if (!new WebElement().ByXPath("//input[contains(@id,'distributionBalance:periodic:name')]").Displayed)
            {
                return "Не отображены элементы интерфейса: имя";
            }
            new WebElement().ByXPath("//input[contains(@id,'distributionBalance:periodic:name')]").SendKeys("Тестовый повтор");
            new WebElement().ByXPath("//input[@id='distributionBalance:periodic:endPeriod_input']").Click();
            Thread.Sleep(5000);
            new WebElement().ByXPath("//div[contains(@id,'ui-datepicker-div')]//table//a[text() = '" + DateTime.Now.Day + "']").Click();
            
            return "success";
        }

        public string onClickDistribution()
        {
            try
            {
                new WebElement().ByXPath("//a/span[contains(text(),'Распределить баланс')]").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string onClickSaveTemplate()
        {
            try
            {
                 SaveTemplateWE = new WebElement().ByXPath("//a/span[contains(text(),'Сохранить шаблон распределения')]");
                 if (!SaveTemplateWE.Displayed) { return "Не отображены элементы интерфейса: список Сохранить шаблон"; }
                 SaveTemplateWE.Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string onClickCancel()
        {
            try
            {
                CancelWE = new WebElement().ByXPath("//a/span[contains(text(),'Отменить распределение')]");
                if (!CancelWE.Displayed) { return "Не отображены элементы интерфейса: ссылка Отменить распределение"; }
                CancelWE.Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
        public string Submit()
        {
            try
            {
                Thread.Sleep(5000);
                if (new WebElement().ByXPath("//input[@id='notificationChange:requestUserServiceParamsForm:email']").Displayed) 
                    new WebElement().ByXPath("//input[@id='notificationChange:requestUserServiceParamsForm:email']").SendKeys("avyalov@bellintegrator.ru");
                else { return "Не отображены элементы интерфейса: Поле ввода емейла"; }
                if (new WebElement().ByXPath("//input[@id='notificationChange:requestUserServiceParamsForm:smsInput']").Displayed)
                    new WebElement().ByXPath("//input[@id='notificationChange:requestUserServiceParamsForm:smsInput']").SendKeys("9272882753");
                else { return "Не отображены элементы интерфейса: Поле ввода номера"; }
                if (new WebElement().ByXPath("//button[@id='notificationChange:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog']").Displayed)
                    new WebElement().ByXPath("//button[@id='notificationChange:requestUserServiceParamsForm:sendRequestButtonNotificationComponentDialog']").Click();
                else { return "Не отображены элементы интерфейса: Кнопка подтверждения"; }
                
                Thread.Sleep(10000);
                new WebElement().ByXPath("//span[text()='Да']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string NotSubmit()
        {
            try
            {
                new WebElement().ByXPath("//button[@id='notificationChange:requestUserServiceParamsForm:cancelSendRequestButtonNotificationComponentDialog']").Click();

                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string ValidMassage()
        {
            WebElement error = new WebElement().ByXPath("//div[@id='distributionBalance:sumInputMsg']//span[@class='ui-message-error-detail']");
            return error.Text;
        }

    }
}

