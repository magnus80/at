using AT;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("HD")]
    public class a__HD_Init: TestBase
    {
        [Test]
        public void step_01()
        {
            Helpers.Methods.HD.ClearGlobalProblems();
            Helpers.Methods.HD.ClearVipClients();

        }
    }
}
