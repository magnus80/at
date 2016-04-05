using System;
using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class users__pl : PageBase
    {
        /// <summary>
        /// Добавление пользователя HD
        /// </summary>
        /// <param name="status">ид статуса</param>
        public string AddLogin(string status, string group)
        {
            var rand = new Random().Next(10000, 99999).ToString();

            var login = "at_" + rand + "_l";
            var fio = "at_" + rand + "_f";
            var tabn = "at_" + rand + "_t";
            var email = "at_" + rand + "@beeline.ru";
            var phone = "9000000000";

            new WebElement().ByXPath("//input[@name='newlogin']").SendKeys(login);
            new WebElement().ByXPath("//input[@name='name']").SendKeys(fio);
            new WebElement().ByXPath("//input[@id='employee_no']").SendKeys(tabn);
            new WebElement().ByXPath("//input[@name='uemail']").SendKeys(email);
            new WebElement().ByXPath("//input[@name='uphone']").SendKeys(phone);

            new WebElementSelect().ByXPath("//select[@id='status']").SelectByValue(status);
            new WebElementSelect().ByXPath("//select[@id='groups_list']").SelectByValue(group);

            new WebElement().ByXPath("//input[@id='write']").Click();
            System.Threading.Thread.Sleep(2000);
            Open("?rand=1&login=" + login);
            new WebElement().ByXPath("//input[@name='write']").Click();

            return login;
        }

        /// <summary>
        /// Добавление пользователя HD
        /// </summary>
        /// <param name="status">статус</param>
        /// <param name="group">ид группы</param>
        /// <param name="emp_no">таб. номер сущ. абонента</param>
        /// <param name="ex_login">логин сущ. абонента (по таб. номеру)</param>
        /// <returns></returns>
        public bool AddLogin(string status, string group, string emp_no, string ex_login)
        {
            var rand = new Random().Next(10000, 99999).ToString();

            var login = "at_" + rand + "_l";
            var fio = "at_" + rand + "_f";
            var tabn = "at_" + rand + "_t";
            var email = "at_" + rand + "@beeline.ru";
            var phone = "9000000000";

            new WebElement().ByXPath("//input[@name='newlogin']").SendKeys(login);
            new WebElement().ByXPath("//input[@name='name']").SendKeys(fio);
            new WebElement().ByXPath("//input[@id='employee_no']").SendKeys(emp_no);
            new WebElement().ByXPath("//input[@name='uemail']").SendKeys(email);
            new WebElement().ByXPath("//input[@name='uphone']").SendKeys(phone);

            new WebElementSelect().ByXPath("//select[@id='status']").SelectByValue(status);
            new WebElementSelect().ByXPath("//select[@id='groups_list']").SelectByValue(group);

            new WebElement().ByXPath("//input[@id='write']").Click();

            return new WebElement().ByLink(ex_login, "users.pl") == null ? false : true;
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <param name="rand"></param>
        public void EditLogin(string rand)
        {
            var login = "at_" + rand + "_l";
            var fio = "at_" + rand + "_f";
            var email = "at_" + rand + "@beeline.ru";

            new WebElement().ByXPath("//input[@name='newlogin']").SendKeys(login);
            new WebElement().ByXPath("//input[@name='name']").SendKeys(fio);
            new WebElement().ByXPath("//input[@name='uemail']").SendKeys(email);

            new WebElement().ByXPath("//input[@name='write']").Click();
        }
    }
}
