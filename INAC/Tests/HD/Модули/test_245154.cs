//автор: NGadiyak
using AT;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Доверительные платежи promissed_pay.pl")]
    public class test_245154 : TestBase
    {
        private string login;
        private string contract;
        private string sum;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create();
            contract = Helpers.Abonents.Info.GetContract(login);
        }

        [Test]
        public void step_03()
        {
            var balance = Helpers.Abonents.Info.GetBalanceByContract(contract);
            Helpers.Abonents.Actions.Payments.AddPaymentToContract("-" + balance, contract);

            Helpers.Abonents.Actions.ShiftBC(login, -31);
            Helpers.Abonents.Actions.Reaccount(contract);
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Queues.OpenLogin(login);
            var adr = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + adr);

            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Доверительный платеж";
            Pages.HD.Address.NewTicket();

            var ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Pages.HD.Comments.Open("?ticket_id=" + ticket_id);
            Pages.HD.Comments.OpenExtendedTab();
            Pages.HD.Comments.PromissedPay();
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Promissed_pay.Open("?contract=" + contract);

            sum = Helpers.Abonents.Info.GetServicesPriceSum(login);

            Pages.HD.Promissed_pay.Open("?contract=" + contract);
            Pages.HD.Promissed_pay.Pay_reservedTableInit();
            var sum_pp = Pages.HD.Promissed_pay.Pay_reservedTable[2, 5];
            var debt = Pages.HD.Promissed_pay.Pay_reservedTable[2, 6];
            var payed = Pages.HD.Promissed_pay.Pay_reservedTable[2, 7];

            Assertion(
                "Некорректные данные доверительного платежа (долг) " + Browser.Url + " | долг (табл.) = " + debt +
                ", с чем сравниваем = " + sum,
                () => Assert.AreEqual(debt, sum_pp));

            Assertion(
                "Некорректные данные доверительного платежа (оплачено) " + Browser.Url + " | оплачено (табл.) = " +
                payed,
                () => Assert.AreEqual(payed, "0"));

            Assertion(
                "Некорректная сумма доверительного платежа" + Browser.Url,
                () => Assert.AreEqual(sum_pp, debt));
            }

        [Test]
        public void step_06()
        {
            Helpers.Abonents.Actions.Payments.AddPaymentToContract(sum, contract);
            Helpers.Abonents.Actions.Payments.AddPaymentToContract(sum, contract);
        }

        [Test]
        public void step_07()
        {
            Pages.HD.Promissed_pay.Open("?contract=" + contract);
            Pages.HD.Promissed_pay.Pay_reservedTableInit();
            var debt = Pages.HD.Promissed_pay.Pay_reservedTable[2, 6];
            var payed = Pages.HD.Promissed_pay.Pay_reservedTable[2, 7];

            Assertion(
                "Некорректные данные доверительного платежа (долг) [в таблице: " + debt + ", ожидаемо: 0] " +
                Browser.Url,
                () => Assert.AreEqual(debt, "0"));

            Assertion(
                "Некорректные данные доверительного платежа (оплачено) [в таблице: " + payed + ", ожидаемо: " + sum +
                "] " + Browser.Url,
                () => Assert.AreEqual(payed, sum));
        }
    }
}
