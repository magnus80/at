//автор: NGadiyak

using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("IPTV"), Description("Смена ТП")]
    public class test_246529 : TestBase
    {
        private string login, contract, address_id, ticket_id, pkt_base_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT spl_login, spl_service
                      FROM   inac.services_per_login
                             JOIN inac.logins
                               ON spl_login = l_login
                      WHERE  l_block_type = 0
                             AND l_login LIKE ( '089%' )
                             AND spl_service IN (SELECT s_id
                                                 FROM   inac.services
                                                 WHERE  s_f_iptv = 1 AND s_city = 12042
                                                 MINUS
                                                 SELECT DISTINCT id_service
                                                 FROM   inac.services_param
                                                 WHERE  param_name IN ( 'RENT', 'UNRENT' ))
                      GROUP  BY spl_login, spl_service
                      HAVING Count (spl_service) = 1";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();
            login = list[0];
            //iptv_old = list[1];
        }

        [Test]
        public void step_03()
        {
            contract = Helpers.Abonents.Info.GetContract(login);
            Helpers.Abonents.Actions.Payments.AddPaymentToContract("100000", contract);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Queues.OpenLogin(login);
            address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "IPTV заявки";
            Pages.HD.Address.TType = "Смена тарифного плана IPTV";
            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_05()
        {
            var query = @"SELECT DISTINCT pkt_id
                      FROM   inac.iptv_packets_cap
                             JOIN inac.iptv_packets_param
                               ON pkt_id = id_packet
                      WHERE  pkt_parent IS NULL
                             AND pkt_city_id = 12042
                      minus
                      SELECT DISTINCT packet_id
                      FROM   inac.iptv_containers
                      WHERE  cont_id = (SELECT splp_param_number
                                        FROM   inac.services_per_login
                                               JOIN inac.services_per_login_param
                                                 ON spl_id = splp_spl
                                        WHERE  spl_login = '" + login + @"'
                                               AND splp_param_name = 'CONTAINER_ID') 
                      ";

            pkt_base_id = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            var list = Helpers.Services.IPTV.Queries.GetIptvPacketsByBaseMinusEx(pkt_base_id);

            Pages.HD.Changeiptv.Open("?act=1&ticket_id=" + ticket_id);
            Pages.HD.Changeiptv.ChangeIptv(pkt_base_id, list);
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT packet_id
             FROM   inac.iptv_containers
             WHERE  cont_id = (SELECT splp_param_number
                               FROM   inac.services_per_login
                                      JOIN inac.services_per_login_param
                                        ON spl_id = splp_spl
                               WHERE  spl_login = '" + login + @"'
                                      AND splp_param_name = 'CONTAINER_ID')
                    AND packet_id = " +
                        pkt_base_id;

            var list_res = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0);

            Assertion(
                "Ошибка при смене сервиса IPTV, [login: " + login + "]",
                () => Assert.IsNotNull(list_res));
        }
    }
}
