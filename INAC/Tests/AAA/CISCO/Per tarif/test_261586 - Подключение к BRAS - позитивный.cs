using AT;
using NUnit.Framework;

namespace INAC.Tests.AAA
{
    [TestFixture]
    [Category("AAA"), Category("CISCO"), Category("PER_TARIF")]
    public class test_261586 : TestBase
    {
        private string login, service, speed_u_bd, speed_d_bd, password, session_id;
        private const string block_type = "0";


        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Queries.GetAbonentForAAA(block_type);
            password = Helpers.Abonents.Info.GetPassword(login);
            service = Helpers.Abonents.Info.GetServices(login)[0];
            speed_u_bd = Helpers.Services.FTTB.Queries.GetFromVpdn(service, "v_speed_in");
            speed_d_bd = Helpers.Services.FTTB.Queries.GetFromVpdn(service, "v_speed_out");
        }

        [Test]
        public void step_02()
        {
            var result = Helpers.AAA.Actions.Connection.ConnectToCisco(login, password);

            Assertion("Ошибка подключения к CISCO (connect), логин:" + login, () => Assert.IsTrue(result));

            result = Helpers.AAA.Actions.Tracert.TracertCisco();

            Assertion("Ошибка трассировки CISCO, логин:" + login, () => Assert.IsTrue(result));
        }

        [Test]
        public void step_03()
        {
            var result = Helpers.AAA.Actions.Bras.ExecuteCmdCisco(login);

            var speed_u = Helpers.AAA.Actions.GetParam(result, "SPEED_U");
            var speed_d = Helpers.AAA.Actions.GetParam(result, "SPEED_D");
            var ctn = Helpers.AAA.Actions.GetParam(result, "CTN");
            var svc = Helpers.AAA.Actions.GetParam(result, "SVC");
            session_id = Helpers.AAA.Actions.GetParam(result, "SESSION_ID");

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
            var res = Helpers.AAA.Actions.Radius.CheckAuthInRadiusLog(login, service, "CISCO", session_id);

            Assertion(
                "Некорректные логи Radius (" + Environment.RadiusIp + "), логин:" + login + "(" + res + ")",
                () => Assert.IsTrue(res.Equals(Environment.RadiusOk)));
        }

        [Test]
        public void step_05()
        {
            var res = Helpers.AAA.Queries.IsActiveSession(login);

            Assertion("Нет активной сессии в BD Storm, логин: " + login, () => Assert.IsTrue(res));
        }

        [Test]
        public void step_06()
        {
            Helpers.AAA.Actions.Connection.Disconnect(Environment.ConnectionCisco);
        }

        [Test]
        public void step_07()
        {
            Helpers.AAA.Actions.WaitForCloseSession();
            var res = Helpers.AAA.Queries.IsActiveSession(login);

            Assertion("Осталась активная сессия после дисконнекта в BD Storm, логин: " + login,
                      () => Assert.IsFalse(res));
        }
    }
}
