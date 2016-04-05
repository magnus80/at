//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Пользователи"), Description("Добавление нового")]
    public class test_246868 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Check("1");
        }

        [Test]
        public void step_03()
        {
            Check("2");
        }

        [Test]
        public void step_04()
        {
            Check("3");
        }

        [Test]
        public void step_05()
        {
            Check("4");
        }

        [Test]
        public void step_06()
        {
            Check("5");
        }

        [Test]
        public void step_07()
        {
            Check("6");
        }

        [Test]
        public void step_08()
        {
            Check("7");
        }

        private void Check(string status)
        {
            var group = Helpers.Query.GetFieldsOfTable("g_id", "helpdesk.groups", "g_partner_group is null")[0];

            Pages.HD.Users.Open("?addlogin=1");
            var login = Pages.HD.Users.AddLogin(status, group);

            var list = Helpers.Query.GetFieldsOfTable("u_status", "helpdesk.users", "u_login = '" + login + "'");

            Assertion(
                "Некорректный статус нового пользователя, [логин: " + login + "], ожидаемо: " +
                status + ", факт: " + list[0],
                () => Assert.AreEqual(status, list[0]));
        }
    }
}
