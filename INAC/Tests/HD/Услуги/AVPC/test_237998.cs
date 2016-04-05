//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_AVPC")]
    public class test_237998 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            var login = Helpers.Abonents.Actions.Creation.Create(10000);

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.ConnectPC();

            var query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'PC'";

            var dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion( "ошибка подключения род. контроля, логин: " + login,
                                          () => Assert.IsTrue(dbRes.Count > 0));

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.ConnectAV();

            query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'AV' ";

            dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion( "ошибка подключения сет. антивируса, логин:" + login,
                                          () => Assert.IsFalse(dbRes.Count == 0));

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.DeleteAV();

            query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'AV' ";

            dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion( "ошибка отключения сет. антивируса, логин: " + login,
                                          () => Assert.IsTrue(dbRes.Count == 0));

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.DeleteAll();

            query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'AV' ";

            dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion( "ошибка отключения род. контроля, логин: " + login,
                                          () => Assert.IsTrue(dbRes.Count == 0));

        }
    }
}
