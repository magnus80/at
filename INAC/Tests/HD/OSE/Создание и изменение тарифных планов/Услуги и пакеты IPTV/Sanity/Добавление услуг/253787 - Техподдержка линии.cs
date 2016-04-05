using System;
using AT;
using AT.DataBase;
using AT.Tools;
using INAC.Helpers.Services;
using INAC.Helpers.TL;
using NUnit.Framework;

namespace INAC.Tests.OSE
{
    [TestFixture]
    [Category("OSE"), Category("OSE_IPTV")]
    public class test_253787 : TestBase
    {
        private string s_id, serviceName, price;
        private string tl, lid;

        [Test]
        public void step_01()
        {
            var user = Helpers.HD_Users.Queries.GetUserByGroups("bo_iptv_ins,bo_iptv_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            Deleter.DeleteServicesFromTempCity();
        }

        [Test]
        public void step_03()
        {
            serviceName = "at_loadservice_iptv_" + (new Random().Next(0, int.MaxValue)).ToString();
            price = new Random().Next(100, 999).ToString();
            tl = IptvTechlists.FormIptvServicesInsertTechlist(Environment.TempCityId, Environment.TempCityName, "5",
                                                               serviceName, price);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenIptvTab();
            Pages.OSE.Services.Form.IptvAction = "insert";
            Pages.OSE.Services.Form.IptvLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.IptvTechlist = tl;
            Pages.OSE.Services.Form.IptvStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.IptvSubmit();
        }

        [Test]
        public void step_05()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.iptv_services_load.job", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            Pages.OSE.Services.Results.Open("?lid=" + lid);
            Pages.OSE.Services.Results.InitResultTable();

            s_id = Pages.OSE.Services.Results.ResultTable[2, 5];
            var s_name = Pages.OSE.Services.Results.ResultTable[2, 6];

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + s_name + "]",
                      () => Assert.AreEqual(serviceName, s_name));
            Assertion("Не отображается ID загруженного сервиса iptv", () => Assert.IsNotNullOrEmpty(s_id));
        }


        [Test]
        public void step_07()
        {
            var query =
                "select s_name, s_price, s_f_iptv, param_number from inac.services join inac.services_param on s_id = id_service where s_id = '" +
                s_id + "' and param_name = 'LINE_HOLDER'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("загруженный тариф отсутствует в БД (выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректное имя сервиса [ожидаемо: " + serviceName + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(serviceName, list[0]));

            Assertion("Некорректная цена сервиса [ожидаемо: " + price + ", актуально: " + list[1] + "]",
                      () => Assert.AreEqual(price, list[1]));

            Assertion("Некорректный параметр сервиса (s_f_iptv) [ожидаемо: 0, актуально: " + list[2] + "]",
                      () => Assert.AreEqual("0", list[2]));

            Assertion("Некорректное значение параметра (LINE_HOLDER) [ожидаемо: 1, актуально: " + list[3] + "]",
                      () => Assert.AreEqual("1", list[3]));
        }
    }
}
