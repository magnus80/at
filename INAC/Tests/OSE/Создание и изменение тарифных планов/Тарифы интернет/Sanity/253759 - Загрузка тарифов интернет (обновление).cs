using System;
using AT;
using AT.DataBase;
using AT.Tools;
using INAC.Helpers.TL;
using NUnit.Framework;

namespace INAC.Tests.OSE
{
    [TestFixture]
    [Category("OSE"), Category("OSE_Internet")]
    public class test_253759 : TestBase
    {
        private string tl, lid;
        private string s_id, s_name_new;

        [Test]
        public void step_01()
        {
            var user = Helpers.OSE.Queries.GetUserByGroups("bo_vpdn_ins,bo_vpdn_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            var query = @"SELECT DISTINCT s_id
                          FROM   inac.services
                                 JOIN (SELECT DISTINCT s_id AS service
                                       FROM   inac.services
                                       WHERE  s_f_vpdn = 1
                                              AND s_f_public = 1
                                              AND s_city = 12042
                                       minus
                                       SELECT id_service AS service
                                       FROM   inac.services_param
                                       WHERE  param_name = 'HD_VISIBLE')
                                   ON s_id = service
                          INTERSECT
                          SELECT id_service
                          FROM   inac.services_param
                            join inac.vpdn on id_service = v_service
                          WHERE  param_name = 'RESERV_TYPE'
                                 AND param_number = 0 and v_limit_out = 0";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();

            Assertion("выборка публичных тарифов интернета для Москвы пустая", () => Assert.IsNotNull(list));

            s_id = list[0];
        }

        [Test]
        public void step_03()
        {
            s_name_new = "at_updateservice_vpdn_" + (new Random().Next(0, int.MaxValue)).ToString();
            tl = FttbTechlists.FormFttbUpdateTechlist(s_id, "R", s_name_new);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenVpdnTab();
            Pages.OSE.Services.Form.VpdnAction = "update";
            Pages.OSE.Services.Form.VpdnLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.VpdnTechlist = tl;
            Pages.OSE.Services.Form.VpdnStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.VpdnSubmit();
        }

        [Test]
        public void step_05()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "p_lid", lid));
            Executor.ExecuteProcedure("inac.pkg_services_load.p_load_rfc", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            Pages.OSE.Services.Results.Open("?lid=" + lid);
            Pages.OSE.Services.Results.InitResultTable();

            var s_name = Pages.OSE.Services.Results.ResultTable[1, 4];

            Assertion("Некорректное имя сервиса [ожидаемо: " + s_name_new + ", актуально: " + s_name + "]",
                      () => Assert.AreEqual(s_name_new, s_name));
        }

        [Test]
        public void step_07()
        {
            var query = "select s_name,s_f_public, param_number from inac.services join inac.services_param on s_id = id_service where s_id = '" + s_id + "' and param_name = 'HD_VISIBLE'";
            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("ошибка при обновлении тарифа (отсутствует параметр HD_VISIBLE)(выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректное имя сервиса [ожидаемо: " + s_name_new + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(s_name_new, list[0]));

            Assertion("Некорректный статус сервиса (s_f_public) [ожидаемо: 1, актуально: " + list[1] + "]",
                      () => Assert.AreEqual("0", list[1]));

            Assertion("Некорректное значение параметра (HD_VISIBLE) [ожидаемо: 1, актуально: " + list[2] + "]",
                      () => Assert.AreEqual("1", list[2]));
        }
    }
}
