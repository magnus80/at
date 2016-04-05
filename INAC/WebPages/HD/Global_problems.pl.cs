using System;
using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class global_problems__pl : PageBase
    {
        #region elements

       

        #endregion

        #region actions


        
        /// <summary>
        /// Устанавливает флаг статуса глобальной проблемы
        /// </summary>
        /// <param name="status">Статус ГП {Открыта, Закрыта, Все}</param>
        public void SetStatusFlag(string status)
        {
            switch (status)
            {
                case "Открыта": new WebElement().ByXPath("//tr[3]/td/form/input[3]").Click(); break;
                case "Закрыта": new WebElement().ByXPath("//tr[3]/td/form/input[4]").Click(); break;
                case "Все": new WebElement().ByXPath("//input[5]").Click(); break;
            }
        }

        /// <summary>
        /// Нажатие на кнопку "Показать"
        /// </summary>
        public void Show()
        {
            new WebElement().ByXPath("//p/table/tbody/tr[4]/td/input[2]").Click();
        }

        /// <summary>
        /// Выбор любой ГП из таблицы
        /// </summary>
        public void ClickAnyTicketInTable()
        {
            var scr = Browser.Source;
            var glTicketList = new List<string>();

            while (scr.IndexOf("globalcomments.pl?ticket_id=") > -1)
            {
                glTicketList.Add(scr.Substring(scr.IndexOf("globalcomments.pl?ticket_id="), "globalcomments.pl?ticket_id=00000000".Length));
                scr = scr.Remove(scr.IndexOf("globalcomments.pl?ticket_id="), 8);
            }

            var link = Browser.Url.Remove(Browser.Url.LastIndexOf('/') + 1) + glTicketList[new Random().Next(glTicketList.Count - 1)];


            Browser.Navigate(link);
        }


        #endregion
    }
}
