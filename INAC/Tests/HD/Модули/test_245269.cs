//автор: NGadiyak
using System;
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Учетные периоды startstop.pl")]
    public class test_245269 : TestBase
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
            Pages.HD.Startstop.Open("?login=" + login);
            Pages.HD.Startstop.StartStopTableInit();

            var list = Helpers.Abonents.Info.GetServices(login);
            var list1 = Helpers.Query.GetFieldsOfTable("s_price, s_name", "inac.services", "s_id='" + list[0] + "'");

            var price_t = Pages.HD.Startstop.StartStopTable[2, 2];
            var date_t = DateTime.Parse(Pages.HD.Startstop.StartStopTable[2, 3]).ToShortDateString();
            var service_t = Pages.HD.Startstop.StartStopTable[2, 5];

            var price = list1[0];

            Assertion(
                "некорректное отображение учетного периода (дата), логин: " +
                login, () => Assert.AreEqual(DateTime.Now.ToShortDateString(), date_t));

            Assertion(
                "некорректное отображение учетного периода (сервис), логин: " +
                login, () => Assert.Greater(service_t.IndexOf(list[0]), -1));

            Assertion(
                "некорректное отображение учетного периода (цена), логин: " +
                login, () => Assert.AreEqual(price, price_t));
        }
    }
}
