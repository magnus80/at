//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_AVPC")]
    public class test_237999 : TestBase
    {
        private string login = "";
        
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            login = Helpers.Abonents.Actions.Creation.Create(10000);


            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.ConnectPC();

            var query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'PC' ";

            var dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("ошибка подключения род. контроля, логин: " + login,
                      () => Assert.IsFalse(dbRes.Count == 0));

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.SelectDays();

            query = @"
            SELECT s_mon, s_wed 
            FROM   inac.avpc_schedule 
            WHERE  s_avpc_id = (SELECT avpc_id 
                                FROM   inac.avpc_subscribers 
                                WHERE  avpc_login = '" + login + "')";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion(
                "некорректное отображение в БД расписания род. контроля, понедельник",
                () => Assert.IsTrue(list[0].Equals("262144")));
            Assertion("некорректное отображение в БД расписания род. контроля, среда",
                      () => Assert.IsTrue(list[1].Equals("262144")));

        }
    }
}
