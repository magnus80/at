using AT;
using AT.WebDriver;

namespace INAC.WebPages.OSE
{
    public class inacusers__pl : PageBase
    {
        public string Login
        {
            set { new WebElement().ByXPath("//input[@name='login']").SendKeys(value); }
        }

        public string Name
        {
            set { new WebElement().ByXPath("//input[@name='finger']").SendKeys(value); }
        }

        public string Email
        {
            set { new WebElement().ByXPath("//input[@name='email']").SendKeys(value); }
        }

        public string EmployeeNo
        {
            set { new WebElement().ByXPath("//input[@name='employee_no']").SendKeys(value); }
        }

        public void WriteBtnClick()
        {
            new WebElement().ByXPath("//input[@name='write']").Click();
        }

        public void NewUser(string login, string name, string email, string employee_no)
        {
            Open();
            Login = login;
            Name = name;
            Email = email;
            EmployeeNo = employee_no;
            WriteBtnClick();
        }
    }
}
