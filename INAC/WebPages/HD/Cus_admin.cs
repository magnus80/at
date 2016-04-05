using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class cus_admin__pl : PageBase
    {
        #region elements

        private WebElement CampaignNameEdit;
        private WebElement CalendarStartEdit;
        private WebElement CalendarStopEdit;
        private WebElement SubmitCusButton;
        // private WebElement CalendarStopNextYear;


        private WebElement AnswerTxtEdit;
        private WebElement AddAswerButton;
        private WebElement ExitsAnswerRbtn;
        private WebElement SubAnswerEdit;
        private WebElement AddSubAnswerButton;


        private WebElement TypeServiceRbtn;
        private WebElement SetAttrButton;


        /// <summary>
        /// Инициализирует элементы страницы
        /// </summary>
        public void LoadElements()
        {
            CampaignNameEdit = new WebElement().ByXPath("//span/table/tbody/tr/td[2]/input");
            CalendarStartEdit = new WebElement().ByXPath("//tr[2]/td[2]/input");
            CalendarStopEdit = new WebElement().ByXPath("//input[2]");
            SubmitCusButton = new WebElement().ByXPath("//tr[3]/td/input");

        }

        /// <summary>
        /// Инициализирует элементы страницы, те, которые недоступны вначале и появляются посредством выболнения javascript кода
        /// </summary>
        public void LoadElementsParam(string param)
        {
            switch (param)
            {
                case "cus_submit_first":
                    {
                        AnswerTxtEdit = new WebElement().ByXPath("//tr[4]/td[2]/input");
                        AddAswerButton = new WebElement().ByXPath("//tr[4]/td[2]/input[2]");
                        SubmitCusButton = new WebElement().ByXPath("//tr[5]/td/input");
                        break;
                    }
                case "cus_submit_second":
                    {
                        ExitsAnswerRbtn = new WebElement().ByXPath("//td[2]/table/tbody/tr[2]/td/input");
                        SubAnswerEdit = new WebElement().ByXPath("//tr[6]/td[2]/input");
                        AddSubAnswerButton = new WebElement().ByXPath("//tr[6]/td[2]/input[2]");
                        SubmitCusButton = new WebElement().ByXPath("//tr[7]/td/input");
                        break;
                    }
                case "cus_submit_third":
                    {
                        TypeServiceRbtn = new WebElement().ByXPath("//td[3]/b/input");
                        SetAttrButton = new WebElement().ByXPath("//tr[18]/td/input");
                        break;
                    }
            }
        }


        #endregion



        #region element_values

        /// <summary>
        /// Подвопрос
        /// </summary>
        public string SubAnswer
        {
            get { return SubAnswerEdit.Text; }
            set { SubAnswerEdit.SendKeys(value); }
        }

        /// <summary>
        /// Поле "вопрос"
        /// </summary>
        public string AnswerTxt
        {
            get { return AnswerTxtEdit.Text; }
            set { AnswerTxtEdit.SendKeys(value); }
        }

        /// <summary>
        /// поле "Название кампании"
        /// </summary>
        public string CampaignName
        {
            get { return CampaignNameEdit.Text; }
            set { CampaignNameEdit.SendKeys(value); }
        }

        #endregion

        #region actions

        /// <summary>
        /// Выбор типа сервиса "ШПД"
        /// </summary>
        public void SelectTypeService()
        {
            TypeServiceRbtn.Click();
        }

        /// <summary>
        /// Нажатие на кнопку "установить атрибуты"
        /// </summary>
        public void SetAttributes()
        {
            SetAttrButton.Click();
        }

        /// <summary>
        /// Выбор добавленного вопроса
        /// </summary>
        public void SelectExistAnswer()
        {
            ExitsAnswerRbtn.Click();
        }

        /// <summary>
        /// Нажатие на кнопку "Добавить вариант ответа"
        /// </summary>
        public void AddAnswer()
        {
            AddAswerButton.Click();
        }

        /// <summary>
        ///  Нажатие на кнопку "Добавить ПОДвариант ответа"
        /// </summary>
        public void AddSubAnswer()
        {
            AddSubAnswerButton.Click();
        }

        /// <summary>
        /// Нажатие на кнопку "сохранить кампанию"
        /// </summary>
        public void SubmitCus()
        {
            SubmitCusButton.Click();
        }

        /// <summary>
        /// Устанавливает в календарях дату старта (первый день текущего месяца), дату завершения (первый день след. месяца)
        /// </summary>
        public void SetCalendarValue()
        {
           /* CalendarStartEdit.Click();
            CalendarStartEdit.Click();
            foreach (var td in CalendarStartEdit.FindElements(OpenQA.Selenium.By.TagName("td")))
            {
                if (td.Text.Equals("1"))
                {
                    td.Click();
                    break;
                }
            }

            CalendarStopEdit.Click();
            CalendarStopEdit.Click();

            CalendarStopEdit.FindElement(OpenQA.Selenium.By.XPath("//div[6]/table/thead/tr[2]/td[4]/div")).Click();

            foreach (var td in CalendarStopEdit.FindElements(OpenQA.Selenium.By.TagName("td")))
            {
                if (td.Text.Equals("1"))
                {
                    td.Click();
                    break;
                }
            }*/
        }

        #endregion
    }
}
