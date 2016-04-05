using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_261933: TestBase
    {
        [Test]
        public void step_01()
        {
            var query = @"SELECT DISTINCT login 
                          FROM   inac.auto_cancel_log 
                          WHERE  error = 'SUCCESSFULL' 
                          MINUS 
                          SELECT DISTINCT l_login 
                          FROM   helpdesk.tickets 
                               join inac.contracts 
                                 ON c_address0 = t_address 
                               join inac.logins 
                                 ON l_contract = c_id 
                          WHERE  t_type = 217 ";

            var res = Executor.ExecuteSelect(query, Environment.InacDb).Count;

            Assertion("Выборка не пустая, необходимо вручную разобрать коды ошибок", () => Assert.AreEqual(0, res));
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT DISTINCT login 
                        FROM   inac.auto_cancel_log 
                        WHERE  error <> 'SUCCESSFULL' 
                        INTERSECT 
                        SELECT DISTINCT l_login 
                        FROM   helpdesk.tickets 
                               join inac.contracts 
                                 ON c_address0 = t_address 
                               join inac.logins 
                                 ON l_contract = c_id 
                        WHERE  t_type = 217 ";

            var res = Executor.ExecuteSelect(query, Environment.InacDb).Count;

            Assertion("Выборка не пустая, необходимо вручную разобрать коды ошибок", () => Assert.AreEqual(0, res));
        }

        [Test]
        public void step_03()
        {
            var query = @"SELECT DISTINCT * 
                            FROM   helpdesk.tickets 
                                   join helpdesk.ticket_statuses 
                                     ON ( t_status = ts_id 
                                          AND ts_id <> 81 ) 
                            WHERE  t_type = 217 ";

            var res = Executor.ExecuteSelect(query, Environment.InacDb).Count;

            Assertion("Выборка не пустая, необходимо вручную разобрать коды ошибок", () => Assert.AreEqual(0, res));
        }
    }
}
