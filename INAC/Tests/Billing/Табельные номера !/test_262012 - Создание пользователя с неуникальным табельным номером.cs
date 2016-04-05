using System;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("OSE"), Category("Main functional")]
    public class test_262012 : TestBase
    {
        private string login, name, email, emp_no;

        [Test]
        public void step_01()
        {
            var user = Helpers.OSE.Queries.GetUserByGroups("admin", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            login = "AT_login_" + new Random().Next(10000, 99999);
            email = "AT_email_" + new Random().Next(10000, 99999) + "@beeline.ru";
            name = "AT_name_" + new Random().Next(10000, 99999);

            var query = @"SELECT iua_employee_no
                            FROM   inac.inac_user_auth 
                            where iua_employee_no is not null";

            emp_no = Executor.ExecuteSelect(query, Environment.InacDb).GetAnyCellFromColumn(0);

            Pages.OSE.InacUsers.NewUser(login, name, email, emp_no);
        }

        [Test]
        public void step_03()
        {
            var query = @"SELECT iua_employee_no 
                            FROM   inac.inac_user_auth 
                            WHERE  iua_login = '" + login + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Абонент добавился, несмотря на неуникальнок значение поля 'табельный номер', логин: " + login,
                      () => Assert.AreEqual(res.Count, 0));
        }
    }
}
