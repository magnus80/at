//автор: NGadiyak
using AT;
using AT.DataBase;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("УКК"), Description("Отображение блокировок")]
    public class test_246605 : TestBase
    {
        private string login;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);
            Check("Не заблокирован");
        }

        [Test]
        public void step_04()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 1
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Финансовая блокировка");
        }

        [Test]
        public void step_05()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 2
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Добровольно заблокирован");
        }

        [Test]
        public void step_06()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 3
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Заблокирован при регистрации");
        }

        [Test]
        public void step_07()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 4
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Административная блокировка");
        }

        [Test]
        public void step_08()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 5
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Расторжение");
        }

        [Test]
        public void step_09()
        {
            var query = @"UPDATE inac.logins
                      SET    l_block_type = 6
                      WHERE  l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Pages.HD.Queues.OpenLogin(login);
            Check("Collection");
        }

        public void Check(string block_text)
        {
            Assertion(
                "Некоррекнтно отображение блокировки, [login: " + login +
                ", ожидаемая блокировка: " + block_text + "]",
                () => Assert.Greater(Browser.Source.IndexOf(block_text), -1));
        }
    }
}
