using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class adv_adr_info__pl : PageBase
    {

        #region element_value

        /// <summary>
        /// поле "1-ый номер контактного телефона"
        /// </summary>
        public string Phone1
        {
            get { return new WebElement().ByXPath("//tr[7]/td[2]/input").Text; }
            set { new WebElement().ByXPath("//tr[7]/td[2]/input").SendKeys(value); }
        }

        /// <summary>
        /// поле "Номер для отправки SMS"
        /// </summary>
        public string PhoneSMS
        {
            get { return new WebElement().ByXPath("//b/input").Text; }
            set { new WebElement().ByXPath("//b/input").SendKeys(value); }
        }

        #endregion

#region actions

        /// <summary>
        /// Установка флага VIP клиент
        /// </summary>
        public void SetVipFlag()
        {
            if (!new WebElement().ByXPath("//input").Checked()) 
                new WebElement().ByXPath("//input").Click();
        }

        /// <summary>
        /// Снятие флага VIP клиент
        /// </summary>
        public void UnsetVipFlag()
        {
            if (new WebElement().ByXPath("//input").Checked()) new WebElement().ByXPath("//input").Click();
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public void Submit()
        {
            new WebElement().ByXPath("//input[3]").Click();
        }

#endregion

    }
}
