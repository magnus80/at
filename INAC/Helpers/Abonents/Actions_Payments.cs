using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static class Payments
        {
            public static void AddBonusToLogin(string summ, string login)
            {
                var contract = Info.GetContract(login);

                var query = @"INSERT INTO inac.bonus_payments (bp_crt, bp_bonus_sum, bp_cdate, bp_mdate, bp_last_act_program_id, bp_last_act_comment)
                          VALUES (" + contract + ", " + summ +
                            @", sysdate, sysdate, (SELECT * FROM (
                          SELECT bp_id
                           FROM inac.bonus_programs
                           WHERE bp_public = 1 ORDER BY dbms_random.value) WHERE rownum = 1), 'at_bonus')";

                Executor.ExecuteUnSelect(query, Environment.InacDb);

                Reaccount(contract);
            }

            public static void AddPaymentToLogin(string summ, string login)
            {
                var contract = Info.GetContract(login);

                AddPaymentToContract(summ, contract);
            }

            public static void AddPaymentToContract(string summ, string contract)
            {
                summ = summ.Replace(',', '.');
                var query = @"INSERT INTO inac.payments
                                      (p_contract,
                                       p_summ,
                                       p_date,
                                       p_date_upd,
                                       p_discount,
                                       p_paytrust,
                                       p_paytype,
                                       p_currency)
                          VALUES      (" + contract + @",
                                       " +
                            summ + @",
                                       sysdate,
                                       sysdate,
                                       " + summ + @",
                                       3,
                                       7,
                                       2)";

                Executor.ExecuteUnSelect(query, Environment.InacDb);

                Reaccount(contract);
            }
        }
    }
}