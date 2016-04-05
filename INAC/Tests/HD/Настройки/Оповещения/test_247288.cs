//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Оповещения"), Description("Удаление")]
    public class test_247288 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Notification_settings.Open();
            var id = Pages.HD.Notification_settings.DeleteNotif("1");

            check(id);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Notification_settings.Open();
            var id = Pages.HD.Notification_settings.DeleteNotif("2");

            check(id);
        }


        [Test]
        public void step_04()
        {
            Pages.HD.Notification_settings.Open();
            var id = Pages.HD.Notification_settings.DeleteNotif("3");

            check(id);
        }


        [Test]
        public void step_05()
        {
            Pages.HD.Notification_settings.Open();
            var id = Pages.HD.Notification_settings.DeleteNotif("4");

            check(id);
        }



        private void check(string id)
        {
            var query = @"SELECT *
                      FROM   helpdesk.notification_message
                      WHERE  m_id = " + id;

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "ошибка удаления шаблона оповещения, [ид: " + id +
                "] ",
                () => Assert.IsTrue(res.Count == 0));

        }
    }
}
