//автор: NGadiyak
using System;
using AT;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Оплаты payments.pl")]
    public class test_245140 : TestBase
    {
        private string login;
        private string ticket_id;
        private string pay_sum;

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
            Pages.HD.Queues.OpenLogin(login);

            var adr = AT.Tools.Other.GetParamFromCurrentUrl("address_id");
            Pages.HD.Address.Open("?address_id=" + adr);

            Pages.HD.Address.TQueue = "Другая";
            Pages.HD.Address.TType = "Возврат ош.платежа";
            Pages.HD.Address.NewTicket();

            ticket_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Pages.HD.Comments.Open("?ticket_id=" + ticket_id);
            Pages.HD.Comments.OpenExtendedTab();

            pay_sum = new Random().Next(10000).ToString() + ".00";
            Pages.HD.Comments.BSumm = pay_sum;
            Pages.HD.Comments.ChangeBonus();
        }

        [Test]
        public void step_04()
        {
            var contract = Helpers.Abonents.Info.GetContract(login);

            Pages.HD.Payments.Open("?contract=" + contract);
            var count = Pages.HD.Payments.FindPayment(pay_sum);

            Assertion(
                "Некорректное отображение платежей " + Browser.Url,
                () => Assert.AreEqual(1, count));
        }
    }
}
