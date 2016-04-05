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
    public class test_253934 : TestBase
    {
        private string tl, lid;
        private string bundleNameNew, bundleId;

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
            var query = @"SELECT s_id,
                                 s_city,
                                 ct_city
                          FROM   inac.services
                                 JOIN inac.services_param
                                   ON s_id = id_service
                                 JOIN inac.cities0
                                   ON s_city = ct_id
                          WHERE  param_name = 'IS_BUNDLE'
                                 AND param_number = 1
                                 AND s_city = " + Environment.TempCityId;

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();

            Assertion("выборка бандлов для обновления, выборка пустая, город: " + Environment.TempCityId,
                      () => Assert.IsNotNull(list));

            bundleId = list[0];
        }

        [Test]
        public void step_03()
        {
            bundleNameNew = "at_updateservice_bundle_" + (new Random().Next(0, int.MaxValue)).ToString();
            tl = BundlesTechlists.FromBundlesUpdateTechlist(bundleId, Environment.TempCityId, Environment.TempCityName,
                                                            "A", bundleNameNew);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenBundleTab();
            Pages.OSE.Services.Form.BundleAction = "update";
            Pages.OSE.Services.Form.BundleLid = new Random().Next(100000, 999999).ToString();
            Pages.OSE.Services.Form.BundleTechlist = tl;
            Pages.OSE.Services.Form.BundleStartDate = DateTime.Now.ToShortDateString();
            Pages.OSE.Services.Form.BundleSubmit();
        }


        [Test]
        public void step_05()
        {
            lid = Other.GetParamFromCurrentUrl("lid");
            lid = lid.Substring(0, lid.IndexOf('#'));
            Executor.ExecuteProcedure("inac.bundles_services_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT s_name 
                          FROM inac.services 
                          WHERE s_id ='" + bundleId + "'";

            var list = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Некорректное обновление имени бандла, бандл: " + bundleId + " [ожидаемо: " + bundleNameNew +
                      ", актуально: " + list[0, 0] + "]",
                      () => Assert.AreEqual(bundleNameNew, list[0, 0]))
                ;
        }
    }
}
