//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("IPTV"), Category("1"), Description("Подключение архивной аренды")]
    public class test_246267 : TestBase
    {
        private string login, service, ticket_id, address_id, guid;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
            address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "IPTV заявки";
            Pages.HD.Address.TType = "Аренда IPTV. Выдача";
            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            guid = Helpers.Services.IPTV.Queries.GetIptvGuid();
            Pages.HD.Rentiptv2.Open("?act=1&command=rent&ticket_id=" + ticket_id);
            Pages.HD.Rentiptv2.RenIptv(guid);

            var query = @"SELECT spl_service
                      FROM   inac.services_per_login
                           JOIN inac.services
                             ON ( s_id = spl_service
                                  AND s_nonstop = 1 )
                      WHERE  spl_login = '" + login + "'";

            service = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "ошибка подключения аренды IPTV, [логин: " + login + ", guid: " + guid + "] ",
                () => Assert.IsNotEmpty(service));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "IPTV заявки";
            Pages.HD.Address.TType = "Аренда IPTV. Прием";
            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_06()
        {
            Pages.HD.Rentiptv2.Open("?&act=1&command=unrent&ticket_id=" + ticket_id);
            Pages.HD.Rentiptv2.UnrentIptv();
        }

        [Test]
        public void step_07()
        {
            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "IPTV заявки";
            Pages.HD.Address.TType = "Подключение архивного сервиса аренды IPTV";
            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_08()
        {
            IsContinueOnStepFail = true;
            var query = @"UPDATE inac.services_param
                      SET    param_name = '1_RENT'
                      WHERE  param_name = 'RENT'
                           AND id_service = '" + service + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

        }

        [Test]
        public void step_09()
        {
            Pages.HD.PTN.IPTV.Archive_Rent.Open(ticket_id + "?login=" + login);
            Pages.HD.PTN.IPTV.Archive_Rent.ConnectArchRent(guid);
        }

        [Test]
        public void step_10()
        {
            var list = Helpers.Abonents.Info.GetServices(login);


            Assertion(
                "ошибка подключения архивной аренды IPTV, [login: " + login + ", service: " + service + "] ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals(service))));
        }

        [Test]
        public void step_11()
        {
            var query = @"UPDATE inac.services_param
                      SET    param_name = 'RENT'
                      WHERE  param_name = '1_RENT'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
    }
}
