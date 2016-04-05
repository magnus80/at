using System;
using AT.Global;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AT.WebDriver
{
    public partial class WebElementBase
    {
        protected IWebElement  Element;

        public WebElementBase()
        {

        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="webElement"></param>
        public WebElementBase(IWebElement webElement)
        {
            Element = webElement;
        }

        #region actions

        public void Click()
        {
            try
            {
                Element.Click();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        public void LoadFile(string keys)
        {
            try
            {
                Element.SendKeys(keys);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        public void SendKeys(string keys)
        {
            try
            {
               
                Element.Click();
                Element.Clear();
                Element.SendKeys(keys);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        /// <summary>
        /// Возращает значение указанного атрибута
        /// </summary>
        /// <param name="att">название атрибута</param>
        /// <returns>значение атрибута</returns>
        public string GetAttribute(string att)
        {
            try
            {
                return Element.GetAttribute(att);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public void Clear()
        {
            if (Equals("input", Element.TagName) || Equals("textarea", Element.TagName))
            {
                Element.Clear();
                Element.Click();
            }
        }

        public void SelectByVisibleText(string text)
        {
            var selectElement = new SelectElement(Element);
            selectElement.SelectByText(text);
        }

        #endregion
        
    }
}
