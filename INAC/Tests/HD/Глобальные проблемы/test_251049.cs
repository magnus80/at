//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы")]
    public class test_251049 : TestBase
    {
        private string gp_id;
        private string t_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();

            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Assertion("ошибка при создании ГП подключения",
                      () => Assert.IsNotEmpty(gp_id));
            }

        [Test]
        public void step_03()
        {
            var query = @"SELECT DISTINCT t_id 
                    FROM   helpdesk.tickets 
                    WHERE  t_type = 1 and t_status IN (1, 11,82,94, 96,98) 
                           AND t_address IN (SELECT a_id 
                                             FROM   inac.addresses0 
                                                    JOIN inac.houses0 
                                                      ON a_house = h_id 
                                                    JOIN inac.areas 
                                                      ON ar_id = h_dealer 
                                             WHERE  ar_city = 12042)";

            t_id = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);
            Assertion("нет заявок на подключение в БД, выборка пустая (Москва)",
                      () => Assert.IsNotEmpty(t_id));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.TStatus = "Новая";
            Pages.HD.Comments.LinkToGp(gp_id);
            Pages.HD.Comments.SubmitTicket();
        }

        [Test]
        public void step_05()
        {
            var query = @"SELECT s_id,
                                 s_street,
                                 h_id
                          FROM   inac.streets0
                                 JOIN inac.houses0
                                   ON h_street = s_id
                          WHERE  h_id IN (SELECT a_house
                                          FROM   inac.addresses0
                                                 JOIN helpdesk.tickets
                                                   ON t_address = a_id
                                          WHERE  t_id = " + t_id + ")";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);


            Pages.HD.Scalechange.Open("?ticket_id=" + gp_id);
            Pages.HD.Scalechange.SetScaleHouse(list[0, 1], list[0, 0], list[0, 2]);
            Executor.ExecuteProcedure("helpdesk.problem_tickets", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            var t_status = Helpers.Tickets.Info.GetStatusText(t_id);
            Assertion(
                "Некорректный статус заявки (" + t_id +
                "), ожидаемо: Проблемное подключение, актуально: " + t_status,
                () => Assert.AreEqual("Проблемное подключение", t_status));
        }

        public void step_07()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpType("Авария");

            var t_type = Helpers.Tickets.Info.GetTypeText(gp_id);
            Assertion(
                "Некорректный тип заявки (" + gp_id +
                "), ожидаемо: Авария, актуально: " + t_type,
                () => Assert.AreEqual("Авария", t_type));
        }

        [Test]
        public void step_08()
        {
            Executor.ExecuteProcedure("helpdesk.problem_tickets", Environment.InacDb);
        }

        [Test]
        public void step_09()
        {
            var t_status = Helpers.Tickets.Info.GetStatusText(t_id);
            Assertion(
                "Некорректный статус заявки (" + t_id +
                "), ожидаемо: Проблемное подключение, актуально: " + t_status,
                () => Assert.AreEqual("Проблемное подключение", t_status));
        }

        [Test]
        public void step_10()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpType("Информация");

            var t_type = Helpers.Tickets.Info.GetTypeText(gp_id);
            Assertion(
                "Некорректный тип заявки (" + gp_id +
                "), ожидаемо: Информация, актуально: " + t_type,
                () => Assert.AreEqual("Информация", t_type));
        }

        [Test]
        public void step_11()
        {
            Executor.ExecuteProcedure("helpdesk.problem_tickets", Environment.InacDb);
        }

        [Test]
        public void step_12()
        {
            var t_status = Helpers.Tickets.Info.GetStatusText(t_id);
            Assertion(
                "Некорректный статус заявки (" + t_id +
                "), ожидаемо: Позвонить клиенту, актуально: " + t_status,
                () => Assert.AreEqual("Позвонить клиенту", t_status));
        }
    }
}
