//автор: NGadiyak
using AT;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD")]
    public class test_251200 : TestBase
    {
        [Test]
        public void step_01()
        {

            var list = Helpers.Query.GetFieldsOfTable("u_login, u_password", "helpdesk.users", "u_group = 453");

            Pages.HD.Logout.Logout();
            Pages.HD.Login.Open();
            Pages.HD.Login.Login = list[0];
            Pages.HD.Login.Password = list[1];
            Pages.HD.Login.Submit();
        }
    }
}
