using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT;
using AT.WebDriver;

namespace USSS.WebPages.B2BPle
{
    class ContractPage: PageBase
    {
        public string balance;
        #region constructor

        // поле ввода логина
        private  WebElement BalanceWE;


        public string ConstructionPage()
        {
            BalanceWE = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget balance']");

            if (!BalanceWE.Displayed) { return "Не отображены элементы интерфейса: баланс"; }

            balance = BalanceWE.Text;
            return "success";
        }

        #endregion

    }
}
