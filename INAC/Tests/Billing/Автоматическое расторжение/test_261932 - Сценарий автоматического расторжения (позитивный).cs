using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_261932 : TestBase
    {
        private string login;

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
        }

        [Test]
        public void step_02()
        {
            var query = @"UPDATE inac.logins 
                          SET    l_block_type = 1, 
                                 l_last_block_date = SYSDATE - 182 
                          WHERE  l_login = '" + login + "'";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }

        [Test]
        public void step_03()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "N", "1"));
            Executor.ExecuteProcedure("inac.automatic_cancel", Environment.InacDb);
        }

        [Test]
        public void step_04()
        {
            var query = @"select error from inac.auto_cancel_log where login = '" + login + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];

            Assertion("Ошибка в поле ERROR [ожидаемо: SUCCESSFULL; актуально: " + res + "], логин:" + login,
                      () => Assert.AreEqual("SUCCESSFULL", res));
        }

        [Test]
        public void step_05()
        {
            var query = @"SELECT tt_type         AS ticket_type, 
                               ts_status       AS ticket_status, 
                               To_char(c_text) AS comment_text 
                        FROM   helpdesk.tickets 
                               join inac.contracts 
                                 ON t_address = c_address0 
                               join inac.logins 
                                 ON c_id = l_contract 
                               join helpdesk.ticket_statuses 
                                 ON t_status = ts_id 
                               join helpdesk.ticket_types 
                                 ON t_type = tt_id 
                               join helpdesk.comments 
                                 ON t_id = c_ticket 
                        WHERE  l_login = '" + login + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion(
                "Ошибка в поле ticket_type [ожидаемо: 'Расторгнут. Прошло более 180 дней'; актуально: " + list[0] +
                "], логин:" + login,
                () => Assert.AreEqual("Расторгнут. Прошло более 180 дней", list[0]));

            Assertion("Ошибка в поле ticket_status [ожидаемо: Закрыто; актуально: " + list[1] + "], логин:" + login,
                      () => Assert.AreEqual("Закрыто", list[1]));

            Assertion(
                "Ошибка в поле comment_text [ожидаемо: 'Абонент переведен из фин. блока в статус Расторжение'; актуально: " +
                list[2] + "], логин:" + login,
                () => Assert.AreEqual("Абонент переведен из фин. блока в статус Расторжение", list[2]));
        }

        [Test]
        public void step_06()
        {
            var serv_count = Helpers.Abonents.Info.GetServices(login).Count;

            Assertion(
                "У абонента остались подключенные сервисы после расторжения, логин" + login,
                () => Assert.AreEqual(serv_count, 0));

            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion(
                "У абонента некорректный тип блокировки после автоматического расторжения [ожидаемо: 5; актуально: " +
                block_type + "], логин: " + login,
                () => Assert.AreEqual(block_type, "5"));
        }
    }
}
