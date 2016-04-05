//автор: NGadiyak
using AT;
using AT.DataBase;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Списания абонентской платы showcomments.pl")]
    public class test_245019 : TestBase
    {
        private string login, contract;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT DISTINCT s_id
                      FROM   inac.services
                             JOIN inac.services_param
                               ON s_id = id_service
                      WHERE  param_name = 'RESERV_TYPE'
                             AND param_number = 1
                             AND s_city = 12042
                             AND s_f_public = 1
                             AND s_price > 150
                             AND s_abontype = 1
                             AND s_f_vpdn = 1
                             AND s_currency = 2 
                      ";

            var serv = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            login = Helpers.Abonents.Actions.Creation.Create(serv, 10000);
        }

        [Test]
        public void step_03()
        {
            contract = Helpers.Abonents.Info.GetContract(login);

            Helpers.Abonents.Actions.Reaccount(contract);
        }

        [Test]
        public void step_04()
        {
            float sum_f;
            float.TryParse(Helpers.Abonents.Info.GetServicesPriceSum(login), out sum_f);

            Pages.HD.Showcomments.Open("?login=" + login);

            var count = Pages.HD.Showcomments.FindWriteOff(sum_f.ToString("0.00"));

            Assertion(
                "Некорректное отображение списаний абон. платы " + Browser.Url,
                () => Assert.AreEqual(1, count));
        }
    }
}
