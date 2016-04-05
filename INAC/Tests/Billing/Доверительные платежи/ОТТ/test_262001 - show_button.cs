using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_262001 : TestBase
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
            var res = Executor.ExecuteProcedure("inac.promised_payments_pkg.show_button_f", Environment.InacDb);

            Assertion("Некорректно отработала процедура inac.promised_payments_pkg.show_button_f, логин: " + login,
                      () => Assert.AreEqual(res, "0"));
        }

        [Test]
        public void step_03()
        {
            var query = @"SELECT l_login 
                        FROM   (SELECT l_login 
                                FROM   inac.logins 
                                       join inac.contract_info 
                                         ON ci_id = l_contract 
                                WHERE  l_login LIKE ( '089%' ) 
                                       AND ci_jur_depend <> 13 
                                       AND l_block_type = 1 
                                       AND l_login NOT IN (SELECT pp_login 
                                                           FROM   inac.promised_payments) 
                                ORDER  BY dbms_random.value)";

            login = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            Assertion("Выборка ОТТ абонентов без доверительного платежа, выборка пустая", () => Assert.IsNotNullOrEmpty(login));
        }

        [Test]
        public void step_04()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "IN_LOGIN", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number"));
            var res = Executor.ExecuteProcedure("inac.promised_payments_pkg.show_button_f", Environment.InacDb);

            Assertion("Некорректно отработала процедура inac.promised_payments_pkg.show_button_f, логин: " + login,
                      () => Assert.AreEqual(res, "1"));
        }
    }
}
