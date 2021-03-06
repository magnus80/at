﻿using AT;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_258625 : TestBase
    {
        private string login, adress, ticket, contract;
        private float balance, price_sum;

        [Test]
        public void step_01()
        {
            var service = Helpers.Services.FTTB.Queries.GetFttbServiceUnlim("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service, 10000);
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
            Pages.HD.Address.CreateTicket("IPTV заявки", "Аренда IPTV. Выдача");
        }

        [Test]
        public void step_04()
        {
            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("Ошибка при создании заявки Аренда IPTV. Выдача", () => Assert.IsNotNullOrEmpty(ticket));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Rentiptv2.Open("?act=1&command=rent&ticket_id=" + ticket);
            var guid = Helpers.Services.IPTV.Queries.GetIptvGuid();
            Pages.HD.Rentiptv2.RenIptv(guid);
        }

        [Test]
        public void step_06()
        {
            var balance_old = Helpers.Abonents.Info.GetBalanceByLogin(login);
            Pages.HD.Address.Open("?address_id=" + adress);
            Pages.HD.Address.CreateTicket("Другая", "Добровольная блокировка");

            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Pages.HD.Comments.Open("?ticket_id=" + ticket);
            Pages.HD.Comments.OpenExtendedTab();
            Pages.HD.Comments.BlockButtonClick();
            Pages.HD.Address.Open("?address_id=" + adress);

            contract = Helpers.Abonents.Info.GetContract(login);
            Helpers.Abonents.Actions.Reaccount(contract);

            var balance_new = Helpers.Abonents.Info.GetBalanceByLogin(login);

            Assertion("Ошибка при работе добровольной блокировки (у абонента изменился баланс",
                      () => Assert.AreEqual(balance_new, balance_old));

            balance = float.Parse(balance_new);
        }

        [Test]
        public void step_07()
        {
            price_sum = float.Parse(Helpers.Abonents.Info.GetServicesPriceSum(login));
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(((-1)*balance + price_sum - 10).ToString(), login);

            var balance_new = float.Parse(Helpers.Abonents.Info.GetBalanceByLogin(login));

            Assertion("Ошибка при начислении денег", () => Assert.AreEqual(balance_new, price_sum - 10));
        }

        [Test]
        public void step_08()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -30);
            Helpers.Abonents.Actions.Reaccount(contract);
        }

        [Test]
        public void step_09()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion("Некорректный тип блокировки (абонент должен быть в добровольной)",
                      () => Assert.AreEqual("2", block_type));

            Pages.HD.Address.Open("?address_id=" + adress);
            Pages.HD.Address.Unblock();
        }

        [Test]
        public void step_10()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);
            Assertion("Некорректный тип блокировки (абонент должен быть в активен)",
                      () => Assert.AreEqual("0", block_type));

            Helpers.Abonents.Actions.Payments.AddPaymentToLogin("11", login);
            Helpers.Abonents.Actions.Reaccount(contract);

            block_type = Helpers.Abonents.Info.GetBlockType(login);
            Assertion("Некорректный тип блокировки (абонент должен быть не заблокирован)",
                      () => Assert.AreEqual("0", block_type));
        }
    }
}
