using System;
using System.Collections.Generic;
using AT.Global;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AT.WebDriver
{
    public partial class WebElementTable : WebElementBase
    {
        private string xpath;

        /// <summary>
        /// Значение ячейки по индексу (текст)
        /// </summary>
        /// <param name="i">i (строка)</param>
        /// <param name="j">j (столбец)</param>
        /// <returns></returns>
        public string this[int i, int j]
        {
            get
            {
                try
                {
                    return Browser.FindElement(By.XPath(xpath + "/tbody/tr[" + i + "]/td[" + j + "]")).Text;
                    //  return Element.FindElement(By.XPath("/tbody/tr[" + i + "]/td[" + j + "]")).Text;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return string.Empty;
                }
            }
        }

        #region Init_By

        public void InitByXPath(string value)
        {
            try
            {
                xpath = value;
                Element = Browser.FindElement(By.XPath(value));
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        #endregion

        public List<List<string>> GetAllData(string path)
        {
            List<List<string>> table = new List<List<string>>();
            List<string> head = new List<string>();
            if (Browser.FindElement(By.XPath( path + "/thead/tr/th")).Displayed)
            {
                IWebElement webElementHead = Browser.FindElement(By.XPath(path + "/thead/tr"));
                IList<IWebElement> ElementCollectionHead = webElementHead.FindElements(By.XPath(path + "/thead/tr/th"));
                foreach (IWebElement item in ElementCollectionHead)
                {
                   head.Add(item.Text);
                }
            }
            List<string> body = new List<string>();
            if (Browser.FindElement(By.XPath(path + "//tbody//tr")).Displayed)
            {
                IWebElement webElementBody = Browser.FindElement(By.XPath( path+"/tbody/tr"));
                IList<IWebElement> ElementCollectionBody =
                    webElementBody.FindElements(By.XPath(path + "/tbody/tr"));
                foreach (IWebElement item in ElementCollectionBody)
                {
                        body.Add(item.Text);
                }
            }
            table.Add(head);
            table.Add(body);
            return table;
        }
    }

}
