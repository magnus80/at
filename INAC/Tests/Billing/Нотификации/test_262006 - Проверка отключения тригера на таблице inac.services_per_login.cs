using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_262006 : TestBase
    {
        [Test]
        public void step_01()
        {
            var query = @"SELECT CASE status 
                                 WHEN 'ENABLED' THEN 'FAIL' 
                                 ELSE 'PASS' 
                               END case 
                        FROM   all_triggers 
                        WHERE  table_name = 'SERVICES_PER_LOGIN' 
                               AND table_owner = 'INAC' 
                               AND trigger_name = 'TRIG_SPL_EVENTS_NOTIFY'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb)[0,0];

            Assertion("Не отключен триггер нотификаций в таблице services_per_login ", () => Assert.AreEqual("PASS", res));
        }
    }
}
