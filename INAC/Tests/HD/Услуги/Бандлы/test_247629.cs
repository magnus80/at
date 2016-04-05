//автор: NGadiyak
using AT;
using AT.DataBase;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("Бандлы"), Description("Отключение (ШПД + wifi rent)")]
    public class test_247629 : TestBase
    {
        private string bundle_id, login, ticket_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            bundle_id = Helpers.Services.Bundles.Queries.GetBundleVpdnAndWifiRent();

            var query = @"SELECT spl_login
                      FROM   inac.services_per_login
                      WHERE  spl_service = '" + bundle_id + "'";

            login = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Управление пакетным предложением";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Bundle_Services2.Open("?delete_current_bundle=1&ticket_id=" + ticket_id);
            Browser.AssertDialog();
        }

        [Test]
        public void step_06()
        {
            var status_text = Helpers.Tickets.Info.GetStatusText(ticket_id);

            Assertion(
                "некорректный статус тикета после отключения бандла (ticket_id = " + ticket_id +
                "), [ожидаемо: Закрыто, актуально: " +
                status_text + "] ",
                () => Assert.AreEqual("Закрыто", status_text));

            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка отключения бандла, [login: " + login + ", service: " + bundle_id + "] ",
                () => Assert.IsNullOrEmpty(list.Find(x => x.Equals(bundle_id))));
        }
    }
}
