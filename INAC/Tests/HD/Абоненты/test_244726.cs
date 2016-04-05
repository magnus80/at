//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Абоненты"), Description("Сценарий переезда абонента")]
    public class test_244726 : TestBase
    {
        private string login;
        private string adr_old, adr_new;
        private string ticket_id, ticket_id_1;
        private string contract;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            contract = Helpers.Abonents.Info.GetContract(login);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);

            adr_old = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + adr_old);

            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Переезд";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Pages.HD.Address.Open("?address_id=" + adr_old);

            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Возврат ош.платежа";
            Pages.HD.Address.NewTicket();
            ticket_id_1 = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Changeadress.Open("?ticket_id=" + ticket_id + "&contract=" + contract);
            var street_id = Helpers.Adresses.Queries.GetConnectedStreet("12042");
            var street = Helpers.Adresses.Queries.GetStreetName(street_id);
            var house_id = Helpers.Adresses.Queries.GetConnectedHouse(street_id);

            Pages.HD.Changeadress.Street = street;

            Pages.HD.Changeadress2.Open("?street_id=" + street_id + "&ticket_id=" + ticket_id + "&contract=" + contract);
            Pages.HD.Changeadress3.Open("?house_id=" + house_id + "&street_id=" + street_id + "&ticket_id=" + ticket_id +
                                        "&contract=" + contract);

            Pages.HD.Changeadress3.Flat = "123";
            Pages.HD.Changeadress3.Room = "123";
            Pages.HD.Changeadress3.Write();
        }

        [Test]
        public void step_05()
        {
            adr_new = Helpers.Query.GetFieldsOfTable("c_address0", "inac.contracts", "c_id = " + contract)[0];
            var list = Helpers.Query.GetFieldsOfTable("t_id", "helpdesk.tickets", "t_address = " + adr_old);
            var oldadr_type = Helpers.Tickets.Info.GetTypeText(list[0]);

            Assertion(
                "ошибка при переезде, на новом адресе должна быть заявка 'Перенесенный договор'",
                () => Assert.AreEqual(oldadr_type, "Перенесенный договор"));
        }

        [Test]
        public void step_06()
        {
            var newadr_ticket =
                Helpers.Query.GetFieldsOfTable("t_address", "helpdesk.tickets", "t_id = " + ticket_id_1)[0];

            Assertion(
                "не перенеслась заявка " + ticket_id_1,
                () => Assert.AreEqual(newadr_ticket, adr_new));

            var query = @"SELECT To_char(c_text)
                      FROM   helpdesk.comments
                      WHERE  c_ticket = " + ticket_id +
                        @" AND To_char(c_text) LIKE ( '%" + adr_old + "%' )";

            var res = Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];

            Assertion(
                "ошибка при переезде, не добавился комментарий в заявку " + ticket_id,
                () => Assert.IsNotEmpty(res));
        }
    }
}
