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
    public class test_253750 : TestBase
    {
        private string tl, lid;
        private string s_id, serviceName, price;

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
            serviceName = "at_loadservice_vpdn_" + (new Random().Next(0, int.MaxValue)).ToString();
            price = new Random().Next(100, 999).ToString();
            tl = FttbTechlists.FormFttbInsertTechlist("A", "U", "M", price, serviceName);
        }

        [Test]
        public void step_03()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenVpdnTab();
            Pages.OSE.Services.Form.VpdnAction = "insert";
            Pages.OSE.Services.Form.VpdnLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.VpdnTechlist = tl;
            Pages.OSE.Services.Form.VpdnStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.VpdnSubmit();
        }

        [Test]
        public void step_04()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "p_lid", lid));
            Executor.ExecuteProcedure("inac.pkg_services_load.p_load_rfc", Environment.InacDb);
        }

        [Test]
        public void step_05()
        {
            Pages.OSE.Services.Results.Open("?lid=" + lid);
            Pages.OSE.Services.Results.InitResultTable();

            s_id = Pages.OSE.Services.Results.ResultTable[2, 4];
            var s_name = Pages.OSE.Services.Results.ResultTable[2, 5];

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + s_name + "]",
                      () => Assert.AreEqual(serviceName, s_name));
            Assertion("Не отображается ID загруженного сервиса интернета", () => Assert.IsNotNullOrEmpty(s_id));
        }

        [Test]
        public void step_06()
        {
            var query = "select s_name, s_price, s_f_public, s_f_vpdn from inac.services where s_id = '" + s_id + "'";
            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("загруженный тариф отсутствует в БД (выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(serviceName, list[0]));

            Assertion("Некорректная цена сервиса [ожидаемо: " + price + ", актуально: " + list[1] + "]",
                      () => Assert.AreEqual(price, list[1]));

            Assertion("Некорректный статус сервиса (s_f_public) [ожидаемо: 1, актуально: " + list[2] + "]",
                     () => Assert.AreEqual("1", list[2]));

            Assertion("Некорректный параметр сервиса (s_f_vpdn) [ожидаемо: 1, актуально: " + list[3] + "]",
                     () => Assert.AreEqual("1", list[3]));
        }
    }
}
