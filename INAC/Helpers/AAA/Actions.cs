using System.IO;
using System.Linq;
using AT.DataBase;
using AT.Tools;

namespace INAC.Helpers.AAA
{
    public static partial class Actions
    {
        public static void WaitForCloseSession()
        {
            System.Threading.Thread.Sleep(40000);
        }
        

        public static string GetParam(string paramString, string paramName)
        {
            foreach (var par in paramString.Split(';'))
            {
                if (par.IndexOf(paramName) != -1) return par.Remove(0, paramName.Length + 1);
            }
            return string.Empty;
        }

        private static void DeleteFiles()
        {
            if (new DirectoryInfo(Environment.Path).GetFiles().Any(res => res.Name.IndexOf("input.txt") != -1))
                File.Delete(Environment.Path + "\\input.txt");
            if (new DirectoryInfo(Environment.Path).GetFiles().Any(res => res.Name.IndexOf("res.txt") != -1))
                File.Delete(Environment.Path + "\\res.txt");
            if (new DirectoryInfo(Environment.Path).GetFiles().Any(res => res.Name.IndexOf("disconnect.txt") != -1))
                File.Delete(Environment.Path + "\\disconnect.txt");
            if (new DirectoryInfo(Environment.Path).GetFiles().Any(res => res.Name.IndexOf("tracert.txt") != -1))
                File.Delete(Environment.Path + "\\tracert.txt");
        }
    }
}
