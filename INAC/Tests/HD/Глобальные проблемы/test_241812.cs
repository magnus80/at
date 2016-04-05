//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("1"), Category("Глобальные проблемы")]
    public class test_241812: TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_connect();

            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Assertion( "ошибка при создании ГП подключения", () => Assert.IsNotEmpty(gp_id));
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }

        [Test]
        public void step_03()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();

            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Assertion( "ошибка при создании ГП авария", () => Assert.IsNotEmpty(gp_id));
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }

        [Test]
        public void step_04()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_information();

            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Assertion( "ошибка при создании ГП информация", () => Assert.IsNotEmpty(gp_id));
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }

        [Test]
        public void step_05()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_resourse();

            var gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
            Assertion( "ошибка при создании ГП ресурсы", () => Assert.IsNotEmpty(gp_id));
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpStatus("Закрыта");
        }
    }
}
