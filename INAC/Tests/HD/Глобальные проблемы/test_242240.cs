//автор: NGadiyak
using AT;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Изменение контроля ГП")]
    public class test_242240 : TestBase
    {
        private string controller;
        private string controller_new;

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
            CheckChangeController("Авария", gp_id);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeController("Подключения", gp_id);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_resourse();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeController("Ресурсы", gp_id);
        }

        [Test]
        public void step_05()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_information();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeController("Информация", gp_id);
        }

        private void CheckChangeController(string gp_type, string gp_id)
        {
            controller = Helpers.Tickets.Queries.GetAnyGpController();

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpController(controller);

            controller_new =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and tt.t_id=" + gp_id)[0];

            Assertion(
                "некорректное обновление контроля проблемы, ГП (тип: " + gp_type +
                "): " + gp_id,
                () => Assert.AreEqual(controller_new, controller));
        }
    }
}
