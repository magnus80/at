using AT;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    public class test_258621 : TestBase
    {
        private string login, balance, service1, service2, service1_price, service2_price;

        [Test]
        public void step_01()
        {
            var service = Helpers.Services.FTTB.Queries.GetFttbServiceUnlim("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service, 10000);
        }

        [Test]
        public void step_02()
        {
            service1 = Helpers.Abonents.Info.GetServices(login)[0];
            service1_price = Helpers.Services.Queries.GetPrice(service1);
            balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            Assertion("стоимость сервиса (" + service1 + "не ушла в резерв, логин: " + login,
                      () => Assert.IsTrue(10000 - float.Parse(balance) == float.Parse(service1_price)));
        }

        [Test]
        public void step_03()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -15);
            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_04()
        {
            service2 = Helpers.Query.GetFieldsOfTable("s_id",
                                                      "inac.services join inac.services_param on (s_id = id_service and param_name = 'RESERV_TYPE' and param_number = 1) ",
                                                      "s_f_vpdn = 1 and s_f_public = 1 and s_city = 12042 and s_id <> '" +
                                                      service1 + "' order by dbms_random.value")[0];
            service2_price = Helpers.Services.Queries.GetPrice(service2);
        }

        [Test]
        public void step_05()
        {
            Helpers.Abonents.Actions.ChangeServ(login, service1, service2);
            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_06()
        {
            balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            float expected = 10000 - float.Parse(service2_price)/2 - float.Parse(service1_price)/2;
            float actual = float.Parse(balance);

            Assertion("некорректный рассчет резервирования, логин: " + login, () => Assert.IsTrue(expected == actual));
        }

        [Test]
        public void step_07()
        {
            float pay = float.Parse(balance)*(-1) + float.Parse(service1_price)/2 - 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(pay.ToString(), login);

            balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            Assertion("Ошибка при начислении денег, логин: " + login, () => Assert.AreEqual(balance, pay));
        }

        [Test]
        public void step_08()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -16);
            Helpers.Abonents.Actions.ReaccLogin(login);
        }

        [Test]
        public void step_09()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);
            Assertion("Некорректный тип блокировки (абонент должен быть в финансовой), актуально: " + block_type,
                      () => Assert.AreEqual("1", block_type));
        }
    }
}
