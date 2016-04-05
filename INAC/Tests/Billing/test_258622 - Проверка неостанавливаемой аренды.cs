using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_258622 : TestBase
    {
        private string login, adress, ticket, service_fttb, service_rent, rent_price;
        private float balance, price_sum;

        [Test]
        public void step_01()
        {
            var service = Helpers.Services.FTTB.Queries.GetFttbServiceUnlim("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service, 10000);
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_03()
        {
            adress = Helpers.Abonents.Info.GetAdressByLogin(login);

            service_fttb = Helpers.Abonents.Info.GetServices(login)[0];

            Pages.HD.Address.Open("?address_id=" + adress);
            Pages.HD.Address.CreateTicket("IPTV заявки", "Аренда IPTV. Выдача");
        }

        [Test]
        public void step_04()
        {
            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("Ошибка при создании заявки Аренда IPTV. Выдача", () => Assert.IsNotNullOrEmpty(ticket));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Rentiptv2.Open("?act=1&command=rent&ticket_id=" + ticket);
            var guid = Helpers.Services.IPTV.Queries.GetIptvGuid();
            Pages.HD.Rentiptv2.RenIptv(guid);

            service_rent = Helpers.Query.GetFieldsOfTable("spl_service", "inac.services_per_login",
                                                          "spl_login = '" + login + "' and spl_service <> '" +
                                                          service_fttb + "'")[0];

            rent_price = Helpers.Query.GetFieldsOfTable("s_price", "inac.services", "s_id = '" + service_rent + "'")[0];
        }

        [Test]
        public void step_06()
        {
            var balance_new = Helpers.Abonents.Info.GetBalanceByLogin(login);
            balance = float.Parse(balance_new);
        }

        [Test]
        public void step_07()
        {
            price_sum = float.Parse(Helpers.Abonents.Info.GetServicesPriceSum(login));
            var r_price = float.Parse(rent_price);

            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(((-1)*balance + price_sum - r_price - 5).ToString(), login);

            var balance_new = float.Parse(Helpers.Abonents.Info.GetBalanceByLogin(login));

            Assertion("Ошибка при начислении денег", () => Assert.AreEqual(balance_new, price_sum - r_price - 5));
        }

        [Test]
        public void step_08()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -35);
            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_09()
        {
            Pages.HD.Address.Open("?address_id=" + adress);
            var query = @"select ss_service from inac.start_stop where ss_login = '" + login + "' and ss_stop is null";
            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Выборка неостанавливаемых сервисов на логине (выборка пустая), логин " + login,
                      () => Assert.IsTrue(list.Count > 0));
            Assertion("Сервис неостанавливаемой аренды остановился, логин " + login,
                      () => Assert.AreEqual(list[0, 0], service_rent));
        }
    }
}
