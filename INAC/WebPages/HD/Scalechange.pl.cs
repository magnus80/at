using System.Collections.Generic;
using AT;
using AT.WebDriver;


namespace INAC.WebPages.HD
{
    public class scalechange__pl: PageBase
    {
        #region elements

       

        #endregion

        #region Actions

        public void SetScaleAllNet()
        {
            new WebElement().ByXPath("//input[@name='area']").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();
            new WebElement().ByXPath("//input[@name='write_global']").Click();

        }

        /// <summary>
        /// выбирает масштаб "район", выбирает район, изменяет масштаб ГП
        /// </summary>
        public void SetScaleArea(string area)
        {
            new WebElement().ByXPath("(//input[@name='area'])[2]").Click(); 
            new WebElement().ByXPath("//input[@name='next']").Click();
            new WebElement().ByXPath("//input[@name='c_" + area + "_id']").Click();
            new WebElement().ByXPath("//input[@name='area_next']").Click();
            new WebElement().ByXPath("//input[@name='write_global']").Click();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="street">название улицы</param>
        /// <param name="street_id">id улицы из таблицы inac.streets0</param>
        /// <param name="house_id">id дома из таблицы inac.houses0</param>
        public bool SetScaleHouse(string street, string street_id, string house_id)
        {
            new WebElement().ByXPath("(//input[@name='area'])[3]").Click();
            new WebElement().ByXPath("//input[@name='next']").Click();

            new WebElement().ByXPath("//input[@name='text_str']").SendKeys(street);
            new WebElement().ByXPath("//input[@name='check']").Click();
            
            foreach (var  webElement in Browser.FindElementsByName("street_id"))
            {
                if (webElement.GetAttribute("value").Equals(street_id))
                {
                    webElement.Click();
                    foreach (var webElement1 in Browser.FindElementsByName("check"))
                    {
                       if(webElement1.GetAttribute("value").Equals("Далее"))
                       {
                           webElement1.Click();
                           break;
                       }
                    }
                    new WebElement().ByXPath("//input[@name='h_" + house_id + "_id']").Click();
                    new WebElement().ByXPath("//input[@name='new']").Click();
                    new WebElement().ByXPath("//input[@name='write_global']").Click();
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
