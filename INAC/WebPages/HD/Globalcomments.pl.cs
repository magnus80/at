using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class globalcomments__pl : PageBase
    {
        
        #region elemen_values





        /// <summary>
        /// Добавление комментария
        /// </summary>
        public string Comment
        {
            set
            {
                new WebElement().ByXPath("//textarea[@name='text']").SendKeys(value);
                new WebElement().ByXPath("//input[@name='write']").Click();
            }
        }

        /// <summary>
        /// устанавливает/снимает флаг полного отсутствия сервиса
        /// </summary>
        public bool ServiceFailure
        {
            get { return new WebElement().ByXPath("//input[@id='service_failure']").GetAttribute("checked").Equals("true"); }
            set
            {
                if(new WebElement().ByXPath("//input[@id='service_failure']").GetAttribute("checked") != value.ToString())
                {
                    new WebElement().ByXPath("//input[@id='service_failure']").Click();
                    new WebElement().ByXPath("//input[@name='changetype']").Click();
                }
            }
        }

        public string ResolveDate
        {
            get
            {
                return new WebElement().ById("resolve_date").GetAttribute("value");
            }
        }

        public bool NotifTimerEnabled
        {
            get { return new WebElement().ByXPath("//input[@name='notif_timer']").Enabled; }
        }

        public bool NotifStartEnabled
        {
            get { return new WebElement().ByXPath("//input[@name='notif_start']").Enabled; }
        }

        #endregion

        #region actions


        /// <summary>
        /// Устанавливает тип ГП
        /// </summary>
        /// <param name="type"></param>
        public void SetGpType(string type)
        {
            new WebElementSelect().ByXPath("//select[@id='gp_type']").SelectByText(type);
            new WebElement().ByXPath("//input[@name='changetype']").Click();
        }

        public void SetGpController(string name)
        {
            new WebElementSelect().ByXPath("//select[@name='ticket_controller']").SelectByValue(name);
            new WebElement().ByXPath("//input[@name='change_ticket_controller']").Click();
        }

        /// <summary>
        /// Выбирает из списка оператора контроля ГП, нажимает кнопку "сменить"
        /// </summary>
        /// <param name="name">имя оператора (u_name из helpdesk.users)</param>
        public void SetGpOperator(string login)
        {
       //      (2000);
            new WebElementSelect().ByXPath("//select[@id='operator']").SelectByValue(login);
         //    (2000);
            new WebElement().ByXPath("//input[@name='changeoperator']").Click();
        }

        public void SetGpOperGroup(string group)
        {
         //    (2000);
            new WebElementSelect().ByXPath("//select[@id='group']").SelectByText(group);
         //    (2000);
            new WebElement().ByXPath("//input[@name='changeoperator']").Click();
        }

        /// <summary>
        /// Устанавливает статус ГП
        /// </summary>
        /// <param name="status">Статус, в который будет переведена ГП {Открыта, Закрыта}</param>
        public void SetGpStatus(string status)
        {
            new WebElementSelect().ByXPath("//select[@id='gp_status']").SelectByText(status);  
            new WebElement().ByXPath("//input[@name='changestatus']").Click();
        }


        public void SetResolveDate(string year)
        {
            new WebElement().ByXPath("//div[@id='content']/table[2]/tbody/tr/td/form/table/tbody/tr[13]/td[2]/img").Click();
            new WebElementSelect().ByXPath("//div[@id='ui-datepicker-div']/div/div/select").SelectByText(year);

            new WebElement().ByXPath("(//button[@type='button'])[2]").Click();

            Browser.AssertDialog();
            new WebElement().ByXPath("//input[@name='write']").Click();
        }

        /// <summary>
        /// нажатие на кнопку "сменить" масштаб проблемы
        /// </summary>
        public void ScaleChangeClick()
        {
            new WebElement().ByXPath("//input[@name='scale_change']").Click();
        }

        #endregion
    }
}
