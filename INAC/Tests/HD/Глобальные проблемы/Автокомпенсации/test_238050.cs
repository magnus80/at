//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_Автокомпенсации по ГП")]
    public class test_238050 : TestBase
    {
        private string login = string.Empty;
        private string gp_id = string.Empty;
        private string t_id = string.Empty;


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

            Assertion( "ошибка при создании неаварийной ГП", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_03()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Компенсация";

            Pages.HD.Address.NewTicket();

            t_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion( "ошибка при создании заявки на компенсацию, абонент:" + login, () => Assert.IsNotEmpty(t_id));
            Helpers.Tickets.Actions.LintToGlobalProblem(t_id, gp_id);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }

        [Test]
        public void step_05()
        {
            Executor.ExecuteProcedure("inac.auto_compensations_pkg.process_pay", Environment.InacDb);

            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            var t_status = Helpers.Tickets.Info.GetStatusText(t_id);
            Assertion( "Некорректный статус тикета после закрытия ГП", () => Assert.IsTrue(t_status.Equals("Закрыта")));
        }

       
    }
}
