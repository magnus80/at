using AT;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_258623 : TestBase
    {
        private string login, unlim_serv, unlim_price, limit_serv, limit_price;

        [Test]
        public void step_01()
        {
            unlim_serv = Helpers.Services.FTTB.Queries.GetFttbServiceUnlim("12042");
            unlim_price = Helpers.Services.Queries.GetPrice(unlim_serv);
            limit_serv = Helpers.Services.FTTB.Queries.GetFttbServiceLimit("12042");
            limit_price = Helpers.Services.Queries.GetPrice(limit_serv);

            login = Helpers.Abonents.Actions.Creation.Create(unlim_serv);
        }

        [Test]
        public void step_02()
        {
            float payment = float.Parse(unlim_price) + float.Parse(limit_price)/2 + 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(payment.ToString(), login);
        }

        [Test]
        public void step_03()
        {
            var balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            float balance_f_act = float.Parse(balance);
            float balance_f_exp = float.Parse(limit_price) / 2 + 1;

            Assertion("Деньги за сервис " + unlim_serv + " не ушли в резерв, логин: " + login,
                      () => Assert.AreEqual(balance_f_exp, balance_f_act));
        }

        [Test]
        public void step_04()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -15);
            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_05()
        {
            Helpers.Abonents.Actions.ChangeServ(login, unlim_serv, limit_serv);

            float payment = float.Parse(unlim_price)/2*(-1);
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(payment.ToString(), login);

            var balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            float balance_act = float.Parse(balance);
            float balance_exp = 1;

            Assertion("Некорректный пересчет денег абонента при смене сервиса, логин: " + login,
                      () => Assert.AreEqual(balance_exp, balance_act));
        }

        [Test]
        public void step_06()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -15);
            Helpers.Abonents.Actions.ReaccLogin(login);

            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion("Некорректный тип блокировки (абонент должен быть в финансовой)",
                      () => Assert.AreEqual("1", block_type));
        }

        [Test]
        public void step_07()
        {
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(limit_price, login);
        }

        [Test]
        public void step_08()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion("Некорректный тип блокировки (абонент должен быть активен)",
                      () => Assert.AreEqual("0", block_type));

            var balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            float balance_act = float.Parse(balance);
            float balance_exp = 1;

            Assertion("Некорректный пересчет денег абонента при старте нового БЦ, логин: " + login,
                      () => Assert.AreEqual(balance_exp, balance_act));
        }
    }
}
