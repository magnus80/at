using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class tickets2__pl: PageBase
    {
        #region elements
        
        #endregion


        #region actions

        /// <summary>
        /// Отмечает радио кнопку со статусом проблемы - открыта
        /// </summary>
        public void Statusf_SetOpen()
        {
            new WebElement().ByXPath("//input[@name='statusf']").Click();
        }

        /// <summary>
        /// Устанавливает тип блокировки
        /// </summary>
        /// <param name="block">тип блокировки</param>
        public void Set_Block(string block)
        {
            new WebElement().ByXPath("//select[@name='blocked_clients']").SendKeys(block);
        }

        /// <summary>
        /// Устанавливает сортировку
        /// </summary>
        /// <param name="order">сортировка</param>
        public void Set_Order(string order)
        {
            new WebElement().ByXPath("//select[@name='sort_order']").SendKeys(order);
        }

        /// <summary>
        /// Нажатие на кнопку "показать"
        /// </summary>
        public void Refresh()
        {
            new WebElement().ByXPath("//input[@name='refresh']").Click();
        }

        #endregion
    }
}
