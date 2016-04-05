//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("ШПД"), Category("1"), Description("Смена Тарифа")]
    public class test_246603 : TestBase
    {
        private string login, ticket_id, service_old, service_new;

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
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Сменить тариф";
            Pages.HD.Address.NewTicket();
        }

        [Test]
        public void step_04()
        {
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            service_old = Helpers.Abonents.Info.GetServices(login)[0];

            service_new = Helpers.Query.GetFieldsOfTable("s_id", "inac.services",
                                                         "s_f_public = 1 and s_abontype = 1 and s_f_vpdn = 1 and s_currency = 2 and s_city = 12042 and s_id <> '" +
                                                         service_old + "'")[0];

            Pages.HD.Services.Open("?ticket_id=" + ticket_id + "&login=" + login + "&&oservice=");
            Pages.HD.Services.SelectService(service_new);
        }

        [Test]
        public void step_05()
        {
            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка смены тарифа ШПД (не отключился старый сервис), [login: " + login + ", service_old: " +
                service_old + "] ",
                () => Assert.IsNullOrEmpty(list.Find(x => x.Equals(service_old))));

            Assertion(
                "ошибка смены тарифа ШПД (не подключился новый сервис), [login: " + login + ", service_new: " +
                service_new + "] ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals(service_new))));
        }
    }
}
