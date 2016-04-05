//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("Нетфон"), Description("Отключение")]
    public class test_246737 : TestBase
    {
        private string login, ticket_id, service;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT spl_login, spl_service FROM inac.services_per_login WHERE spl_service in (
                                  SELECT DISTINCT s_id
                                            FROM   inac.services
                                                   JOIN inac.services_param
                                                     ON s_id = id_service
                                            WHERE  param_name = 'NETPHONE'
                                                   AND param_number = 1
                                                   AND s_city = 12042
                                                   AND s_f_public = 1)";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();
            login = list[0];
            service = list[1];
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Нетфон. Отключение услуги";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Netfone_add.Open("?ticket_id=" + ticket_id);
            Pages.HD.Netfone_add.DelNetphone();
        }

        [Test]
        public void step_05()
        {
            var query = @"UPDATE inac.netphone_queue
                      SET    state = 101
                      WHERE  id = ((SELECT id
                                    FROM   (SELECT id
                                            FROM   inac.netphone_queue
                                            WHERE  login = '" + login + @"'
                                            ORDER  BY dt DESC)
                                    WHERE  ROWNUM = 1))";

            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.Netphone.Delete(login, "0");
        }

        [Test]
        public void step_06()
        {
            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка отключения нетфона, [login: " + login + ", service: " + service +
                "] ",
                () => Assert.IsNullOrEmpty(list.Find(x => x.Equals(service))));
        }
    }
}
