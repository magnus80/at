using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_262005 : TestBase
    {
        private string text1, text2, phone, date1, date2;

        [Test]
        public void step_01()
        {
            phone = "90101" + new Random().Next(10000, 99999).ToString();
            text1 = "AT_Test_Message_" + new Random().Next(10000, 99999).ToString();

            var query = @"select sysdate from dual";
            date1 = Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        [Test]
        public void step_02()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "i_phone_number", phone));
            Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "i_message", text1));
            Executor.ProcedureParamList.Add(new ProcedureParam("DateTime", "i_dispatch_datetime", date1));
            Executor.ExecuteProcedure("beeline.Smsgw_send_sms_etl", Environment.InacDb);
        }

        [Test]
        public void step_03()
        {
            var query = @"SELECT msgbody, 
                                   svp 
                            FROM   beeline.tbl_submit 
                            WHERE  da_value = '7" + phone + "' ORDER BY svp desc";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("SMS не добавилось в очередь отправки (номер: " + phone + "), ошибка процедуры beeline.Smsgw_send_sms_etl",
                      () => Assert.GreaterOrEqual(res.Count, 1));

            text2 = res[0, 0];
            date2 = res[0, 1];

            Assertion(
                "Не соответствует текст SMS [ожидаемо: " + text1 + " ; актуально: " + text2 +
                "], ошибка процедуры beeline.Smsgw_send_sms_etl", () => Assert.AreEqual(text1, text2));

            Assertion(
                "Не соответствует дата отправки SMS [ожидаемо: " + date1 + " ; актуально: " + date2 +
                "], ошибка процедуры beeline.Smsgw_send_sms_etl", () => Assert.AreEqual(date1, date2));
        }
    }
}
