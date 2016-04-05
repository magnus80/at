using System;
using AT;
using AT.DataBase;
using AT.Tools;
using INAC.Helpers.TL;
using NUnit.Framework;

namespace INAC.Tests.OSE
{
    [TestFixture]
    [Category("OSE"), Category("OSE_Netphone")]
    public class test_254244 : TestBase
    {
        private string serviceName, serviceId;

        private string tl, lid;

        [Test]
        public void step_01()
        {
            var user = Helpers.HD_Users.Queries.GetUserByGroups("bo_netphone_ins,bo_netphone_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            var query =
               @"SELECT s_id
                  FROM   inac.services
                         JOIN inac.services_param
                           ON s_id = id_service
                  WHERE  param_name = 'NETPHONE'
                         AND param_number = 1";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("выборка тарифов нетфона пустая", () => Assert.IsNotNull(list));

            serviceId = list[0, 0];
        }

        [Test]
        public void step_04()
        {
            serviceName = "at_loadservice_netphone_" + (new Random().Next(0, int.MaxValue)).ToString();
            tl = NetphoneTechlists.FormNetphoneServicesUpdateTechlist(serviceId, serviceName, "A");
        }

        [Test]
        public void step_05()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenNetphoneTab();
            Pages.OSE.Services.Form.NetphoneAction = "update";
            Pages.OSE.Services.Form.NetphoneLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.NetphoneTechlist = tl;
            Pages.OSE.Services.Form.NetphoneStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.NetphoneSubmit();
        }

        [Test]
        public void step_06()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.netphone_services_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_07()
        {
            var query = @"SELECT s_name
                          FROM   inac.services
                          WHERE  s_id = '" + serviceId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("загруженный сервис отсутствует в БД (выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректная смена имени сервиса [ожидаемо: " + serviceName + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(serviceName, list[0]));

        }
    }
}
