using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_262003 : TestBase
    {
        private string login = "";

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Queries.GetOTTLogin();

            Assertion("Выборка ОТТ абонентов, выборка пустая", () => Assert.IsNotNullOrEmpty(login));
        }

        [Test]
        public void step_02()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "LOGIN", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "DOMAIN", "-"));
            var res = Executor.ExecuteProcedure("inac.reactivate_login", Environment.InacDb);

            Assertion("Некорректно отработала процедура inac.reactivate_login (должна быть ошибка), логин: " + login,
                     () => Assert.AreEqual(res, "error"));
        }
    }
}
