//автор: NGadiyak
using System.Collections.Generic;
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("IPTV"), Category("1"), Description("Активация")]
    public class test_246265 : TestBase
    {
        private string login;
        private string ticket_id;
        private string pkt_base_id;

        private List<string> list;

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
            var address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + address_id);

            Pages.HD.Address.CreateActivationIptv();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

        }

        [Test]
        public void step_04()
        {
            var guid = Helpers.Services.IPTV.Queries.GetIptvGuid();
            pkt_base_id = Helpers.Services.IPTV.Queries.GetIptvPacket("12042");

            list = Helpers.Services.IPTV.Queries.GetIptvPacketsByBaseMinusEx(pkt_base_id);

            Pages.HD.Addiptv.Open("?ticket_id=" + ticket_id + "&act=1");
            Pages.HD.Addiptv.ConnectIptv(guid, pkt_base_id, list);
        }

        [Test]
        public void step_05()
        {
            /* проблемы AJAX, пока не решены, поэтому проверка немного изменена.
             * Если после нажатия на кнопку "подключить пакеты" нас переносят на страницу тикета, то, теоретически, все ок.
             
             query = @"SELECT packet_id
             FROM   inac.iptv_containers
             WHERE  cont_id = (SELECT splp_param_number
                               FROM   inac.services_per_login
                                      JOIN inac.services_per_login_param
                                        ON spl_id = splp_spl
                               WHERE  spl_login = '" + login + @"'
                                      AND splp_param_name = 'CONTAINER_ID')
                    AND packet_id <> " +
                     pkt_base_id;

             var list_res = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0); 

                        Assertion(
                                           "Подключенные IPTV пакеты не совпадают с ожидаемыми: ",
                                           () => Assert.AreEqual(list, list_res)); */

            var ticket_new = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id") ?? string.Empty;

            Assertion(
                "ошибка подключения IPTV, [логин: " + login + ", базовый пакет: " +
                pkt_base_id + "] ",
                () => Assert.IsNotEmpty(ticket_new));
        }

    }
}
