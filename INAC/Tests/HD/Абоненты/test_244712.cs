//автор: NGadiyak
using AT;
using AT.WebDriver;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Абоненты"), Description("Реактивация_доступы")]
    public class test_244712 : TestBase
    {
        private string login, ticket_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            login = Helpers.Abonents.Actions.Creation.Create();

            Pages.HD.Queues.OpenLogin(login);
            var address_id = AT.Tools.Other.GetParamFromCurrentUrl("address_id");

            Pages.HD.Address.Open("?address_id=" + address_id);
            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Реактивация";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_02()
        {
            IsContinueOnStepFail = true;

            var list = Helpers.HD_Users.Queries.GetHdUser(1);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], true);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_03()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(2);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_04()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(3);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_05()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(4);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_06()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(5);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_07()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(6);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_08()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(7);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], false);

            Pages.HD.Logout.Logout();
        }

        [Test]
        public void step_09()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(8);

            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();

            Check(list[0], true);

            Pages.HD.Logout.Logout();
        }

        private void Check(string login, bool flag)
        {
            Pages.HD.Comments.Open("?ticket_id=" + ticket_id);

            var src = Browser.Source;
            var index = src.IndexOf("Доступ к данному модулю ограничен!Недостаточно прав");

            if (index == -1 && flag == false)
            {
                Pages.HD.Comments.Open("?ticket_id=" + ticket_id);
                Pages.HD.Comments.OpenExtendedTab();
                Assertion(
                    "Кнопка доступна, пользователь " + login,
                    () => Assert.IsFalse(Pages.HD.Comments.ReactAbonBtnExists));
                return;
            }
            if (index == -1 && flag)
            {
                Pages.HD.Comments.Open("?ticket_id=" + ticket_id);
                Pages.HD.Comments.OpenExtendedTab();
                Assertion(
                    "Кнопка недоступна, пользователь " + login,
                    () => Assert.IsTrue(Pages.HD.Comments.ReactAbonBtnExists));
                return;
            }
            if (index != -1 && flag == false)
            {
                return;
            }
            if (index != -1 && flag)
            {
                Assertion(
                    "Кнопка недоступна, пользователь " + login,
                    () => Assert.IsTrue(false));
            }
        }
    }
}
