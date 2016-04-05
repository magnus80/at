using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Услуги"), Category("IPTV"), Description("Проверка наличия новой заявки в HD")]
    public class test_251042 : TestBase
    {
        [Test]
        public void step_01()
        {
            var query = @"SELECT q_queue AS queue, 
                           tt_type AS name 
                    FROM   helpdesk.ticket_types 
                           JOIN helpdesk.queues 
                             ON tt_queue = q_id 
                    WHERE  tt_queue = 10";
            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(1);


            Assertion(
                "в БД отсутствует заявка на подключение архивной аренды IPTV ",
                () => Assert.IsNotNullOrEmpty(list.Find(x => x.Equals("Подключение архивного сервиса аренды IPTV"))));
        }
    }
}
