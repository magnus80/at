//автор: NGadiyak
using System;
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Абоненты"), Description("Начисление денег из заявки")]
    public class test_244984 : TestBase
    {
        private string login;
        private string ticket_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Queues.OpenLogin(login);

            var adr = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + adr);

            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Возврат ош.платежа";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_04()
        {
            var contract = Helpers.Abonents.Info.GetContract(login);
            var balance = Convert.ToDouble(Helpers.Abonents.Info.GetBalanceByContract(contract));
            double payment = 1000;
            var balance_new = balance + payment;

            Pages.HD.Comments.Open("?ticket_id=" + ticket_id);
            Pages.HD.Comments.OpenExtendedTab();
            Pages.HD.Comments.BSumm = payment.ToString();
            Pages.HD.Comments.ChangeBonus();

            Helpers.Abonents.Actions.Reaccount(contract);

            balance = Convert.ToDouble(Helpers.Abonents.Info.GetBalanceByContract(contract));

            Assertion(
                "некорректное начисление баланса, логин: " +
                login, () => Assert.AreEqual(balance, balance_new));

        }
    }
}
