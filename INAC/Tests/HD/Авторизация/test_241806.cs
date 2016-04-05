//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Авторизация")]
    public class test_241806: TestBase
    {
        [Test]
        public void step_01()
        {
            IsContinueOnStepFail = true;

            var list = Helpers.HD_Users.Queries.GetHdUser(1);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_02()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(2);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_03()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(3);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_04()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(4);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_05()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(5);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_06()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(6);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_07()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(7);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_08()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(8);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion("Ошибка авторизации, логин: " + list[0],
                                          () => Assert.IsTrue(res));
        }

        [Test]
        public void step_09()
        {
            var list = Helpers.HD_Users.Queries.GetHdUser(0);

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            var res = Pages.HD.Login.Submit();

            Assertion( "Удалось авторизоваться под удаленным пользователем, логин: " + list[0],
                                          () => Assert.IsFalse(res));
        }
    }
}
