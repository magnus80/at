using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class antivir__pl : PageBase
    {
        #region elements

        
        #endregion



        #region actions

        public void DeleteAvClick(string spl_id)
        {
          /*  WebElement form = new WebElement().ByName(spl_id);
            form.FindElement(By.Name("delete_av")).Click();*/
        }

        public string getAbonPrice(string spl_id)
        {
           /* WebElement form = new WebElement().ByName(spl_id);
            form.FindElement(By.Name("delete_av")).Click();
            */
            return "";
        }

        public bool CheckTable()
        {
            bool s_id = false;
            bool s_name = false;
            bool abon = false;
            bool date = false;


           /* foreach (var el in Browser.FindElements(By.TagName("td")))
            {
                if (el.Text.Equals("Сервис")) s_id = true;
                if (el.Text.Equals("Название")) s_name = true;
                if (el.Text.Equals("Абонентская плата")) abon = true;
                if (el.Text.Equals("Дата подключения")) date = true;
            }*/

            return s_id && s_name && abon && date;
        }

        #endregion
    }
}
