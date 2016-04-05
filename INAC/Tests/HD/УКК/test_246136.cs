//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("УКК"), Category("time_limit"), Description("Отправка СМС")]
    public class test_246136 : TestBase
    {
        private string login, text_sms;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            Pages.HD.Queues.OpenLogin(login);
        }

        [Test]
        public void step_03()
        {
            text_sms = "autotest_send sms_" + login + "_" + DateTime.Now.ToShortDateString();
            Pages.HD.Address.SendSms(text_sms);

            Executor.ExecuteProcedure("inac.queue_proc.sms_processor", Environment.InacDb);
        }

        [Test]
        public void step_04()
        {

            var query = @"SELECT 1
                      FROM   beeline.tbl_submit
                      WHERE  msgbody = '" + text_sms + "'";

            var result = Executor.ExecuteSelect(query, Environment.InacDb);

            if (Convert.ToInt32(DateTime.Now.ToString("HH tt")) <= 20 &&
                Convert.ToInt32(DateTime.Now.ToString("HH tt")) >= 10)
            {
                Assertion(
                    "ошибка отправки sms, [login: " + login + "]",
                    () => Assert.Greater(result.Count, 0));
            }
        }
    }
}
