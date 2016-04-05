using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_262002 : TestBase
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
            Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "IN_LOGIN", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number"));
            var res = Executor.ExecuteProcedure("inac.promised_payments_pkg.button_pressed_f", Environment.InacDb);

            Assertion("Некорректно отработала процедура inac.promised_payments_pkg.button_pressed_f, логин: " + login,
                      () => Assert.AreEqual(res, "-1"));
        }
    }
}
