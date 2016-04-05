//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("УКК"), Description("Отправка СМС_отграничение")]
    public class test_246604 : TestBase
    {
        private string login, text_sms, address_id;

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
            address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
        }

        [Test]
        public void step_03()
        {
            text_sms = "autotest_send sms_" + login + "_" + DateTime.Now.ToShortDateString() + "_";
            for (int i = 0; i < 15; i++)
            {
                Pages.HD.Address.Open("?address_id=" + address_id);
                Pages.HD.Address.SendSms(text_sms + i);
            }

            Executor.ExecuteProcedure("inac.queue_proc.sms_processor", Environment.InacDb);
        }

        [Test]
        public void step_04()
        {
            var query = @"SELECT 1
                      FROM   beeline.tbl_submit
                      WHERE  msgbody like ('" + text_sms + "%')";

            var result = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "ошибка отправки sms, [login: + " + login + "]",
                () => Assert.Less(result.Count, 11));

        }
    }
}
