//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Изменение даты окончания ГП")]
    public class test_242242 : TestBase
    {
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
            
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetResolveDate("2020");
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            var res_date_1 = Pages.HD.GlobalComments.ResolveDate.Substring(0, 10);
            
            var res_date_2 = Helpers.Query.GetFieldsOfTable("gp_resolve_date", "helpdesk.global_problems", "gp_id=" + gp_id)[0].Substring(0, 10);
            
            Assertion("ошибка при смене даты окончания ГП, ГП: " + gp_id, () => Assert.AreEqual(res_date_1, res_date_2));

        }

        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            var res_date_1 = Pages.HD.GlobalComments.ResolveDate.Substring(0, 10);

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetResolveDate("2010");

            var res_date_2 =
                Helpers.Query.GetFieldsOfTable("gp_resolve_date", "helpdesk.global_problems", "gp_id=" + gp_id)[0].
                    Substring(0, 10);
            
            Assertion("ошибка при смене даты окончания ГП, ГП: " + gp_id, () => Assert.AreEqual(res_date_1, res_date_2));
        }
    }
}
