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
    public class test_254038 : TestBase
    {
        private string serviceName, serviceId, cityId, cityName;

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
            var query =
                @"SELECT s_id, s_city, ct_city
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                         JOIN inac.cities0 on ct_id = s_city
                  WHERE  param_name = 'IS_AC'
                         AND param_number = 1";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("выборка тарифов годового контракта пустая", () => Assert.IsNotNull(list));

            serviceId = list[0, 0];
            cityId = list[0, 1];
            cityName = list[0, 2];
        }

        [Test]
        public void step_03()
        {
            serviceName = "at_loadservice_annual_" + (new Random().Next(0, int.MaxValue)).ToString();

            tl = AnnualContractsTechlists.FormAnnualContractUpdateTechlist(serviceId, cityId,cityName, "A", serviceName);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenAnnualTab();
            Pages.OSE.Services.Form.AnnualAction = "update";
            Pages.OSE.Services.Form.AnnualLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.AnnualTechlist = tl;
            Pages.OSE.Services.Form.AnnualStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.AnnualSubmit();
        }

        [Test]
        public void step_05()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.annual_contracts_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT s_name
                          FROM   inac.services
                          WHERE  s_id = '" + serviceId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + list[0, 0] + "]",
                      () => Assert.AreEqual(serviceName, list[0, 0]));
        }
    }
}
