
using System;
using AT.Global;
using OpenQA.Selenium;


namespace AT.WebDriver
{
    public class WebElement : WebElementBase
    {
        public string Text
        {
            get
            {
                try
                {
                    return Element.Text;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return string.Empty;
                }
            }
        }

        public string Value
        {
            get
            {
                try
                {
                    return Element.GetAttribute("value");
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return string.Empty;
                }
            }
        }

        #region constructors

        public WebElement()
        {

        }

  
        public WebElement(IWebElement webElement)
        {
            Element = webElement;
        }

       
        #endregion

        #region States

        public bool IsNull()
        {
            return Element == null;
        }

        public bool Enabled
        {
            get
            {
                try
                {
                    return Element.Enabled;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return false;
                }
            }
        }

        public bool Displayed
        {
            get
            {
                try
                {
                    return Element.Displayed;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return false;
                }
            }
        }

        public string Id
        {
            get
            {
                try
                {
                    return Element.GetAttribute("id");
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return "";
                }
            }
        }

        public bool Selected
        {
            get
            {
                try
                {
                    return Element.Selected;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return false;
                }
            }
        }

        public bool Checked()
        {
            return Element.GetAttribute("value").Equals("off");
        }

        public bool Exits()
        {
            return Element != null;
        }

        #endregion


        
        #region Search_By

        public WebElement ById(string id)
        {
            try
            {
                Element = Browser.FindElement(By.Id(id));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public WebElement ByCssSelector(string cssSelector)
        {
            try
            {
                Element = Browser.FindElement(By.CssSelector(cssSelector));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public WebElement ByName(string name)
        {
            try
            {
                Element = Browser.FindElement(By.Name(name));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public WebElement ByXPath(string xpath)
        {
            try
            {
                Element = Browser.FindElement(By.XPath(xpath));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }

        }

        public WebElement ByTagName(string tagName)
        {
            try
            {
                Element = Browser.FindElement(By.TagName(tagName));
                return this;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public WebElement ByLink(string linkText, string href)
        {
            foreach (var elem in Browser.FindElements(By.TagName("a")))
            {
                if (elem.Text.Equals(linkText) && elem.GetAttribute("href").IndexOf(href) > -1)
                {
                    Element = elem;
                    return this;
                }

            }
            return null;
        }

        #endregion
    }
}
