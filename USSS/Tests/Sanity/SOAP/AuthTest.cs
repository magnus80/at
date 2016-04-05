using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.AuthSoap;
using USSS.Helpers;

namespace USSS.Tests.SOAP
{
    [TestFixture]
    [Category("SOAP")]
    class AuthTest : TestBase
    {
        static string testName = "[SOAP] Метод аутентификации [auth]";
        string login = ReaderTestData.ReadExel(testName, "Login");
        string password = ReaderTestData.ReadExel(testName, "Password");

        TokenHashSoap ths = new TokenHashSoap();

        [Test]
        public void step_01()
        {

            Logger.PrintHeadTest("[SOAP] Метод аутентификации [auth]");
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("подключению к сервису", "");
            AuthSoap.AuthInterface d = new AuthInterfaceClient();
            AuthSoap.authRequest authRequest = new authRequest();
            authRequest.login = login;
            authRequest.password = password;
            string token = String.Empty;
            try
            {
                AuthSoap.authResponse authResponse = d.auth(authRequest);
                token = ths.GetToken(login, password);
                Logger.PrintAction("Токен получен", token);
            }
            catch (Exception ex)
            {
                Assertion("Ошибка получения токена: " + ex.Message, Assert.Fail);
            }
        }
    }
}
