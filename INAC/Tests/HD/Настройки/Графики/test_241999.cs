using AT;

using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Settings")]
    public class test_241999 :TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            Pages.HD.Shedule_nconnect.Open();
        }
    }
}
