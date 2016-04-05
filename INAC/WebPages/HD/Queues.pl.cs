using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class queues__pl : PageBase
    {
        #region element_values


        /// <summary>
        /// поле "Логин"
        /// </summary>
        public string Login
        {
            get { return new WebElement().ByXPath("//input[3]").Text; }
            set { new WebElement().ByXPath("//input[3]").SendKeys(value); }
        }

        /// <summary>
        /// поле "Контракт"
        /// </summary>
        public string Contract
        {
            get { return new WebElement().ByXPath("//input[2]").Text; }
            set { new WebElement().ByXPath("//input[2]").SendKeys(value); }
        }

        /// <summary>
        /// поле "Номер проблемы"
        /// </summary>
        public string Ticket
        {
            get { return new WebElement().ByXPath("//input").Text; }
            set { new WebElement().ByXPath("//input").SendKeys(value); }
        }


        #endregion

        #region actions

        /// <summary>
        /// Нажатие на кнопку "Искать"
        /// </summary>
        public void Search()
        {
            new WebElement().ByXPath("//input[@value='Искать']").Click();
        }

        public void OpenTicket(string ticket)
        {
            Open();
            Ticket = ticket;
            Search();
        }

        /// <summary>
        /// Открывает карточку заявки 
        /// </summary>
        /// <param name="contract">номер заявки (helpdesk.tickets поле t_number)</param>
        public void OpenContract(string contract)
        {
            Open();
            Contract = contract;
            Search();
        }

        public void OpenLogin(string login)
        {
            Open();
            Login = login;
            Search();
        }

        #endregion
    }
}
