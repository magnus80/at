//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Пользователи"), Description("Табельные номера")]
    public class test_247244 : TestBase
    {
        private string count_old, emp_no;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            count_old = Helpers.Query.GetFieldsOfTable("count(*)", "helpdesk.users", "")[0];
        }

        [Test]
        public void step_03()
        {
            var list = Helpers.Query.GetFieldsOfTable("u_name, u_employee_no", "helpdesk.users",
                                                      "u_employee_no is not null");
            var group = Helpers.Query.GetFieldsOfTable("g_id", "helpdesk.groups", "g_partner_group is null")[0];

            emp_no = list[1];

            Pages.HD.Users.Open("?addlogin=1");
            var res = Pages.HD.Users.AddLogin("1", group, list[1], list[0]);

            Assertion(
                "Система не выдала сообщения о неуникальности введенного таб. номера, [т.номер: " +
                list[1] + "]",
                () => Assert.IsTrue(res));
        }

        [Test]
        public void step_04()
        {
            var count_new = Helpers.Query.GetFieldsOfTable("count(*)", "helpdesk.users", "")[0];

            Assertion(
                "Добавился пользователь с неуникальным табельным номером, [таб. номер: " + "]",
                () => Assert.AreEqual(count_new, count_old));
        }

    }
}
