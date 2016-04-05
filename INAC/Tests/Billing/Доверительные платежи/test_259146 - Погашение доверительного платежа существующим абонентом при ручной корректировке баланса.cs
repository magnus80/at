using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Promised payments")]
    public class test_259146 : TestBase
    {
        private string login, adress, ticket, pp_sum, price, service, change_serv;
        private float pay, price_f;

        [Test]
        public void step_01()
        {
            service = Helpers.Services.FTTB.Queries.GetFttbServiceForVsu("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service);
            price = Helpers.Abonents.Info.GetServicesPriceSum(login);
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_03()
        {
            adress = Helpers.Abonents.Info.GetAdressByLogin(login);
            Pages.HD.Address.Open("?address_id=" + adress);
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
            pay = float.Parse(pp_sum)/2;

            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(pay.ToString(), login);
            Executor.ExecuteProcedure("inac.promised_payments_pkg.payoff_time_p", Environment.InacDb);

            var res = Helpers.Query.GetFieldsOfTable("pp_debted, pp_payed", "inac.promised_payments",
                                                     "pp_login = '" + login + "'");
            
            var pp_debted = res[0];
            var pp_payed = res[1];

            Assertion(
                "Некорректная сумма доверительного платежа (оплачено), ожидаемо: " + pay + ", актуально: " + pp_payed +
                "; логин: " + login, () => Assert.IsTrue(pp_payed.IndexOf(pay.ToString()) != -1));
            Assertion(
                "Некорректная сумма доверительного платежа (долг), ожидаемо: " + pay + ", актуально: " + pp_debted +
                "; логин: " + login, () => Assert.IsTrue(pp_debted.IndexOf(pay.ToString()) != -1));

        }

        [Test]
        public void step_07()
        {
            change_serv = service;

            while (change_serv.Equals(service))
            {
                change_serv = Helpers.Services.FTTB.Queries.GetFttbServiceForVsu("12042");
            }

            price_f = float.Parse(price) + 1;

            Helpers.Services.FTTB.Actions.SetServicePrice(change_serv, price_f.ToString());

            var result = Helpers.Abonents.Actions.ChangeServ(login, service, change_serv);

            Assertion("Ошибка. Сервис успешно сменен на большую стоимость при не закрытом ДП, логин " + login,
                      () => Assert.IsTrue(result.Equals("error")));
        }

        [Test]
        public void step_08()
        {
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin((pay + price_f).ToString(), login);
            Executor.ExecuteProcedure("inac.promised_payments_pkg.payoff_time_p", Environment.InacDb);

            var result = Helpers.Abonents.Actions.ChangeServ(login, service, change_serv);

            Assertion("Ошибка. Не порлучилось выполнить смену тарифа, логин " + login,
                      () => Assert.IsTrue(result.Equals("success")));
        }
    }
}
