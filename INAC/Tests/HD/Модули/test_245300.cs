//автор: NGadiyak
using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Остановка БЦ stop_billing.pl")]
    public class test_245300 : TestBase
    {
        private string login;

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
            Pages.HD.Stop_billing.Open("?address_id=" + adr);
            Pages.HD.Stop_billing.StopBilling();
        }

        [Test]
        public void step_04()
        {
            var query = @"SELECT ss_service,
                           ss_start,
                           ss_stop
                     FROM   inac.start_stop
                     WHERE  ss_login = '" + login + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            var ss_service = list[0];
            var ss_start = DateTime.Parse(list[1]).ToShortDateString();
            var ss_stop = DateTime.Parse(list[2]).ToShortDateString();

            var list1 = Helpers.Abonents.Info.GetServices(login);

            Assertion(
                "некорректное значение сервиса (табл. start_stop) "
                , () => Assert.AreEqual(ss_service, list1[0]));

            Assertion(
                "некорректное значение даты начала бц (табл. start_stop) "
                , () => Assert.AreEqual(ss_start, DateTime.Now.ToShortDateString()));

            Assertion(
                "некорректное значение даты окончания бц (табл. start_stop) "
                , () => Assert.AreEqual(ss_stop, DateTime.Now.ToShortDateString()));

        }
    }
}
