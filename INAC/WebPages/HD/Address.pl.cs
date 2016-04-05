using System;
using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class address__pl : PageBase
    {
        #region elements
        
        #endregion

        #region element_value

        /// <summary>
        /// Очередь заявок
        /// </summary>
        public string TQueue
        {
            set { new WebElementSelect().ByXPath("//select[@id='select_t']").SelectByText(value); }
        }

        /// <summary>
        /// Очередь заявок
        /// </summary>
        public string TType
        {
            set { new WebElementSelect().ByXPath("//select[@id='list_tickets']").SelectByText(value); }
        }

        /// <summary>
        /// Глобальная проблема
        /// </summary>
        public string TGlobalId
        {
            get { return new WebElement().ByXPath("//select[@id='global_id']").Text; }
            set { new WebElement().ByXPath("//select[@id='global_id']").SendKeys(value); }
        }
        

        #endregion
        
        #region actions

        /// <summary>
        /// Создать заявку
        /// </summary>
        /// <param name="queue">очередь</param>
        /// <param name="type">название</param>
        public void CreateTicket(string queue, string type)
        {
            TQueue = queue;
            TType = type;
            NewTicket();
        }

        /******* АКТИВАЦИЯ IPTV ********/

        public void CreateActivationIptv()
        {
            TQueue = "IPTV заявки";
            TType = "Активация IPTV";
            new WebElement().ByXPath("//input[@id='call_fromuser']").Click();
            new WebElementSelect().ByXPath("//select[@id='sale_code_hlp']").SelectByText("телесэйл");
            new WebElement().ByXPath("//input[@id='worker_name']").SendKeys("autotest_" + DateTime.Now.ToShortDateString());
            new WebElementSelect().ByXPath("//select[@id='chanals']").SelectByText("из другого источника");
            NewTicket();
        }

        /*******************************/

        /// <summary>
        /// Отправка SMS
        /// </summary>
        public void SendSms(string text)
        {
            new WebElement().ByXPath("//input[@id='send_smsx']").Click();
            Browser.SwitchToLastFrame();
            new WebElement().ByXPath("//textarea[@id='textsms']").SendKeys(text);
            new WebElement().ByXPath("//input[@name='send']").Click();
      //      new WebElement().ByCssSelector("span.ui-icon.ui-icon-closethick").Click();
//            Browser.CloseLastFrame();
        }

        /// <summary>
        /// Клик по ссылке "История IPTV"
        /// </summary>
        public void HistotyIptvClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'history_iptv')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Комментарии на контракте: Смотреть"
        /// </summary>
        public void ContractCommensClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'contract_comments')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Бонусный счет 	Просмотр/Управление"
        /// </summary>
        public void BonusContractClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'bonuses_contract')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Комментарии на логине:	Смотреть"
        /// </summary>
        public void LoginContractClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'logincomments')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Заходы: 	Смотреть"
        /// </summary>
        public void ShowCallsClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'showcalls')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Списания абонентской платы: 	Смотреть"
        /// </summary>
        public void ShowCommentsClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'showcomments')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Оплаты клиента: 	Смотреть"
        /// </summary>
        public void PaymentsClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'payments')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Резервирование средств: 	Смотреть"
        /// </summary>
        public void PayReservedClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'pay_reserved')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Доверительные платежи: 	Смотреть"
        /// </summary>
        public void PromissedPaymentsClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'payments')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Учетные периоды: 	Смотреть"
        /// </summary>
        public void StartStopClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'startstop')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "Оборудование: 	Смотреть"
        /// </summary>
        public void EquipmentsClick()
        {
             new WebElement().ByXPath(" //a[contains(@href, 'ptn/equipments/for_login')]").Click();
        }

        /// <summary>
        /// Клик по ссылке "История смены портов: 	Смотреть"
        /// </summary>
        public void PortHistoryClick()
        {
            new WebElement().ByXPath(" //a[contains(@href, 'portshistory')]").Click();
        }


        /// <summary>
        /// Нажатие на кнопку "Перейти к переносу баланса"
        /// </summary>
        public void MoveBill()
        {
            int i = 0;
            while (!new WebElement().ByXPath("//input[@value='Перейти к переносу баланса']").Displayed || ++i > 50)
            {
                Pages.HD.Address.Open("?address_id=" + Browser.Url.Substring(Browser.Url.LastIndexOf('=') + 1));

            }
            new WebElement().ByXPath("//input[@value='Перейти к переносу баланса']").Click();
        }

        /// <summary>
        /// Нажатие на кнопку "остановить биллинг цикл"
        /// </summary>
        public void StopBc()
        {
            new WebElement().ByXPath("//input[@name='stop_bc']").Click();
        }

        /// <summary>
        /// нажатие на кнопку "Административная блокировка"
        /// </summary>
        public void AdmBlockClick()
        {
            new WebElement().ByXPath("//input[@name='adm_block']").Click();
            Browser.CloseLastFrame();
        }

        /// <summary>
        /// нажатие на кнопку "разблокировать"
        /// </summary>
        public void Unblock()
        {
            new WebElement().ByXPath("//input[@name='unblock']").Click();
            //Browser.CloseLastFrame();
        }

        /// <summary>
        /// Баланс абонента, отображаемый на УКК
        /// </summary>
        public string Balance
        {
            get
            {
                var source = Browser.Source;

                source = source.Remove(source.IndexOf(" RUR"));
                source = source.Substring(source.LastIndexOf("<td>") + "<td>".Length);

                if (source.IndexOf('.') != -1)
                    source = source.Substring(0, source.IndexOf('.'));
                return source;
            }
        }

        /// <summary>
        /// Возвращает истину, если на УКК присутствует вкладка "обращения на адресе"
        /// </summary>
        /// <returns></returns>
        public bool InteractionTabExits()
        {
            return new WebElement().ByXPath("//div[@id='interaction_tickets_tabs']/ul/li[2]/a").IsNull() ? false : true;
        }

        /// <summary>
        /// Закрывает всплывающее окно, сообщающее, что клиент VIP
        /// </summary>
        public void CloseVipPopup()
        {/*
            try
            {
                if (Browser.FindElement(OpenQA.Selenium.By.XPath("//left/span")) != null)
                {
                    new WebElement().ByXPath("//left/span").Click();
                }
            }
            catch (Exception ex)
            {

            } */
        }

        /// <summary>
        /// Нажатие на кнопку блокировки (заблокировать / разблокировать)
        /// </summary>
        public void BlockUnblockClick()
        {
            new WebElement().ByXPath("//input[@name='unblock']").Click();
        }

        /// <summary>
        /// возвращает истину если на УКК есть кнопка блокировки (заблокировать / разблокировать)
        /// </summary>
        /// <returns></returns>
        public bool BlockUnblockExists()
        {
            return new WebElement().ByXPath("//input[@name='unblock']").Exits();
        }

        /// <summary>
        /// нажатие на кнопку "Завести заявку на этот контракт"
        /// </summary>
        public void NewTicket()
        {
            new WebElement().ByXPath("//input[@id='newticket']").Click();
        }

        /// <summary>
        /// нажатие на кнопку "Новое_обращение"
        /// </summary>
        public void NewInteraction()
        {
            new WebElement().ByXPath("//input[@id='interaction_button']").Click();
        }

        #endregion

    }
}
