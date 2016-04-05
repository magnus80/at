//автор: NGadiyak
using AT;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Изменение статуса ГП")]
    public class test_242241 : TestBase
    {
        private string status;
        private string status_new;

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
            CheckChangeStatus("Авария", gp_id);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeStatus("Подключения", gp_id);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_resourse();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeStatus("Ресурсы", gp_id);
        }

        [Test]
        public void step_05()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_information();
            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            CheckChangeStatus("Информация", gp_id);
        }

        private void CheckChangeStatus(string gp_type, string gp_id)
        {
            status = "Закрыта";

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");

            status_new = Helpers.Tickets.Info.GetStatusText(gp_id);

            Assertion(
                "некорректное обновление статуса проблемы, ГП (тип: " + gp_type +
                "): " + gp_id,
                () => Assert.AreEqual(status, status_new));

            status = "Открыта";

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Открыта");

            status_new = Helpers.Tickets.Info.GetStatusText(gp_id);

            Assertion(
                "некорректное обновление статуса проблемы, ГП (тип: " + gp_type +
                "): " + gp_id,
                () => Assert.AreEqual(status, status_new));
        }
    }
}
