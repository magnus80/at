using AT.DataBase;
using System.Diagnostics;

//using learncsharp.Service;

namespace USSS.Helpers
{
    class TokenHashRestAPI
    {
        public string GetSystemTokenREST()
        {
            return Executor.ExecuteSelect(
                "select token from bsscm5.ecr6_api_system_tokens where signature like 'PREVED' and description like '%системка%' ")[0, 0];
        }

        public string GetHash2(string parametres)
        {
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-cp c:\\WorkTools.jar ru.dl.work.bss.UssHashGenerator PREVED true " + parametres;
            p.Start();
            // instead of p.WaitForExit(), do
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string rez = output.Remove(0, output.IndexOf("hash  =") + 7);
            rez = rez.Remove(rez.IndexOf('='), 3);

            return rez + "=";
        }
    }
}
