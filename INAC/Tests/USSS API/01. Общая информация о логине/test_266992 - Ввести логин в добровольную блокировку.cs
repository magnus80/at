using AT;
using NUnit.Framework;

namespace INAC.Tests.USSS_API
{
    [TestFixture]
    [Category("USSS"), Category("USSS_InfoService")]
    public class test_266992 : TestBase
    {
        private string login;

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
        }

        [Test]
        public void step_02()
        {
            var result = Helpers.USSS.InfoService.UserStopLogin(login);

            Assertion("Ошибка при выполнении метода userStopLogin (InfoService)", () => Assert.IsTrue(result));
        }

        [Test]
        public void step_03()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion(
                "userStopLogin (InfoService): некорректный тип блокировки [ожидаемо: 2, актуально:" +
                block_type + "]; логин: " + login, () => Assert.AreEqual(block_type, "2"));
        }
    }
}
