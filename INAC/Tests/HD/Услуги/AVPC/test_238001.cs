//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("HD_AVPC")]
    public class test_238001 : TestBase
    {
        private string login = "";

        [Test]
        public void step_01()
        {

           login = Helpers.Abonents.Actions.Creation.Create(10000);

            Pages.HD.Login.LoginAsGod();
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

        }

        [Test]
        public void step_02()
        {
            var query = @"
            SELECT pn_category_id 
            FROM   inac.avpc_property_name 
            WHERE  pn_default = 1 ";

            var val = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Assertion("некорректное значение по умолчанию в категориях род. контроля",
                      () => Assert.IsTrue(Pages.HD.Avpc_statuses.CheckSelectedCat(val)));
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Avpc_statuses.Open("?login=" + login);
            Pages.HD.Avpc_statuses.DeleteAll();

            var query = @"
            SELECT spl_service 
            FROM   inac.services_per_login spl 
                   JOIN inac.services_param 
                     ON spl_service = id_service 
            WHERE  spl_login = '" + login + @"' 
                   AND param_char = 'AV' ";

            var dbRes = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("ошибка отключения род. контроля, логин: " + login,
                      () => Assert.IsTrue(dbRes.Count == 0));

        }
    }
}
