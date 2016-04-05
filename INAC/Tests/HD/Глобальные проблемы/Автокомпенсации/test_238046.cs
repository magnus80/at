//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_Автокомпенсации по ГП"), Category("time_limit")]
    public class test_238046 : TestBase
    {
        private string sms_text = string.Empty;
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
            Pages.HD.New_global.NewGP_connect();

            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("ошибка при создании неаварийной ГП", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Notification_settings.Open();
            sms_text = Pages.HD.Notification_settings.ChangeNewSMSTimeTemp();
        }

        [Test]
        public void step_04()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            Pages.HD.Queues.Open();
            Pages.HD.Queues.OpenLogin(login);
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Возврат ош.платежа";

            Pages.HD.Address.NewTicket();

            t_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("ошибка при создании заявки, абонент:" + login, () => Assert.IsNotEmpty(t_id));

            Pages.HD.Comments.Open("?ticket_id=" + t_id);
            Pages.HD.Comments.LinkToGp(gp_id);

            var t_global = Helpers.Query.GetFieldsOfTable("t_global", "helpdesk.tickets", "t_id=" + t_id)[0];
            Assertion("ошибка привязки тикета к ГП (тикет: " + t_id + ", ГП: " + gp_id + ")",
                      () => Assert.AreEqual(t_global, gp_id));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetResolveDate("2018");

        }

        [Test]
        public void step_06()
        {
            Executor.ExecuteProcedure("inac.helpdesk_notification.notification_job", Environment.InacDb);
            Helpers.Queues.Actions.RunSMSProcessor();

            string phone =
                Helpers.Query.GetFieldsOfTable("c_sms_warnto", "inac.logins join inac.contracts on l_contract = c_id ",
                                               "l_login='" + login + "'")[0];


            var query = @"
            SELECT msgbody 
            FROM   beeline.tbl_submit 
            WHERE  da_value = '7" + phone + @"' 
            ORDER  BY svp DESC ";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("не отправилось SMS о продлении срока ГП на номер: " + phone + " (абонент: " + login + ")",
                      () => Assert.IsTrue(res.Count > 0));

            var text = res[0, 0];

            Assertion("текст смс отличается от текста шаблона. ожидаемо: " + sms_text + "; актуально: " + text,
                      () => Assert.AreEqual(sms_text, text));
        }
    }
}
