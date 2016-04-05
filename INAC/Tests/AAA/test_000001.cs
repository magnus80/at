using AT;
using NUnit.Framework;

namespace INAC.Tests.AAA
{
    [TestFixture]
    [Category("AAA")]
    public class test_000001 : TestBase
    {
        private string login, service, speed_u_bd, speed_d_bd;
        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Actions.GetAbonentForAAA();
            service = Helpers.Abonents.Info.GetServices(login)[0];
            speed_d_bd = Helpers.Services.FTTB.Queries.GetFromVpdn(service, "v_speed_in");
            speed_u_bd = Helpers.Services.FTTB.Queries.GetFromVpdn(service, "v_speed_out");
        }

        [Test]
        public void step_02()
        {
            var result = Helpers.AAA.Actions.ConnectToCisco(login, "password1");

            Assertion("Ошибка подключения к CISCO, логин:" + login, () => Assert.IsTrue(result));
        }

        [Test]
        public void step_03()
        {
            var result = Helpers.AAA.Actions.ExecuteCmdCisco(login);

            Assert.Fail(result);

            var speed_u = Helpers.AAA.Actions.GetParam(result, "SPEED_U");
            var speed_d = Helpers.AAA.Actions.GetParam(result, "SPEED_D");
            var ctn = Helpers.AAA.Actions.GetParam(result, "CTN");
            var svc = Helpers.AAA.Actions.GetParam(result, "SVC");

            Assertion(
                "Некорректное значение параметра SVC на брасе (" + Environment.BrasCisco + ") [ожидаемо: " + service +
                "; актуально: " + svc +
                "], логин:" + login, () => Assert.AreEqual(service, svc));

            Assertion(
                "Некорректное значение параметра CTN на брасе (" + Environment.BrasCisco + ") [ожидаемо: " + login +
                "; актуально: " + ctn +
                "], логин:" + login, () => Assert.AreEqual(login, ctn));

            Assertion(
                "Некорректное значение параметра SPEED_U на брасе (" + Environment.BrasCisco + ") [ожидаемо: " +
                speed_u_bd + "000; актуально: " + speed_u +
                "], логин:" + login, () => Assert.AreEqual(speed_u_bd + "000", speed_u));

            Assertion(
                "Некорректное значение параметра SPEED_U на брасе (" + Environment.BrasCisco + ") [ожидаемо: " +
                speed_d_bd + "000; актуально: " + speed_d +
                "], логин:" + login, () => Assert.AreEqual(speed_d_bd + "000", speed_d));
        }

        [Test]
        public void step_04()
        {
            Helpers.AAA.Actions.Disconnect(Environment.ConnectionCisco);
        }
    }
}
