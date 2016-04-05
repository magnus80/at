using System;
using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class notification_settings__pl:PageBase
    {
        #region element_values

        /// <summary>
        /// поле "Текст"
        /// </summary>
        public string NewText
        {
            set
            {
                new WebElement().ByXPath("//textarea[@id='new_text']").SendKeys(value);
            }
            get { return new WebElement().ByXPath("//textarea[@id='new_text']").Text; }
        }

        #endregion

        #region actions

        #region change 

        /// <summary>
        /// Создание нового sms оповещения о закрытии ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string ChangeNewSmsTemp()
        {
            return ChangeNotif("1");
        }

        /// <summary>
        /// Создание нового email оповещения о закрытии ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string ChangeNewEmailTemp()
        {
            return ChangeNotif("2");
        }

        /// <summary>
        /// Создание нового sms оповещения о продлении срока ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string ChangeNewSMSTimeTemp()
        {
            return ChangeNotif("3");
        }

        /// <summary>
        /// Создание нового оповещения для аварийных ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string ChangeGpCrashTemp()
        {
            return ChangeNotif("4");
        }

        private string ChangeNotif(string val)
        {
            new WebElementSelect().ByXPath("//select[@id='template_type']").SelectByValue(val);
            var text = DateTime.Now.Date + "  " + (new Random()).Next(100000, 999999);
            NewText = text;
            new WebElement().ByXPath("//input[@name='edit']").Click();

            Open();
            new WebElementSelect().ByXPath("//select[@id='template_type']").SelectByValue(val);
            new WebElement().ByXPath("//input[@name='change']").Click();

            return text;
        }

        #endregion

        #region new

        /// <summary>
        /// Создание нового sms оповещения о закрытии ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string NewSmsTemp()
        {
            return NewNotif("1");
        }

        /// <summary>
        /// Создание нового email оповещения о закрытии ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string NewEmailTemp()
        {
            return NewNotif("2");
        }

        /// <summary>
        /// Создание нового email оповещения о закрытии ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string NewSMSTimeTemp()
        {
            return NewNotif("3");
        }

        /// <summary>
        /// Создание нового оповещения для аварийных ГП
        /// </summary>
        /// <returns>текст оповещения</returns>
        public string NewGpCrashTemp()
        {
            return NewNotif("4");
        }


        private string NewNotif(string val)
        {
            new WebElementSelect().ByXPath("//select[@id='template_type']").SelectByValue(val);
            var text = DateTime.Now.Date + "  " + (new Random()).Next(100000, 999999);
            NewText = text;
            new WebElement().ByXPath("//input[@name='save']").Click();

            Open();
            new WebElementSelect().ByXPath("//select[@id='template_type']").SelectByValue(val);
            new WebElement().ByXPath("//input[@name='change']").Click();

            return text;
        }

        #endregion

        #region del

        /// <summary>
        /// Удаление шаблона
        /// </summary>
        /// <param name="type">тип</param>
        /// <param name="text">текст</param>
        public string DeleteNotif(string type)
        {
            new WebElementSelect().ByXPath("//select[@id='template_type']").SelectByValue(type);
            new WebElementSelect().ByXPath("//select[@id='template_text_type_" + type + "']").SelectByIndex(1);
            var id = new WebElementSelect().ByXPath("//select[@id='template_text_type_" + type + "']").GetSelectedValue();
            new WebElement().ByXPath("//input[@name='del']").Click();

            return id;
        }

        #endregion

        #endregion
    }
}
