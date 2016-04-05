//автор: NGadiyak

using AT;
using AT.DataBase;
using INAC.WebPages;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("IPTV"), Category("1"), Description("Деактивация")]
    public class test_246266 : TestBase
    {
        private string login;
        private string service;
        private string ticket_id;

        private int count_old, count_new;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT spl_login,
                             spl_service
                      FROM   inac.services_per_login
                             JOIN (SELECT *
                                   FROM   inac.services
                                   minus
                                   (SELECT s.*
                                    FROM   inac.services s
                                           JOIN inac.services_param
                                             ON s.s_id = id_service
                                    WHERE  param_name IN ( 'RENT', 'UNRENT' )))
                               ON ( spl_service = s_id
                                    AND s_f_iptv = 1
                                    AND s_city = 12042 )
                             JOIN inac.logins
                               ON ( spl_login = l_login
                                    AND l_block_type = 0 )
                      GROUP  BY spl_login,
                                spl_service
                      HAVING Count (spl_login) = 1 
                      ";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();
            login = list[0];
            service = list[1];

            count_old = Helpers.Abonents.Info.GetServices(login).Count;
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
            var address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "IPTV заявки";
            Pages.HD.Address.TType = "Деактивация IPTV";
            Pages.HD.Address.NewTicket();
            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Deliptv.Open("?ticket_id=" + ticket_id + "&act=1");
            Pages.HD.Deliptv.DeleteIptv();
            count_new = Helpers.Abonents.Info.GetServices(login).Count;
        }

        [Test]
        public void step_05()
        {
            Assertion(
                "ошибка деактивации IPTV, [login: " + login + ", service: " + service + "] ",
                () => Assert.Less(count_new, count_old));
        }
    }
}
