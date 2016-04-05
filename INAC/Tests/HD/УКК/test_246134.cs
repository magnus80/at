//автор: NGadiyak
using AT;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("УКК"), Description("Отображение баланса на УКК")]
    public class test_246134 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            for (int i = 0; i < 5; i++)
            {
                var list = Helpers.Abonents.Queries.GetAnyClient("c_id", "");

                var contract = list[0];
                Helpers.Abonents.Actions.Reaccount(contract);

                list = Helpers.Abonents.Queries.GetAnyClient("c_address0, c_payed - c_uses", "c_id = " + contract);
                var address_id = list[0];
                var balance = list[1];

                Pages.HD.Address.Open("?address_id=" + address_id);
                var balance_p = Pages.HD.Address.Balance;

                Assertion(
                    "Некорректное отображение баланса на УКК, контракт: " + contract +
                    ", [отображается: " + balance_p + "; должно отображатьcя: " + balance +
                    "]",
                    () => Assert.AreEqual(0, balance.IndexOf(balance_p)));
            }
        }
    }
}
