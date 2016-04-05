using System;
using AT.Global;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AT.WebDriver
{
    public class WebElementSelect : WebElementBase
    {
        private SelectElement _element;

        public string GetSelectedValue()
        {
            try
            {
                return _element.SelectedOption.GetAttribute("value");
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public void SelectByValue(string text)
        {
            try
            {
                _element.SelectByValue(text);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        public void SelectByText(string text)
        {
            try
            {
                _element.SelectByText(text);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        public void SelectByIndex(int index)
        {
            try
            {
                _element.SelectByIndex(index);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        #region Search_By

        public WebElementSelect ById(string value)
        {
            try
            {
                _element = new SelectElement(Browser.FindElement(By.Id(value)));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }


        public WebElementSelect ByName(string value)
        {

            _element = new SelectElement(Browser.FindElement(By.Name(value)));
            return this;
        }

        public WebElementSelect ByXPath(string value)
        {
            _element = new SelectElement(Browser.FindElement(By.XPath(value)));
            return this;
        }

        public WebElementSelect ByTagName(string value)
        {
            _element = new SelectElement(Browser.FindElement(By.TagName(value)));
            return this;
        }


        #endregion
    }
}
