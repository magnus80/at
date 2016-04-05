using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class comments__pl : PageBase
    {

        #region element_value


        public bool PassportTabExists
        {
            get { return new WebElement().ByXPath("//a[contains(text(),'Расширенные функции заявки')]").IsNull(); }
        }

        public bool ReactAbonBtnExists
        {
            get { return new WebElement().ByXPath("//input[@name='reactivate_abon']").Displayed; }
        }
        
        public void FillPassport()
        {
            new WebElement().ByXPath("//td[2]/select").SendKeys("Москва");
            new WebElement().ByXPath("//div[4]/table/tbody/tr[4]/td[2]/input").SendKeys("111");
            new WebElement().ByXPath("//div[4]/table/tbody/tr[5]/td[2]/input").SendKeys("111");
            new WebElement().ByXPath("//tr[6]/td[2]/input").SendKeys("111");
            new WebElement().ByXPath("//div[3]/input").SendKeys("11");
            new WebElement().ByXPath("//div[3]/input[2]").SendKeys("11");
            new WebElement().ByXPath("//div[3]/input[3]").SendKeys("2000");
            new WebElement().ByXPath("//tr[10]/td/input").Click();
        }



        /// <summary>
        /// Статус заявки
        /// </summary>
        public string TStatus
        {
            get { return new WebElement().ByXPath("//select[@id='status']").Text; }
            set { new WebElement().ByXPath("//select[@id='status']").SendKeys(value); }
        }

        /// <summary>
        /// Комментарий к заявке
        /// </summary>
        public string TComment
        {
            get { return new WebElement().ByXPath("//textarea[@id='comment_text']").Text; }
            set { new WebElement().ByXPath("//textarea[@id='comment_text']").SendKeys(value); }
        }

        /// <summary>
        /// Поле "Сумма" в расширенных функциях заявки
        /// </summary>
        public string BSumm
        {
            set { new WebElement().ByXPath("//input[@name='b_summ']").SendKeys(value); }
        }

        #endregion

        #region actions



        /// <summary>
        /// Нажатие на кнопку "добровольная блокировка"
        /// </summary>
        public void BlockButtonClick()
        {
            new WebElement().ByXPath("//input[@name='block']").Click();
        }

        /// <summary>
        /// нажатие на кнопку "сменить" напротив адреса
        /// </summary>
        public void ChangeAddress()
        {
            new WebElement().ByXPath("//input[@name='adress_update']").Click();
        }

        /// <summary>
        /// Привязка тикета к ГП
        /// </summary>
        /// <param name="gp">ID глобалки</param>
        public void LinkToGp(string gp_id)
        {
            new WebElementSelect().ByXPath("//select[@name='global_id']").SelectByValue(gp_id);
            new WebElement().ByXPath("//input[@id='submit_ticket_form']").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "записать" во владке с паспортными данными
        /// </summary>
        public void SubmitPasportData()
        {
            new WebElement().ByXPath("//tr[10]/td/input").Click();
        }

        /// <summary>
        /// Развернуть вкладку с пасспортными данными
        /// </summary>
        public void OpenPassportDataTab()
        {
            new WebElement().ByXPath("//td[2]/div").Click();
        }


        /// <summary>
        /// Нажатие на кнопку "изменить заявку"
        /// </summary>
        public void SubmitTicket()
        {
            new WebElement().ByXPath("//input[@id='submit_ticket_form']").Click();
        }

        /// <summary>
        /// Закрывает всплывающее окно, сообщающее, что клиент VIP
        /// </summary>
        public void CloseVipPopup()
        {
            /*
            if (Browser.FindElement(OpenQA.Selenium.By.XPath("//div[6]/left/span")) != null)
            {
                new WebElement().ByXPath("//div[6]/left/span").Click();
            } */
        }

        /// <summary>
        /// Переход к вкладке "расширенные функции заявки"
        /// </summary>
        public void OpenExtendedTab()
        {
            new WebElement().ByXPath("//a[contains(text(),'Расширенные функции заявки')]").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "внести доверительный платеж"
        /// </summary>
        public void PromissedPay()
        {
            new WebElement().ByXPath("//input[@name='promissed_pay']").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "расторжение контракта
        /// </summary>
        public void CancelContract()
        {
            new WebElement().ByXPath("//center/input").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "продлить биллинг цикл"
        /// </summary>
        public void IncBillPer()
        {
            new WebElement().ByXPath("//div[2]/form/input[6]").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "начислить" в расширенных функциях заявки
        /// </summary>
        public void ChangeBonus()
        {
            new WebElement().ByXPath("//input[@name='bonus']").Click();
        }

        /// <summary>
        /// установка канала оповещения (email) - основной
        /// </summary>
        public void SetNotifEmail()
        {
            new WebElement().ByXPath("(//input[@name='answer'])[3]").Click();
            new WebElement().ByXPath("//input[@name='n_data']").Click();
            Browser.AssertDialog();
        }

        /// <summary>
        /// установка канала оповещения (sms) - основной
        /// </summary>
        public void SetNotifSMS()
        {
            new WebElement().ByXPath("(//input[@name='answer'])[2]").Click();
            new WebElement().ByXPath("//input[@name='answer_sms']").Click();
            new WebElement().ByXPath("//input[@name='n_data']").Click();
            Browser.AssertDialog();
        }


        #endregion

    }
}
