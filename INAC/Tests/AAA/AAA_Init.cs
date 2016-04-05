using System.Collections.Generic;
using AT;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("AAA")]
    public class b__AAA_Init : TestBase
    {
        [Test]
        public void step_01()
        {
            var abons = new List<string>();

            for (var i = 0; i < 14; i++)
            {
                Helpers.AAA.Actions.Prepare.NewAbonentsPackLimit(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackUnlim(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackVSU(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackTB(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackShaped(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackMultiService(ref abons, 1);
            }

            for (int i = 0, block_type = 0; i < abons.Count; i++, block_type++)
            {
                Helpers.Abonents.Actions.SetBlockType(abons[i], block_type);
                if (block_type == 6) block_type = 0;
            }

            Helpers.Abonents.Actions.ReaccAll();
            System.Threading.Thread.Sleep(60 * 15 * 1000); /* ожидание репликации БД 15 минут */
        }
    }
}
