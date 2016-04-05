//автор: NGadiyak
using AT;
using AT.WebDriver;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Изменение ответственного по ГП")]
    public class test_242175 : TestBase
    {
        private string u_name;
        private string u_group;
        private string u_login;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckOperChange("Авария", gp_id);
            CheckGroupChange("Авария", gp_id);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckOperChange("Подключения", gp_id);
            CheckGroupChange("Подключения", gp_id);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_resourse();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckOperChange("Ресурсы", gp_id);
            CheckGroupChange("Ресурсы", gp_id);
        }

        [Test]
        public void step_05()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_information();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckOperChange("Информация", gp_id);
            CheckGroupChange("Информация", gp_id);
        }

        private void CheckOperChange(string gp_type, string gp_id)
        {
            var list = Helpers.Query.GetFieldsOfTable("u_name, u_group, u_login", "helpdesk.users",
                                                      "u_status = 1 and u_group = 25 and u_name not like (' %')");

            u_name = list[0];
            u_group = list[1];
            u_login = list[2];

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpOperator(u_login);

            list = Helpers.Query.GetFieldsOfTable("t_operator", "helpdesk.tickets", "t_id=" + gp_id);

            Assertion(
                "некорректное обновление ответственного по ГП (оператор), ГП (тип: " + gp_type +
                "): " + gp_id,
                () => Assert.AreEqual(u_login, list[0]));

            list = Helpers.Query.GetFieldsOfTable("u_group", "helpdesk.users", "u_login = '" + u_login + "'");

            Assertion(
                "некорректное обновление ответственного по ГП (группа), ГП (тип: " + gp_type +
                "): " + gp_id,
                () => Assert.AreEqual(u_group, list[0]));
            Browser.AssertDialog();
        }

        private void CheckGroupChange(string gp_type, string gp_id)
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);

            var list = Helpers.Query.GetFieldsOfTable("u_group", "helpdesk.users",
                                                      "u_status not in (0,1,8) and u_group <> 25 and u_name not like (' %')");

            var u_group_id = list[0];
            u_group = Helpers.Query.GetFieldsOfTable("g_name", "helpdesk.groups", "g_id = " + u_group_id)[0];

            Pages.HD.GlobalComments.SetGpOperGroup(u_group);

            list = Helpers.Query.GetFieldsOfTable("t_operator", "helpdesk.tickets", "t_id=" + gp_id);
            u_login = list[0];

            list = Helpers.Query.GetFieldsOfTable("u_group", "helpdesk.users", "u_login = '" + u_login + "'");

            Assertion(
                "некорректное обновление ответственного по ГП (группа: " + u_group + "), ГП (тип: " + gp_type + "): " +
                gp_id,
                () => Assert.AreEqual(u_group_id, list[0]));

            Browser.AssertDialog();
        }
    }
}
