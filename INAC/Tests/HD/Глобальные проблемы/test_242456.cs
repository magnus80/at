//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Полное отсутствие сервиса")]
    public class test_242456 : TestBase
    {
        private string gp_id;

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
            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_03()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.ServiceFailure = false;

            var param_num =
                Helpers.Query.GetFieldsOfTable("param_number", "helpdesk.tickets_params", "t_id=" + gp_id)[0];
            Assertion("ошибка при смене даты окончания ГП, ГП: " + gp_id, () => Assert.AreEqual(param_num, ""));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.ServiceFailure = true;

            var param_num =
                Helpers.Query.GetFieldsOfTable("param_number", "helpdesk.tickets_params",
                                               "param_name = 'SERVICE FAILURE' and t_id=" + gp_id)[0];
            Assertion("ошибка при установке флаша полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param_num, "1"));
        }
    }
}
