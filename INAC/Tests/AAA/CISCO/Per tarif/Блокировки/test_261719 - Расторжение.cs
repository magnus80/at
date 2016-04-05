using AT;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("AAA"), Category("CISCO"), Category("PER_TARIF")]
    public class test_261719 : TestBase
    {
        private string login, password, session_id, service;
        private const string block_type = "5";

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Queries.GetAbonentForAAA(block_type);
            password = Helpers.Abonents.Info.GetPassword(login);
            service = Helpers.Abonents.Info.GetServices(login)[0];
        }

        [Test]
        public void step_02()
        {
            var result = Helpers.AAA.Actions.Connection.ConnectToCisco(login, password);

            Assertion("Ошибка подключения к CISCO (connect), логин:" + login, () => Assert.IsTrue(result));
        }

        [Test]
        public void step_03()
        {
            var result = Helpers.AAA.Actions.Bras.ExecuteCmdCisco(login);

            var ctn = Helpers.AAA.Actions.GetParam(result, "CTN");
            var svc = Helpers.AAA.Actions.GetParam(result, "SVC");
            session_id = Helpers.AAA.Actions.GetParam(result, "SESSION_ID");
            var block_type_act = Helpers.AAA.Actions.GetParam(result, "BLOCK_TYPE");

            Assertion(
                "Некорректное значение параметра SVC на брасе (" + Environment.BrasCisco + ") [ожидаемо: " + service +
                "; актуально: " + svc +
                "], логин:" + login, () => Assert.AreEqual(service, svc));

            Assertion(
                "Некорректное значение параметра CTN на брасе (" + Environment.BrasCisco + ") [ожидаемо: " + login +
                "; актуально: " + ctn +
                "], логин:" + login, () => Assert.AreEqual(login, ctn));

            Assertion(
                "Некорректное значение параметра BLOCK_TYPE на брасе (" + Environment.BrasCisco + ") [ожидаемо: " +
                block_type + "; актуально: " + block_type_act +
                "], логин:" + login, () => Assert.AreEqual(block_type, block_type_act));
        }

        [Test]
        public void step_04()
        {
            var radius_res = Helpers.AAA.Actions.Radius.CheckAuthInRadiusLog(login, service, "CISCO", session_id);

            Assertion(
                "Некорректные логи Radius (" + Environment.RadiusIp + "), логин:" + login + "(" + radius_res + ")",
                () => Assert.IsTrue(radius_res.IndexOf(Environment.RadiusOk) > -1));

            var block_type_act = Helpers.AAA.Actions.GetParam(radius_res, "block_type");

            Assertion(
                "Некорректные логи Radius (" + Environment.RadiusIp + ") - блокировка, [ожидаемо: " + block_type +
                "; актуально:" + block_type_act + "], логин:" + login,
                () => Assert.AreEqual(block_type_act, block_type));
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
