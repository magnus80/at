using System;
using AT;
using AT.DataBase;
using AT.Tools;
using INAC.Helpers.TL;
using NUnit.Framework;

namespace INAC.Tests.OSE
{
    [TestFixture]
    [Category("OSE"), Category("OSE_Annual")]
    public class test_253992 : TestBase
    {
        private string serviceName, price, serviceId;
        private int time;

        private string tl, lid;

        [Test]
        public void step_01()
        {
            var user = Helpers.HD_Users.Queries.GetUserByGroups("bo_annual_ins,bo_annual_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            serviceName = "at_loadservice_annual_" + (new Random().Next(0, int.MaxValue)).ToString();
            price = new Random().Next(100, 999).ToString();
            time = new Random().Next(3, 12);
            tl = AnnualContractsTechlists.FormAnnualContractInsertTechlist(serviceName, Environment.TempCityId,
                                                                           Environment.TempCityName, "A", price, time);
        }

        [Test]
        public void step_03()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenAnnualTab();
            Pages.OSE.Services.Form.AnnualAction = "insert";
            Pages.OSE.Services.Form.AnnualLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.AnnualTechlist = tl;
            Pages.OSE.Services.Form.AnnualStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.AnnualSubmit();
        }

        [Test]
        public void step_04()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.annual_contracts_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_05()
        {
            Pages.OSE.Services.Results.Open("?lid=" + lid);

            Pages.OSE.Services.Results.InitResultTable();


            serviceId = Pages.OSE.Services.Results.ResultTable[3, 3];
            var s_name = Pages.OSE.Services.Results.ResultTable[3, 2];

            
            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + s_name + "]",
                      () => Assert.AreEqual(serviceName, s_name));
            Assertion("Не отображается ID загруженного сервиса интернета", () => Assert.IsNotNullOrEmpty(serviceId));
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT s_name,
                                 s_city,
                                 s_f_public
                          FROM   inac.services
                          WHERE  s_id = '" + serviceId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("загруженный сервис отсутствует в БД (выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(serviceName, list[0]));

            Assertion(
                "Некорректный город сервиса [ожидаемо: " + Environment.TempCityId + ", актуально: " + list[1] + "]",
                () => Assert.AreEqual(Environment.TempCityId, list[1]));

            Assertion("Некорректный статус сервиса (s_f_public) [ожидаемо: 1, актуально: " + list[2] + "]",
                      () => Assert.AreEqual("1", list[2]));
        }

        [Test]
        public void step_07()
        {
            var query =
                @"SELECT param_number 
                FROM inac.services_param 
                WHERE id_service = '" + serviceId +
                "' AND param_name = 'IS_AC'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "в БД отсутствует параметр загруженного сервиса (выборка пустая), [параметр: IS_AC" + ", сервис: " +
                serviceId + "]", () => Assert.IsNotNull(res));

            Assertion("Некорректное значение параметра (IS_AC) [ожидаемо: 1, актуально: " + res[0, 0] + "]",
                      () => Assert.AreEqual("1", res[0, 0]));

        }

        [Test]
        public void step_08()
        {
            var query =
                @"SELECT param_number 
                FROM inac.services_param 
                WHERE id_service = '" + serviceId +
                "' AND param_name = 'AC_SCALE_SIZE'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "в БД отсутствует параметр загруженного сервиса (выборка пустая), [параметр: AC_SCALE_SIZE" +
                ", сервис: " + serviceId + "]", () => Assert.IsNotNull(res));

            Assertion(
                "Некорректное значение параметра (AC_SCALE_SIZE) [ожидаемо: " + time + ", актуально: " + res[0, 0] + "]",
                () => Assert.AreEqual(time.ToString(), res[0, 0]));

        }

        [Test]
        public void step_09()
        {
            var query =
                @"SELECT param_number 
                FROM inac.services_param 
                WHERE id_service = '" + serviceId +
                "' AND param_name = 'AC_SERV_PARAM'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "в БД отсутствует параметр загруженного сервиса (выборка пустая), [параметр: AC_SERV_PARAM" +
                ", сервис: " + serviceId + "]", () => Assert.IsNotNull(res));

            Assertion(
                "Некорректное значение параметра (AC_SERV_PARAM) [ожидаемо: " + price + ", актуально: " + res[0, 0] +
                "]",
                () => Assert.AreEqual(price, res[0, 0]));

        }
    }
}
