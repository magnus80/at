using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class avpc_statuses__pl : PageBase
    {
        private List<WebElement> listPolices; 

        #region elemen_values


        public string ConnectEmail
        {
            get { return new WebElement().ByXPath("//input[@name='connect_email']").Text; }
            set { new WebElement().ByXPath("//input[@name='connect_email']").SendKeys(value); }
        }

        #endregion


        #region actions

        /// <summary>
        /// подключение род. контроля
        /// </summary>
        public void ConnectPC()
        {
            new WebElement().ByXPath("(//input[@name='service'])[2]").Click();
            ConnectEmail = (ConnectEmail == "") ? "test@test.ru" : ConnectEmail;
            new WebElement().ByXPath("//input[@name='connect']").Click();
        }

        /// <summary>
        /// подключение антивируса (когда подключен род. контроль)
        /// </summary>
        public void ConnectAV()
        {
            new WebElement().ByXPath("//input[@name='connectAV']").Click();
        }

        /// <summary>
        /// удаление антивируса когда подключен род. кнотроль
        /// </summary>
        public void DeleteAV()
        {
            new WebElement().ByXPath("//input[@name='deleteAV']").Click();
        }

        /// <summary>
        /// удаление род.контроля
        /// </summary>
        public void DeleteAll()
        {
            new WebElement().ByXPath("//input[@name='deleteall']").Click();
        }

        /// <summary>
        /// загрузка категорий
        /// </summary>
        private void LoadDefaultCat()
        {
            listPolices = new List<WebElement>();

            listPolices.Add(new WebElement().ByXPath("//tr[14]/td/input"));
            listPolices.Add(new WebElement().ByXPath("//tr[14]/td/input[2]"));
            listPolices.Add(new WebElement().ByXPath("//input[3]"));
            listPolices.Add(new WebElement().ByXPath("//input[4]"));
            listPolices.Add(new WebElement().ByXPath("//input[5]"));

        }

        /// <summary>
        /// проверка выбранной категории
        /// </summary>
        /// <param name="val">ид категории</param>
        /// <returns></returns>
        public bool CheckSelectedCat(string val)
        {
            LoadDefaultCat();
            foreach (var pol in listPolices)
            {
                if (pol.Selected && pol.GetAttribute("value").Equals(val)) 
                    return true;
            }
            return false;
        }

        /// <summary>
        /// выбирает время ограничения (понедельник 06-07, среда 06-07)
        /// </summary>
        public void SelectDays()
        {
            new WebElement().ByXPath("//input[@name='days_mon6']").Click();
            new WebElement().ByXPath("//input[@name='days_wed6']").Click();
            new WebElement().ByXPath("//input[@name='ch_sched']").Click();

        }

        #endregion

    }
}
