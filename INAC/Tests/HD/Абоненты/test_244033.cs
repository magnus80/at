//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Абоненты"), Description("Расторжение")]
    public class test_244033 : TestBase
    {
        private string login;
        private string address;
        private string ticket_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();

            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);

            address = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + address);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Расторжение";

            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Stop_bc.Open("?address_id=" + address + "&ticket_id=" + ticket_id);
            Pages.HD.Stop_bc.CancelContract();
        }

        [Test]
        public void step_05()
        {
            var contract = Helpers.Abonents.Info.GetContract(login);
            Helpers.Abonents.Actions.Reaccount(contract);

            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "Расторжение некорректно, остались подключенные сервисы, [login: " + login + "]",
                () => Assert.IsEmpty(list));
        }
    }
}
