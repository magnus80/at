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
    public class test_254236 : TestBase
    {
        private string serviceName, price;

        private string tl, lid;

        [Test]
        public void step_01()
        {
            var user = Helpers.OSE.Queries.GetUserByGroups("bo_netphone_ins,bo_netphone_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            serviceName = "at_loadservice_netphone_" + (new Random().Next(0, int.MaxValue)).ToString();
            price = new Random().Next(100, 999).ToString();
            tl = NetphoneTechlists.FormNetphoneServicesInsertTechlist(Environment.TempCityId, Environment.TempCityName,
                                                                      "A", serviceName, price, "1111");
        }

        [Test]
        public void step_03()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenNetphoneTab();
            Pages.OSE.Services.Form.NetphoneAction = "insert";
            Pages.OSE.Services.Form.NetphoneLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.NetphoneTechlist = tl;
            Pages.OSE.Services.Form.NetphoneStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.NetphoneSubmit();
        }

        [Test]
        public void step_04()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.netphone_services_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_05()
        {
            var query = @"SELECT s_price,
                                 s_city,
                                 s_f_public
                          FROM   inac.services
                          WHERE  s_name = '" + serviceName + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromRow(0);

            Assertion("загруженный сервис отсутствует в БД (выборка пустая)", () => Assert.IsNotNull(list));

            Assertion("Некорректная цена сервиса [ожидаемо: " + price + ", актуально: " + list[0] + "]",
                      () => Assert.AreEqual(price, list[0]));

            Assertion(
                "Некорректный город сервиса [ожидаемо: " + Environment.TempCityId + ", актуально: " + list[1] + "]",
                () => Assert.AreEqual(Environment.TempCityId, list[1]));

            Assertion("Некорректный статус сервиса (s_f_public) [ожидаемо: 1, актуально: " + list[2] + "]",
                      () => Assert.AreEqual("1", list[2]));
        }
    }
}
