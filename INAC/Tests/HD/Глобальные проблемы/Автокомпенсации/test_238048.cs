//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_Автокомпенсации по ГП")]
    public class test_238048 : TestBase
    {
        private string gp_id = string.Empty;
        private string login = string.Empty;
        private string t_id = string.Empty;

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();

            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("ошибка при создании аварийной ГП", () => Assert.IsNotEmpty(gp_id));

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Компенсация";

            Pages.HD.Address.NewTicket();

            t_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("ошибка при создании заявки на компенсацию, абонент:" + login, () => Assert.IsNotEmpty(t_id));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.LinkToGp(gp_id);
            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.TStatus = "Ожидание компенсации";
            Pages.HD.Comments.SubmitTicket();

            var query = @"UPDATE helpdesk.tickets
                      SET    t_cdate = sysdate - 10,
                           t_mdate = sysdate - 10
                      WHERE  t_id = " + t_id;
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            query = @"UPDATE helpdesk.tickets
                      SET    t_cdate = sysdate - 5,
                           t_mdate = sysdate - 5
                      WHERE  t_id = " + gp_id;

            Executor.ExecuteUnSelect(query, Environment.InacDb);

        }

        [Test]
        public void step_05()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }

        [Test]
        public void step_06()
        {
            Pages.HD.Comments.Open("?ticket_id=" + t_id);

            Executor.ExecuteProcedure("inac.auto_compensations_pkg.process_pay", Environment.InacDb);

            var t_status = Helpers.Tickets.Info.GetStatusText(t_id);
            Assertion("Некорректный статус тикета после закрытия ГП, ГП: " + gp_id,
                      () => Assert.IsTrue(t_status.Equals("Закрыта")));
        }
    }
}
