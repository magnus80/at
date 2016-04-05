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
    [Category("OSE"), Category("OSE_Bundles")]
    public class test_253889 : TestBase
    {
        private string tl, lid;
        private string bundleName, price, bundleId;

        [Test]
        public void step_01()
        {
            var user = Helpers.HD_Users.Queries.GetUserByGroups("bo_bundles_ins,bo_bundles_upd", Environment.InacDb);
            Pages.OSE.Inaclogin.Open();
            Pages.OSE.Inaclogin.Login = user[0];
            Pages.OSE.Inaclogin.Password = user[1];
            Pages.OSE.Inaclogin.Submit();
        }

        [Test]
        public void step_02()
        {
            bundleName = "at_loadbundle_vpdn_" + (new Random().Next(0, int.MaxValue)).ToString();
            price = new Random().Next(100, 999).ToString();
            tl = BundlesTechlists.FromBundlesInsertTechlist(Environment.TempCityId, Environment.TempCityName, "A", price,
                                                            bundleName);
        }

        [Test]
        public void step_03()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenBundleTab();
            Pages.OSE.Services.Form.BundleAction = "insert";
            Pages.OSE.Services.Form.BundleLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.BundleTechlist = tl;
            Pages.OSE.Services.Form.BundleStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.BundleSubmit();
        }


        [Test]
        public void step_04()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.bundles_services_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_05()
        {
            var query = @"SELECT s_id
                            FROM   inac.services
                            WHERE  s_name = '" + bundleName + "' and s_city = " +
                        Environment.TempCityId;

            var res = Executor.ExecuteSelect(query, Environment.InacDb);


            Assertion("Ошибка при загрузке бандла, имя: " + bundleName, () => Assert.IsNotNull(res));

            bundleId = res[0, 0];
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT param_number
                          FROM   inac.services_param
                          WHERE  param_name = 'IS_BUNDLE'
                                 AND id_service = '" + bundleId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Некорректное значение параметра (IS_BUNDLE) [ожидаемо: 1, актуально: " + list[0, 0] + "]",
                      () => Assert.AreEqual("1", list[0,0]));
        }

        [Test]
        public void step_07()
        {
            var query = @"SELECT param_number
                          FROM   inac.services_param
                          WHERE  param_name = 'EF_PRICE'
                                 AND id_service = '" + bundleId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Некорректное значение параметра (EF_PRICE) [ожидаемо: " + price + ", актуально: " + list[0, 0] + "]",
                      () => Assert.AreEqual(price, list[0, 0]));
        }

}
}
