using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using AT.Global;
using AT.Service;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;


namespace AT.WebDriver
{
    public enum Browsers
    {
        [Description("Windows Internet Explorer")] InternetExplorer,

        [Description("Mozilla Firefox")] Firefox,

        [Description("Google Chrome")] Chrome
    }



    public static class Browser
    {
        public static Browsers SelectedBrowser
        {
            get { return SelectBrowser(); }
        }

        #region Private

        private static IWebDriver _webDriver;
        private static string _mainWindowHandler;
        private static IWebDriver WebDriver
        {
            get { return _webDriver ?? StartWebDriver(); }
        }

        private static Browsers SelectBrowser()
        {
            var selectedBrowser = Config.GetStringParam("browser") ?? string.Empty;
            switch (selectedBrowser)
            {
                case "chrome":
                    return Browsers.Chrome;
                case "firefox":
                    return Browsers.Firefox;
                case "iexplore":
                    return Browsers.InternetExplorer;
                default:
                    GlobalEvents.ErrorFounded("Ошибка: некорректный код браузера в конфиге, код : " +
                                              selectedBrowser + "; в этом случае IE");
                    return Browsers.InternetExplorer;
            }
        }

        #region Start Browser

        private static IWebDriver StartWebDriver()
        {
            if (_webDriver != null) return _webDriver;

            switch (SelectedBrowser)
            {
                case Browsers.InternetExplorer:
                    _webDriver = StartInternetExplorer();
                    break;
                case Browsers.Firefox:
                    _webDriver = StartFirefox();
                    break;
                case Browsers.Chrome:
                    _webDriver = StartChrome();
                    break;
                default:
                    Logger.Write(string.Format("Unknown browser selected: {0}.", SelectedBrowser));
                    return null;
            }

            var implicitlyWait = Config.GetIntParam("ImplicitlyWait");

            _webDriver.Manage().Window.Maximize();
            _webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(implicitlyWait));
            _mainWindowHandler = _webDriver.CurrentWindowHandle;

            return WebDriver;
        }

        private static InternetExplorerDriver StartInternetExplorer()
        {
            var internetExplorerOptions = new InternetExplorerOptions
                                              {
                                                  IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                                                  InitialBrowserUrl = "about:blank",
                                                  EnableNativeEvents = true
                                              };

            return new InternetExplorerDriver(internetExplorerOptions);
        }

        private static FirefoxDriver StartFirefox()
        {
            var firefoxProfile = new FirefoxProfile
                                     {
                                         AcceptUntrustedCertificates = true,
                                         EnableNativeEvents = true
                                     };

            return new FirefoxDriver(firefoxProfile);
        }

        private static ChromeDriver StartChrome()
        {
            var chromeOptions = new ChromeOptions();
            var defaultDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\Local\Google\Chrome\User Data\Default";
            String curr = @"C:\Tools";


            //return new ChromeDriver(Directory.GetCurrentDirectory());

            return new ChromeDriver(curr, chromeOptions);
        }
        
        #endregion
        
        private static void WaitForAjax()
        {
            int counter = 0;
            var waitForAjax = Config.GetIntParam("WaitForAjax");

            while (counter < waitForAjax)
            {
                try
                {
                    var javaScriptExecutor = _webDriver as IJavaScriptExecutor;
                    var ajaxIsComplete = javaScriptExecutor != null &&
                                         (bool)javaScriptExecutor.ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    counter++;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    break;
                }
            }
        }

        #endregion

        #region Public
        
        #region states 

        public static string Url
        {
            get
            {
                return _webDriver.Url;
            }
        }

        public static void ReinitBrowser(string url)
        {
            _webDriver = null;
            WebDriver.Navigate().GoToUrl(url);
        }

        public static string Source
        {
            get { return _webDriver.PageSource; }
        }

        public static int WindowsCount
        {
            get { return WebDriver.WindowHandles.Count; }
        }

        #endregion

        #region actions 
        
        public static void Navigate(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public static void MoveToElement(string xpath)
        {
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(15));

            var element = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));

            Actions action = new Actions(_webDriver);
            action.DoubleClick(element).Perform();
            action.MoveToElement(element).Perform();
        }

        public static void Quit()
        {
            if (_webDriver == null) return;

            _webDriver.Quit();
            _webDriver = null;
        }

        public static void Close()
        {
            if (_webDriver == null) return;

            _webDriver.Close();
        }

        public static IEnumerable<IWebElement> FindElements(By selector)
        {
            WaitForAjax();

            return WebDriver.FindElements(selector);
        }

        public static List<WebElement> FindElementsByName(string name)
        {
            WaitForAjax();

            return WebDriver.FindElements(By.Name(name)).Select(el => new WebElement(el)).ToList();
        }

        public static List<WebElement> FindElementsByXPath(string xpath)
        {
            WaitForAjax();
            ReadOnlyCollection<IWebElement> we = WebDriver.FindElements(By.XPath(xpath));
            return we.Select(el => new WebElement(el)).ToList();
        }

        public static List<WebElement> FindElementsByTagName(string tag_name)
        {
            WaitForAjax();

            return WebDriver.FindElements(By.TagName(tag_name)).Select(el => new WebElement(el)).ToList();
        }

        public static IWebElement FindElement(By selector)
        {
            try
            {
                WaitForAjax();
                return WebDriver.FindElement(selector);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public static void CloseLastFrame()
        {
            SwitchWindow(WindowsCount);
            Close();
            SwitchToMainWindow();
        }

        public static object ExecuteJavaScript(string javaScript, params object[] args)
        {
            try
            {
                var scriptExecutor = WebDriver as IJavaScriptExecutor;

                if (scriptExecutor != null)
                    return scriptExecutor.ExecuteScript(javaScript, args);

                return null;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        #region switch

        public static void SwitchToLastFrame()
        {
            _webDriver.SwitchTo().Frame(1);
        }

        public static void SwitchToFrame(IWebElement inlineFrame)
        {
            WebDriver.SwitchTo().Frame(inlineFrame);
        }

        public static void SwitchToPopupWindow()
        {
            foreach (var handle in WebDriver.WindowHandles.Where(handle => handle != _mainWindowHandler)) // TODO:
            {
                WebDriver.SwitchTo().Window(handle);
            }
        }

        public static void SwitchToMainWindow()
        {
            WebDriver.SwitchTo().Window(_mainWindowHandler);
        }

        public static void SwitchToDefaultContent()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public static void SwitchWindow(int numberOfWindow)
        {
            string handle = WebDriver.WindowHandles.ToArray()[numberOfWindow - 1];
            WebDriver.SwitchTo().Window(handle);
        }

        #endregion

        public static void AssertDialog()
        {
            try
            {
                _webDriver.SwitchTo().Alert().Accept();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        #endregion

        #endregion

    }
}
