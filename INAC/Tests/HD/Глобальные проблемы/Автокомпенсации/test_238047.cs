//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_Автокомпенсации по ГП")]
    public class test_238047 : TestBase
    {
        private string gp_id = string.Empty;
        private string login = string.Empty;
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
            Pages.HD.New_global.NewGP_crash();

            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion( "ошибка при создании аварийной ГП", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_03()
        {
            login = Helpers.Abonents.Actions.Creation.Create();

            Helpers.Abonents.Actions.Payments.AddPaymentToContract("-10000", Helpers.Abonents.Info.GetContract(login));

            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Компенсация";

            Pages.HD.Address.NewTicket();

            t_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion( "ошибка при создании заявки на компенсацию, абонент:" + login,
                                          () => Assert.IsNotEmpty(t_id));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.LinkToGp(gp_id);
            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.TStatus = "Ожидание компенсации";
            Pages.HD.Comments.SubmitTicket();
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
            Assertion( "Некорректный статус тикета после закрытия ГП", () => Assert.IsTrue(t_status.Equals("Закрыта")));
        }
    }
}


