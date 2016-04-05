//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Комментарии")]
    public class test_243278 : TestBase
    {
        private string login;

        [Test]
        public void step_01()
        {
            login = Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckComments(gp_id, "Авария");
        }

        [Test]
        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckComments(gp_id, "Подключение");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_information();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckComments(gp_id, "Информация");
        }

        [Test]
        public void step_05()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_resourse();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckComments(gp_id, "Ресурсы");
        }

        private void CheckComments(string gp_id, string type)
        {
            var comment = "at_test_comment_" + type;

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.Comment = comment;

            var list = Helpers.Query.GetFieldsOfTable("TO_CHAR(c_text), c_operator", "helpdesk.comments",
                                                      "c_ticket = " + gp_id + " and to_char(c_text) = '" + comment + "'");

            Assertion("ошибка при добавлении комментария (текст), ГП: " + gp_id + "[ожидаемо: " + comment + "; актуально: " + list[0] + "]", () => Assert.AreEqual(comment, list[0]));
            Assertion("ошибка при добавлении комментария (логин), ГП: " + gp_id, () => Assert.AreEqual(login, list[1]));
        }
    }
}
