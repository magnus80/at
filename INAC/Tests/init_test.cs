using System.Collections.Generic;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests
{
    [TestFixture]
    [Category("Service")]
    public class c__init_test : TestBase
    {
        [Test]
        public void step_01()
        {
            var system = AT.Tools.Other.GetEnvironmentVariable("system");

            switch (system)
            {
                case "HD":
                    ClearGlobalProblems();
                    ClearVipClients();
                    break;
                case "AAA":
                    PrepareAAA();
                    break;

            }
        }

        private void ClearGlobalProblems()
        {
            var query = @"UPDATE helpdesk.tickets
                          SET    t_status = 15
                          WHERE  t_id IN (SELECT gp_id
                                        FROM   helpdesk.global_problems)";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }

        private void ClearVipClients()
        {
            var query = @"UPDATE helpdesk.advanced_adr_info
                          SET    vip_client = 0";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
        
        public void PrepareAAA()
        {
            var abons = new List<string>();

            for (var i = 0; i < 14; i++)
            {
                Helpers.AAA.Actions.Prepare.NewAbonentsPackLimit(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackUnlim(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackVSU(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackTB(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackShaped(ref abons, 1);
                Helpers.AAA.Actions.Prepare.NewAbonentsPackMultiService(ref abons, 1);
            }

            int block_type = 0;
            for (var i = 0; i < abons.Count; i++, block_type++)
            {
                Helpers.Abonents.Actions.SetBlockType(abons[i], block_type);
                if (block_type == 6) block_type = 0;
            }

            Helpers.Abonents.Actions.ReaccAll();
            System.Threading.Thread.Sleep(60*15*1000); /* ожидание репликации БД 15 минут */
        }
    }
}
