using AT;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_261931 : TestBase
    {
        [Test]
        public void step_01()
        {
            var t_type = Helpers.Query.GetFieldsOfTable("tt_type", "helpdesk.ticket_types", "tt_id=217");

            Assertion(
                "Выборка пустая или некорректный тип заявки [Ожидаемо: Расторгнут. Прошло более 180 дней; актуально: " +
                t_type, () => Assert.AreEqual("Расторгнут. Прошло более 180 дней", t_type[0]));
        }

        [Test]
        public void step_02()
        {
            var status = Helpers.Query.GetFieldsOfTable("ssbs_description", "inac.start_stop_block_statuses ", "ssbs_id = 42");

            Assertion(
                "Выборка пустая или некорректный статус [Ожидаемо: Автоматическое расторжение после 180 дней фин. блока; актуально: " +
                status, () => Assert.AreEqual("Автоматическое расторжение после 180 дней фин. блока", status[0]));
        }
    }
}
