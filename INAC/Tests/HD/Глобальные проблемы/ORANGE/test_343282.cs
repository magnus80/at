//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange_нотификация_email"), Category("time_limit")]
    public class test_243282 : TestBase
    {
        private string gp_id;
        private string t_id;
        private string text;

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
        }

        [Test]
        public void step_03()
        {
            var login = Helpers.Abonents.Actions.Creation.Create();
            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Аварийная";
            Pages.HD.Address.TType = "Компенсация";

            Pages.HD.Address.NewTicket();

            t_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion( "ошибка при создании заявки на компенсацию, абонент:" + login, () => Assert.IsNotEmpty(t_id));

            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.LinkToGp(gp_id);

            var t_global = Helpers.Query.GetFieldsOfTable("t_global", "helpdesk.tickets", "t_id = " + t_id)[0];

            Assertion( "ошибка привязки тикета к ГП (тикет: " + t_id + ", ГП: " + gp_id + ")", () => Assert.AreEqual(t_global, gp_id));

            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.SetNotifEmail();

        }

        [Test]
        public void step_04()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.Notification_settings.Open();
            text =  Pages.HD.Notification_settings.ChangeNewEmailTemp();

        }

        [Test]
        public void step_05()
        {
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_notif"));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "1"));
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            var timer = Helpers.Query.GetFieldsOfTable("t_timer", "helpdesk.tickets", "t_id = " + gp_id)[0];
        }

        [Test]
        public void step_07()
        {
            var query = @"
            UPDATE helpdesk.tickets 
            SET    t_timer = SYSDATE - interval '1' minute 
            WHERE  t_id = " + gp_id;
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Executor.ExecuteProcedure("inac.helpdesk_notification.p_send_gp_timer", Environment.InacDb);
        }

        [Test]
        public void step_08()
        {
            var query = @"
            SELECT q_id 
            FROM   inac.queues 
            WHERE  q_operation = 'SendEmail' 
            ORDER  BY q_submit_date DESC ";

            var queue = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Helpers.Queues.Actions.RunMailProcessor(queue);

            query = @"
            SELECT e_message 
            FROM   inac.email_notify_spool 
            WHERE  e_message = '" + text + "' ";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            if (Convert.ToInt32(DateTime.Now.ToString("HH tt")) <= 20 && Convert.ToInt32(DateTime.Now.ToString("HH tt")) >= 10)
            {
                Assertion( "не отправилось EMAIL с шаблоном " + text,
                                              () => Assert.IsTrue(res.Count > 0));
            }

        }
    }
}
