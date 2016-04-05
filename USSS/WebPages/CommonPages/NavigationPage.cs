using AT;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.CommonPages
{
    internal class NavigationPage : PageBase
    {
        #region managerPage

        public string GoToTariff()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[@href='/c/tariffs/tariffsList.html']").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Тарифы'";
                }
                new WebElement().ByXPath("//a[@href='/c/tariffs/tariffsList.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToRequestHistory()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[@href='/c/operations/operationsHistory.html']").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'История заявок'";
                }
                new WebElement().ByXPath("//a[@href='/c/operations/operationsHistory.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoToServices()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[@href='/c/services/servicesList.html']").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Услуги'";
                }
                new WebElement().ByXPath("//a[@href='/c/services/servicesList.html']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoFinansAndDetalization()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[contains(@href,'fininfo/index.xhtml')]").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Финансы и детализация'";
                }
                new WebElement().ByXPath("//a[contains(@href,'fininfo/index.xhtml')]").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoManagerContracts()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[@id='navCatalog']").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Управление контрактами'";
                }
                new WebElement().ByXPath("//a[@id='navCatalog']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoReports()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[@id='guiReports:reports']").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Отчеты'";
                }
                new WebElement().ByXPath("//a[@id='guiReports:reports']").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GoProfile()
        {
            try
            {
                if (!new WebElement().ByXPath("//a[contains(@id,'Profile')]").Displayed)
                {
                    return "Не отображены элементы интерфейса: ссылка навигации 'Профиль'";
                }
                new WebElement().ByXPath("//a[contains(@id,'Profile')]").Click();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }
}
