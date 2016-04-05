//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("Бандлы"), Description("Подключение (ШПД + wifi rent)")]
    public class test_247523 : TestBase
    {
        private string login, ticket_id, bundle_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Конфигурация пакета";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            bundle_id = Helpers.Services.Bundles.Queries.GetBundleVpdnAndWifiRent();
            var router = Helpers.Other.Queries.GetWifiRouterSerial();

            Pages.HD.Bundle_Services2.Open("?login=" + login + "&ticket_id=" + ticket_id);
            Pages.HD.Bundle_Services2.ConnectBundleVdpnAndWifiRent(bundle_id, router);
        }

        [Test]
        public void step_05()
        {
            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка подключения бандла, [login: " + login + ", service: " + bundle_id + "] ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals(bundle_id))));
        }
    }
}
