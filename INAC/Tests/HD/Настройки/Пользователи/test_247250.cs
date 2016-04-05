//автор: NGadiyak
using System;
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Пользователи"), Description("Редактирование")]
    public class test_247250 : TestBase
    {
        private string login_old, name_old, email_old;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var list = Helpers.Query.GetFieldsOfTable("u_login, u_name, u_email", "helpdesk.users",
                                                      "u_employee_no is not null");

            login_old = list[0];
            name_old = list[1];
            email_old = list[2];
        }

        [Test]
        public void step_03()
        {
            var rand = new Random().Next(10000, 99999).ToString();

            Pages.HD.Users.Open("?rand=1&login=" + login_old);
            Pages.HD.Users.EditLogin(rand);

            var list = Helpers.Query.GetFieldsOfTable("u_login, u_name, u_email", "helpdesk.users",
                                                      "u_login = 'at_" + rand + "_l'");

            Assertion(
                "Ошибка при редактировании пользователя (логин)",
                () => Assert.AreNotEqual(login_old, list[0]));

            Assertion(
                "Ошибка при редактировании пользователя (имя)",
                () => Assert.AreNotEqual(name_old, list[1]));

            Assertion(
                "Ошибка при редактировании пользователя (email)",
                () => Assert.AreNotEqual(email_old, list[2]));
        }

    }
}
