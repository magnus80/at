using AT;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("PER_TARIF")]
    public class a__AAA_PerTarif_Init : TestBase
    {
        [Test]
        public void SetBrasType()
        {
            Helpers.AAA.Actions.Bras.SetBrasPerTarif(Environment.BrasCisco);
        }
    }
}
