//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("Нетфон"), Category("1"), Description("Подключение")]
    public class test_246734 : TestBase
    {
        private string login, service, nickname, ticket_id, phone;

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
            Pages.HD.Address.TType = "Нетфон. Подключение услуги";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            var query = @"SELECT DISTINCT s_id
                      FROM   inac.services
                             JOIN inac.services_param
                               ON s_id = id_service
                      WHERE  param_name = 'NETPHONE'
                             AND param_number = 1
                             AND s_city = 12042
                             AND s_f_public = 1";

            service = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
            nickname = "at__" + DateTime.Now.ToShortDateString().Replace('.', '_') + "__" +
                       new Random().Next(0, 100).ToString("0");

            Pages.HD.Netfone_add.Open("?ticket_id=" + ticket_id);
            Pages.HD.Netfone_add.AddNetphone(nickname, service);
            Pages.HD.Queues.OpenLogin(login);
        }

        [Test]
        public void step_05() /* заглушка, имитирующая корректную работу АСР Атлант */
        {
            phone = "74999" + new Random().Next(100000, 999999).ToString("0");

            var query = @"UPDATE inac.netphone_queue
                      SET    state = 1
                      WHERE  id = ((SELECT id
                                    FROM   (SELECT id
                                            FROM   inac.netphone_queue
                                            WHERE  login = '" + login + @"'
                                            ORDER  BY dt DESC)
                                    WHERE  ROWNUM = 1))";

            Executor.ExecuteSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.Netphone.Confirm(login, nickname, phone);
        }

        [Test]
        public void step_06()
        {
            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка подключения нетфона, [login: " + login + ", service: " + service +
                "] ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals(service))));
        }

        [Test]
        public void step_07()
        {
            var status_text = Helpers.Tickets.Info.GetStatusText(ticket_id);

            Assertion(
                "некорректны статус заявки на подключение нетфона [ожидается: закрыто] ",
                () => Assert.AreEqual(status_text, "Закрыто"));

        }

        [Test]
        public void step_08()
        {
            var query = @"SELECT tt_type
                      FROM   helpdesk.tickets
                             JOIN inac.contracts
                               ON c_address0 = t_address
                             JOIN inac.logins
                               ON l_contract = c_id
                             JOIN helpdesk.ticket_types
                               ON t_type = tt_id
                      WHERE  l_login = '" + login + @"' 
                             AND tt_id = 160";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0);

            Assertion(
                "не создалась заявка 'Заключение договора с клиентом', [login: " + login +
                ", service: " + service + "] ",
                () => Assert.IsNotEmpty(list));
        }

        [Test]
        public void step_09()
        {
            var query = @"SELECT DISTINCT To_char(c_text)
                      FROM   helpdesk.comments
                      WHERE  c_ticket = " + ticket_id + @"
                             AND c_text LIKE ( '%" +
                        nickname + @"%' )
                              OR c_text LIKE ( '%" + phone + @"%' )";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0);

            Assertion(
                "некорректные комментарии к заявке подключения нетфона, [login: " + login +
                ", ticket: " + ticket_id + "] ",
                () => Assert.AreEqual(list.Count, 2));
        }
    }
}
