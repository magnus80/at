using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Promised payments")]
    public class test_259148: TestBase
    {
        private string login, ticket, pp_sum, address;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
        }

        [Test]
        public void step_03()
        {
            address = Helpers.Abonents.Info.GetAdressByLogin(login);
            Pages.HD.Address.Open("?address_id=" + address);
            Pages.HD.Address.CreateTicket("Другая", "Доверительный платеж");

            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("Ошибка при создании заявки 'Доверительный платеж'", () => Assert.IsNotNullOrEmpty(ticket));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Comments.Open("?ticket_id=" + ticket);
            Pages.HD.Comments.OpenExtendedTab();
            Pages.HD.Comments.PromissedPay();
        }

        [Test]
        public void step_05()
        {
            var res = Helpers.Query.GetFieldsOfTable("pp_debted, pp_status", "inac.promised_payments",
                                                     "pp_login = '" + login + "'");

            Assertion("Доверительный платеж не начислился, логин: " + login, () => Assert.IsTrue(res.Count > 0));

            Assertion(
                "Некорректный статус доверительного платежа, ожидаемо: 0 или 1, актуально" + res[1] + "; логин: " +
                login, () => Assert.IsTrue(res[1].Equals("0") || res[1].Equals("1")));

            var sum_expected = res[0];
            var sum_actual = Helpers.Abonents.Info.GetServicesPriceSum(login);

            Assertion(
                "Некорректная сумма доверительного платежа, ожидаемо: " + sum_expected + ", актуально: " + sum_actual +
                "; логин: " + login, () => Assert.AreEqual(sum_expected, sum_actual));

            pp_sum = sum_expected;
        }

        [Test]
        public void step_06()
        {
            Pages.HD.Address.Open("?address_id=" + address);
            Pages.HD.Address.StopBc();

            var adr = AT.Tools.Other.GetParamFromCurrentUrl("address_id");

            Assertion(
                "Редирект на некорректную страницу при нажатии кнопки остановки БЦ (неверное значение параметра address_id), логин: " + login,
                () => Assert.AreEqual(adr, address));

            Pages.HD.Stop_billing.Open("?address_id=" + address);
            Pages.HD.Stop_billing.StopBilling();
        }

        [Test]
        public void step_07()
        {
            var pay = float.Parse(pp_sum) + 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(pay.ToString(), login);

            var query = @"UPDATE inac.promised_payments
                          SET    pp_payoff_date = SYSDATE
                          WHERE  pp_login = '" + login + "'";

            Executor.ExecuteUnSelect(query, Environment.InacDb);

            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_08()
        {
            Executor.ExecuteProcedure("inac.promised_payments_pkg.payoff_time_p", Environment.InacDb);

            Pages.HD.Address.Open("?address_id=" + address);

            var res = Helpers.Query.GetFieldsOfTable("pp_payed, pp_debted, pp_status", "inac.promised_payments",
                                                     "pp_login = '" + login + "'");

            Assertion("Сумма, списанная за дов. платеж не соответствует сумме доверительного платежа; логин: " +
                      login, () => Assert.IsTrue(res[0].Equals(pp_sum)));

            Assertion("Доверительный платеж должен быть полность покашен (pp_debted = 0); логин: " +
                      login, () => Assert.IsTrue(res[1].Equals("0")));

            Assertion(
                "Некорректный статус доверительного платежа, ожидаемо: 2, актуально" + res[2] + "; логин: " +
                login, () => Assert.IsTrue(res[2].Equals("2")));
        }
    }
}
