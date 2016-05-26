using AT;
using AT.DataBase;
using NUnit.Framework;
using USSS.Helpers;
using USSS.Helpers.API_REST;

namespace USSS.Tests.RestAPI
{
    [TestFixture]
    [Category("RestAPI")]
    public class ServiceActivate_FREEMIUM : TestBase
    {
        private static readonly string testName = "[API] Проверка подключения услуг";
        //string CtnMain = ReaderTestData.ReadExel("[Rest API] serviceActivate FREEMIUM", "CtnMain");
        //string CtnExtra = ReaderTestData.ReadExel("[Rest API] serviceActivate FREEMIUM", "CtnExtra");
        private readonly string ctn = ReaderTestData.ReadExel("[Rest API] serviceActivate FREEMIUM", "ctn");
        private readonly string hostAPI = ReaderTestData.ReadCExel(12, 7);
        private bool globalR = true;

        [Test]
        public void step_01_02()
        {
            Logger.PrintHeadTest(testName);
            Logger.PrintStepName("Step 1 PUT /api/1.0/request/serviceActivate для FREEMIUM");
            string type = null;
            string ctn_from = null;
            var step1 = new RestRequestPut(
                hostAPI + "/api/1.0/request/serviceActivate?ctn=" + ctn + "&serviceName=FREEM_200");
                //"&hash=" + new TokenHashSoap().GetHashAPI(ctn));
            //"&hash=" + new TokenHashRestAPI().GetHash2(ctn)
            //);
            try
            {
                type = step1.outputt.Find_Data("type");
                ctn_from = step1.outputt.Find_Data("ctn");
                try
                {
                    //проверка корректности  
                    var entitySoc =
                        Executor.ExecuteSelect(
                            "select SOC_RELATED from ecr9_service_agreement where subscriber_no like '" + ctn_from +
                            "%'")[0, 0];

                    var result =
                        Executor.ExecuteSelect(
                            @"select ctn_from, ctn_to, status, type from ecr6_subscriber_invitation  where ctn_to = '" +
                            ctn + "' and ctn_from = '" + ctn_from + "' and type = '" + type +
                            "' order by INVITE_DATE desc");

                    Assert.AreEqual("SENT", result[0, 2].Trim());

                    Assert.AreEqual(step1.outputt.Find_Data("entitySoc"), entitySoc.Trim());
                    Logger.PrintRezult(globalR, "parameters correct");
                }
                catch
                {
                    globalR = false;
                    Logger.PrintRezult(globalR, "parameters STaTUS incorrect:");
                }

                //catch
                //   {
                //     globalR = false;
                //     Logger.PrintRezult(globalR, "parameters STaTUS incorrect:");
                //   }
            }
            finally
            {
            }
        }
    }
}