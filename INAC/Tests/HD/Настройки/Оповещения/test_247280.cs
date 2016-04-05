//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Оповещения"), Description("Создание")]
    public class test_247280 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            IsContinueOnStepFail = true;
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Notification_settings.Open();
            var text = Pages.HD.Notification_settings.NewSmsTemp();

            var query = @"SELECT m_type
                      FROM   helpdesk.notification_message
                      WHERE  m_text = '" + text + "'";

            var type = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "некорректный тип SMS оповещения о закрытии ГП, [ожидаемо: 1, факт: " + type +
                "] ",
                () => Assert.AreEqual("1", type));
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Notification_settings.Open();
            var text = Pages.HD.Notification_settings.NewEmailTemp();

            var query = @"SELECT m_type
                      FROM   helpdesk.notification_message
                      WHERE  m_text = '" + text + "'";

            var type = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "некорректный тип EMAIL оповещения о закрытии ГП, [ожидаемо: 2, факт: " + type +
                "] ",
                () => Assert.AreEqual("2", type));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Notification_settings.Open();
            var text = Pages.HD.Notification_settings.NewSMSTimeTemp();

            var query = @"SELECT m_type
                      FROM   helpdesk.notification_message
                      WHERE  m_text = '" + text + "'";

            var type = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "некорректный тип SMS оповещения о продлении сроков ГП, [ожидаемо: 3, факт: " + type +
                "] ",
                () => Assert.AreEqual("3", type));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Notification_settings.Open();
            var text = Pages.HD.Notification_settings.NewGpCrashTemp();

            var query = @"SELECT m_type
                      FROM   helpdesk.notification_message
                      WHERE  m_text = '" + text + "'";

            var type = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "некорректный тип оповещения Аварийной ГП, [ожидаемо: 4, факт: " + type +
                "] ",
                () => Assert.AreEqual("4", type));
        }
    }
}
