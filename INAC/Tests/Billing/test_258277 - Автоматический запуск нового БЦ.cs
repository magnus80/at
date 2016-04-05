using AT;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Main functional")]
    public class test_258277 : TestBase
    {
        private string login, sprice;
        private int iprice = -1;

        [Test]
        public void step_01()
        {
            var service = Helpers.Services.FTTB.Queries.GetFttbServiceUnlim("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service);

            Assertion("Ошибка при создании абонента", () => Assert.IsNotNullOrEmpty(login));
        }

        [Test]
        public void step_02()
        {
            sprice = Helpers.Abonents.Info.GetServicesPriceSum(login);

            Assertion("Ошибка при выборке стоимости подключенных сервисов (абонент: " + login + ")",
                      () => Assert.IsNotNullOrEmpty(sprice));

            int.TryParse(sprice, out iprice);

            Assertion("Некорректная цена сервисов (абонент: " + login + ")", () => Assert.AreNotEqual(iprice, -1));
        }

        [Test]
        public void step_03()
        {
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin((iprice * 2 + 1).ToString(), login);
        }

        [Test]
        public void step_04()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);

            Assertion("Абонент не прошел реактивацию или не начислились деньги (абонент: " + login + ")",
                      () => Assert.AreEqual(block_type, "0"));
        }

        [Test]
        public void step_05()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -30);
            var contract = Helpers.Abonents.Info.GetContract(login);
            Helpers.Abonents.Actions.ReaccAll();

            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void step_06()
        {
            var block_type = Helpers.Abonents.Info.GetBlockType(login);
            var balance = Helpers.Abonents.Info.GetBalanceByLogin(login);

            Assertion("Абонент не прошел реактивацию или некорректно списались деньги (абонент: " + login + ")",
                      () => Assert.AreEqual(block_type, "0"));
            Assertion("некорректные списания денег или не стартовал новый БЦ (абонент: " + login + ")", () => Assert.AreEqual(int.Parse(balance), 1));
        }
        
    }
}
