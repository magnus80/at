using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.UssProcess3;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    class GetUSSSTest : TestBase
    {
        static string testName = "[SOAP] Получение набора рекомендуемых предложений";

        string ctn = ReaderTestData.ReadExel(testName, "ctn");
        private string ctn13 = ReaderTestData.ReadExel(testName, "ctn13");
        TokenHashSoap ths = new TokenHashSoap();
        private bool globalR=true;
        [Test]
        public void step_01()
        {
            Logger.PrintStepName("Step 1");
            UssProcess3.UsssProcess3 usssProcess3 = new UsssProcess3Client();
            UssProcess3.getUsss getRequest = new UssProcess3.getUsss();
            getRequest.ctn = ctn;
            getRequest.channel_type = 4;

            try
            {
                Logger.PrintAction("Получение набора рекомендуемых предложений", "");//http://nba33.vimpelcom.ru:8082/cmws/v3/usss
                UssProcess3.getUsssResponse requestResponse = usssProcess3.getUsss(getRequest);
                if (requestResponse.USSS != null)
                {
                    
                    var s = requestResponse.USSS[0].soc_code;
                    Logger.PrintRezult(true,"Данные получены " + s);
                }
                else
                {
                    Logger.PrintRezult(false,"не вышло");
                    globalR = false;
                }
            }
            catch (Exception ex)
            {
                Logger.PrintRezult(false,"Ошибка при получении набора рекомендуемых предложений " + ex.Message);
                globalR = false;
            }
        }

        [Test]
        public void step_02()
        {
            Logger.PrintStepName("Step 2");
            UssProcess3.UsssProcess3 usssProcess3 = new UsssProcess3Client();
            UssProcess3.getUsss getRequest = new getUsss();
            getRequest.ctn = ctn13;
            getRequest.channel_type = 4;

            try
            {
                Logger.PrintAction("Получение набора рекомендуемых предложений", "");
                UssProcess3.getUsssResponse requestResponse = usssProcess3.getUsss(getRequest);

                if(requestResponse.USSS == null)
                {
                    Logger.PrintRezult(true,"Рек предложений нет");
                }
                else
                {
                    Logger.PrintRezult(false,"Рек предложения загружены");
                    globalR = false;
                }
            }
            catch(Exception e)
            {
                Logger.PrintRezult(false,"Ошибка получения набора рекомендуемых предложений "+e.Message);
                globalR = false;
            }

            Logger.PrintRezultTest(globalR);
        }
    }
}
