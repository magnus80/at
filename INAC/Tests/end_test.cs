using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("Service")]
    public class z__end_test
    {
        [Test]
        public void step_01()
        {
            Browser.Quit();
            AT.Service.Notifier.FormXmlReport();
          //  AT.Service.Notifier.SendNotif();
        }
    }
}
