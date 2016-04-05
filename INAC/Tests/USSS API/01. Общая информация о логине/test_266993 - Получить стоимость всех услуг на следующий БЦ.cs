using AT;
using NUnit.Framework;

namespace INAC.Tests.USSS_API
{
    [TestFixture]
    [Category("USSS"), Category("USSS_InfoService")]
    public class test_266993 : TestBase
    {
        private string login;

        [Test]
        public void step_01()
        {
            login = Helpers.Abonents.Actions.Creation.Create(10000);
        }

        [Test]
        public void step_02()
        {
            var result = Helpers.USSS.InfoService.GetNextBCSum(login);

            Assertion("Ошибка при выполнении метода getNextBCSum (InfoService)", () => Assert.IsTrue(result));
        }

        [Test]
        public void step_03()
        {
            var s_price_sum_bd = float.Parse(Helpers.Abonents.Info.GetServicesPriceSum(login));
            var s_price_sum_soap =
                float.Parse(AT.WebServices.SOAP.SoapExecutor.Results["tns:next_bc_sum"].Replace('.', ','));

            Assertion(
                "userStopLogin (InfoService): некорректный вывод метода getNextBCSum (InfoService) [ожидаемо: " +
                s_price_sum_bd.ToString("0.00") + ", актуально:" +
                s_price_sum_soap.ToString("0.00") + "]; логин: " + login,
                () => Assert.AreEqual(s_price_sum_soap, s_price_sum_bd));
        }
    }
}
