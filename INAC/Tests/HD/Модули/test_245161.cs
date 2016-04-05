//автор: NGadiyak
using AT;
using AT.DataBase;
using AT.WebDriver;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Модули"), Description("Доверительные платежи promissed_pay.pl - blacklist")]
    public class test_245161 : TestBase
    {
        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            var query = @"select * from (SELECT l_contract,
                                ppbl_login 
                         FROM   inac.promised_payments_blacklist
                                 JOIN inac.logins
                                  ON l_login = ppbl_login order by dbms_random.value) where rownum = 1";

            var res = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();

            Pages.HD.Promissed_pay.Open("?contract=" + res[0]);
            Pages.HD.Promissed_pay.BlackListTableTableInit();
            var login_pp = Pages.HD.Promissed_pay.BlackListTable[2, 1];
            
            Assertion(
                "Некорректные данные доверительного платежа - blacklist - (логин)" + Browser.Url,
                () => Assert.AreEqual(login_pp, res[1]));
        }
    }
}
