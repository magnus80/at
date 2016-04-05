using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;
using USSS.AuthSoap;

namespace USSS.Helpers
{
    class TokenHashSoap
    {
        public string GetToken(string login, string password)
        {
            AuthSoap.AuthInterface d = new AuthInterfaceClient();
            AuthSoap.authRequest authRequest = new authRequest();
            authRequest.login = login;
            authRequest.password = password;
            string token = String.Empty;
            try
            {
                AuthSoap.authResponse authResponse = d.auth(authRequest);
                return authResponse.@return;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetSystemToken()
        {
            return Executor.ExecuteSelect("select token from bsscm5.ecr6_api_system_tokens where signature = 'PREVED' and description like '%системка%' ")[0, 0];
        }

        public string GetHashAPI(string paramentrs)
        {
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-cp c:\\WorkTools.jar ru.dl.work.bss.UssHashGenerator PREVED true " + paramentrs;
            p.Start();
            // instead of p.WaitForExit(), do
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string rez = output.Remove(0, output.IndexOf("hash  =") + 7);
           
            rez = rez.Remove(rez.IndexOf("\r\n"), 1);

            return rez + "=";
        }

        public string GetHash(string paramentrs)
        {
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-cp c:\\WorkTools.jar ru.dl.work.bss.UssHashGenerator PREVED false " + paramentrs;
            p.Start();
            // instead of p.WaitForExit(), do
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string rez = output.Remove(0, output.IndexOf("hash  =") + 7);
            rez = rez.Remove(rez.IndexOf('='), 3);
            return rez+"=";
        }
    }
}
