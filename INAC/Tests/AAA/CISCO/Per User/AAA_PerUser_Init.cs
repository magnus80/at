using AT;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("PER_USER")]
    public class a__AAA_PerUser_Init : TestBase
    {
        [Test]
        public void SetBrasType()
        {
            Helpers.AAA.Actions.Bras.SetBrasPerUser(Environment.BrasCisco);
        }
    }
}
