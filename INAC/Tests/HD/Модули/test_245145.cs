//автор: NGadiyak
using AT;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Резервирование pay_reserved.pl")]
    public class test_245145 : TestBase
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
            var price = Helpers.Abonents.Info.GetServicesPriceSum(login);
            var id_service = Helpers.Abonents.Info.GetServices(login)[0];
            var Params = Helpers.Services.Queries.GetAllServiceParams(id_service);


            var price_f = float.Parse(price);

            var discount_f = Params["FC_DISCOUNT"] == null
                                 ? 0
                                 : float.Parse(Params["FC_DISCOUNT"].ParamNumber);

            sum = (price_f - discount_f).ToString();

            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(int.Parse(sum).ToString(), login);

            Pages.HD.Pay_reserved.Open("?contract=" + contract);
            Pages.HD.Pay_reserved.PayReservedTableInit();
            var sum_r = Pages.HD.Pay_reserved.Pay_reservedTable[2, 2];

            Assertion(
                "Некорректная сумма резервирования " + Browser.Url,
                () => Assert.AreEqual(sum, sum_r));
        }

        [Test]
        public void step_04()
        {
            Helpers.Abonents.Actions.ShiftBC(login, -30);
            Helpers.Abonents.Actions.Reaccount(contract);

            Pages.HD.Pay_reserved.Open("?contract=" + contract);
            Pages.HD.Pay_reserved.PayReservedTableInit();
            var sum_u = Pages.HD.Pay_reserved.Pay_reservedTable[2, 2];
            var sum_r = Pages.HD.Pay_reserved.Pay_reservedTable[2, 3];
            var contract_r = Pages.HD.Pay_reserved.Pay_reservedTable[2, 1];

            Assertion(
                "Некорректные данные в таблице (поле контракт) " + Browser.Url,
                () => Assert.AreEqual(contract, contract_r));

            Assertion(
                "Некорректная сумма резервирования " + Browser.Url,
                () => Assert.AreEqual(sum, sum_r));

            Assertion(
                "Некорректная сумма резервирования (прошлый месяц)" + Browser.Url,
                () => Assert.AreEqual("0", sum_u));
        }
    }
}
