using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("OSE"), Category("Main functional")]
    public class test_262010 : TestBase
    {
        [Test]
        public void step_01()
        {
            var query = @"WITH t1 
                             AS (SELECT data_type, data_length, column_name, 
                                        CASE 
                                          WHEN column_name = 'IUA_EMPLOYEE_NO'
                                               AND owner = 'INAC' 
                                               AND table_name = 'INAC_USER_AUTH' THEN 'PASS'
                                          ELSE 'FAIL' 
                                        END AS column_exists 
                                 FROM   all_tab_columns) 
                        SELECT DISTINCT t1.data_type, t1.data_length, t1.column_exists 
                        FROM   t1 
                        WHERE  column_exists = 'PASS'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Отсутствует поле IUA_EMPLOYEE_NO в таблице INAC_USER_AUTH", () => Assert.GreaterOrEqual(res.Count, 1));

            var data_type = res[0, 0];
            var data_lenth = res[0, 1];

            Assertion(
                "Не соответствует тип данных поля IUA_EMPLOYEE_NO в таблице INAC_USER_AUTH [ожидаемо: VARCHAR2 ; актуально: " + data_type +
                "]", () => Assert.AreEqual("VARCHAR2", data_type));

            Assertion(
                "Не соответствует длина поля IUA_EMPLOYEE_NO в таблице INAC_USER_AUTH [ожидаемо: 255 ; актуально: " + data_lenth +
                "]", () => Assert.AreEqual("255", data_lenth));
        }
    }
}
