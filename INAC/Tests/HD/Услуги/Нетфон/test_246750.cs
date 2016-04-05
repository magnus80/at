//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("Нетфон"), Description("Смена тарифа")]
    public class test_246750 : TestBase
    {
        private string login, service_old, service_new, nickname, ticket_id, phone;

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

            service_old = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
            nickname = "at__" + DateTime.Now.ToShortDateString().Replace('.', '_') + "__" +
                       new Random().Next(0, 100).ToString("0");

            Pages.HD.Netfone_add.Open("?ticket_id=" + ticket_id);
            Pages.HD.Netfone_add.AddNetphone(nickname, service_old);
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

            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.Netphone.Confirm(login, nickname, phone);
        }

        [Test]
        public void step_06()
        {
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Нетфон. Смена тарифного плана";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_07()
        {
            var query = @"SELECT DISTINCT s_id
                      FROM   inac.services
                             JOIN inac.services_param
                               ON s_id = id_service
                      WHERE  param_name = 'NETPHONE'
                             AND param_number = 1
                             AND s_city = 12042
                             AND s_f_public = 1
                             AND s_id <> '" + service_old + "'";

            service_new = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            Pages.HD.Netfone_add.Open("?ticket_id=" + ticket_id);
            Pages.HD.Netfone_add.ChangeNetphone(service_new);

            query = @"UPDATE inac.netphone_queue
                      SET    state = 101
                      WHERE  id = ((SELECT id
                                    FROM   (SELECT id
                                            FROM   inac.netphone_queue
                                            WHERE  login = '" + login + @"' AND srv_id = '" +
                    service_old + @"' 
                                            ORDER  BY dt DESC)
                                    WHERE  ROWNUM = 1))";

            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.Netphone.Delete(login, "0");


            query = @"UPDATE inac.netphone_queue
                      SET    state = 1
                      WHERE  id = ((SELECT id
                                    FROM   (SELECT id
                                            FROM   inac.netphone_queue
                                            WHERE  login = '" + login + @"' AND srv_id = '" +
                    service_new + @"' 
                                            ORDER  BY dt DESC)
                                    WHERE  ROWNUM = 1))";

            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.Netphone.Confirm(login, nickname, phone);
        }

        [Test]
        public void step_08()
        {
            var list = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "ошибка смены тарифа нетфона, [login: " + login + ", service_old: " +
                service_old + ", service_new: " + service_new +
                "] ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals(service_new))));
        }
    }
}
