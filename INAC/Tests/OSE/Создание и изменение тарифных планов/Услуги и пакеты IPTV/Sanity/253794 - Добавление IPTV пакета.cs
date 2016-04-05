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
    public class test_253794 : TestBase
    {
        private string tl, lid;
        private string basePacketName, basePacketId, effectivePrice;

        [Test]
        public void step_01()
        {
            var user = Helpers.OSE.Queries.GetUserByGroups("bo_iptv_ins,bo_iptv_upd", Environment.InacDb);
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
            effectivePrice = "1";
            basePacketName = "at_loadservice_IPTV-packet_" + (new Random().Next(0, int.MaxValue)).ToString();
            tl = IptvTechlists.FormIptvPacketsInsertTechlist(basePacketName, "A", effectivePrice,
                                                              Environment.TempCityId, Environment.TempCityName);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenIptvTab();
            Pages.OSE.Services.Form.IptvAction = "insert_packets";
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
            Executor.ExecuteProcedure("inac.iptv_packets_load_pkg.job", Environment.InacDb);
        }

        [Test]
        public void step_06()
        {
            var query = @"
                            SELECT pkt_id,
                                   pkt_city_id
                            FROM   inac.iptv_packets 
                            WHERE pkt_name = '" + basePacketName + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("IPTV-пакет не добавился в БД, название: " + basePacketName, () => Assert.IsNotNull(res));


            basePacketId = res[0, 0];

            Assertion(
                "Не заполнилось поле ID пакета, пакет: " + basePacketName,
                () => Assert.IsNotNullOrEmpty(basePacketId));


            Assertion(
                "Некорректный город добавния, [ожидаемо " + Environment.TempCityId + ", актуально: " + res[0, 1] + "]",
                () => Assert.AreEqual(res[0, 1], Environment.TempCityId));
        }

        [Test]
        public void step_07()
        {

            var query = @"SELECT param_number
                            FROM   inac.iptv_packets_param
                            WHERE  param_name = 'EFFECTIVE_PRICE' 
                                   AND id_packet = '" + basePacketId + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion(
                "Некорректная эффективная цена пакета [ожидаемо: " + effectivePrice + ", актуально: " + res[0, 0] + "]",
                () => Assert.AreEqual(res[0, 0], effectivePrice));
        }
    }
}
