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
    public class test_253888 : TestBase
    {
        private string tl, lid;

        private string packetNameNew,
                       packetId,
                       effectivePriceNew,
                       externalId;

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
            var query = @"SELECT pkt_id, pkt_name
                          FROM   inac.iptv_packets
                          WHERE  pkt_city_id = " + Environment.TempCityId +
                        "AND pkt_parent IS NULL";

            var list = Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromAnyRow();

            packetId = list[0];
            var packetNameOld = list[1];
            packetNameNew = "at_loadservice_IPTV-packet_" + (new Random().Next(0, int.MaxValue)).ToString();

            if (packetNameNew.Equals(packetNameOld)) 
                packetNameNew = "at_loadservice_IPTV-packet_" + (new Random().Next(0, int.MaxValue)).ToString();

            query = @"SELECT param_number
                      FROM   inac.iptv_packets_param
                      WHERE  param_name = 'EXTERNAL_ID'
                            AND id_packet = " + packetId;

            externalId = Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];

            query = @"SELECT param_number
                      FROM   inac.iptv_packets_param
                      WHERE  param_name = 'EFFECTIVE_PRICE'
                            AND id_packet = " + packetId;

            var effectivePriceOld = Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];

            effectivePriceNew = new Random().Next(91, 99).ToString();

             if (effectivePriceNew.Equals(effectivePriceOld))
                 effectivePriceNew = new Random().Next(91, 99).ToString();
        }

        [Test]
        public void step_03()
        {
            tl = IptvTechlists.FormIptvPacketsUpdateTechlist(packetId, packetNameNew, externalId, effectivePriceNew);
        }

        [Test]
        public void step_04()
        {
            Pages.OSE.Services.Form.Open();
            Pages.OSE.Services.Form.OpenIptvTab();
            Pages.OSE.Services.Form.IptvAction = "update_packets";
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
            var query = @"SELECT pkt_name
                          FROM   inac.iptv_packets
                          WHERE  pkt_id = " + packetId;

            var packetName = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "Некорректное обновление имени пакета, id: " + packetId + " [ожидаемо: " + packetNameNew +
                ", актуально: " + packetName + "]",
                () => Assert.AreEqual(packetNameNew, packetName));
        }

        [Test]
        public void step_07()
        {
            var query = @"SELECT param_number
                          FROM   inac.iptv_packets_param
                          WHERE  param_name = 'EFFECTIVE_PRICE' AND id_packet = " + packetId;

            var effectivePrice = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion(
                "Некорректное обновление эффективной цены пакета, id: " + packetId + " [ожидаемо: " + effectivePriceNew +
                ", актуально: " + effectivePrice + "]",
                () => Assert.AreEqual(effectivePriceNew, effectivePrice));
        }

    }
}
