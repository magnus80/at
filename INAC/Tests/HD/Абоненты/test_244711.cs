//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Абоненты"), Description("Реактивация_функционал")]
    public class test_244711 : TestBase
    {
        private string oper_god;
        private string login, contract;
        private readonly string react_summ = "1";

        [Test]
        public void step_01()
        {
            oper_god = Helpers.HD_Users.Queries.GetHdGoUserLogin();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT l_login 
                          FROM   inac.logins ll 
                          WHERE  ll.l_login LIKE ('089%')
                                              AND LL.l_login NOT IN (SELECT L.l_login 
                                              FROM   inac.logins l 
                                              WHERE  L.l_block_type IN ( 1, 3 ) 
                                                     AND L.l_contract IN (SELECT c.c_id 
                                                                          FROM   inac.contracts c 
                                                                          WHERE  ( 
                                                                         C.c_payed - C.c_uses ) 
                                                                                 >=- 500) 
                                                     AND NOT EXISTS (SELECT NULL 
                                                                     FROM   inac.services_per_login  sp, 
                                                                            inac.services s 
                                                                     WHERE  sp.spl_login = l.l_login 
                                                                            AND SP.spl_service = 
                                                                                s.s_id 
                                                                            AND S.s_nonstop = 1))";

            var login = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            var contract = Helpers.Abonents.Info.GetContract(login);

            Helpers.Abonents.Actions.Reaccount(contract);

            Helpers.Abonents.Actions.Reactivate(login, "30", oper_god);
        }

        [Test]
        public void step_03()
        {
            var query = @"SELECT SP.spl_login 
                      FROM   inac.services_per_login sp 
                      WHERE  SP.spl_service IN (SELECT S.s_id 
                                              FROM   inac.services s 
                                              WHERE  S.s_nonstop = 1) 
                           AND spl_login LIKE ( '089%' ) 
                      ORDER  BY spl_login DESC";

            var login = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            Helpers.Abonents.Actions.Reactivate(login, "30", oper_god);
        }

        [Test]
        public void step_04()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
            contract = Helpers.Abonents.Info.GetContract(login);
            var balance = Helpers.Abonents.Info.GetBalanceByContract(contract);
            float new_balance;
            float.TryParse(balance, out new_balance);
            new_balance = (new_balance + 1)*(-1);

            Helpers.Abonents.Actions.Payments.AddPaymentToContract(new_balance.ToString(), contract);
            Helpers.Abonents.Actions.ShiftBC(login, -61);
            Helpers.Abonents.Actions.Reaccount(contract);
            Helpers.Abonents.Actions.Reactivate(login, react_summ, oper_god);
        }

        [Test]
        public void step_05()
        {
            var balance = Helpers.Abonents.Info.GetBalanceByContract(contract);
            var pp_summ = Helpers.Abonents.Info.GetPromisedPaymentSum(login);
            var serv_price = Helpers.Abonents.Info.GetServicesPriceSum(login);

            float balance_f, pp_summ_f, serv_price_f, react_summ_f;

            float.TryParse(balance, out balance_f);
            float.TryParse(pp_summ, out pp_summ_f);
            float.TryParse(serv_price, out serv_price_f);
            float.TryParse(react_summ, out react_summ_f);

            float result = react_summ_f + pp_summ_f - (serv_price_f + balance_f);

            Assertion(
                "некорректная реактивация (неверная сумма доверительного платежа) [сумма: " + result +
                ", ожидается 0], логин: " +
                login, () => Assert.AreEqual(result, 0));

        }
    }
}
