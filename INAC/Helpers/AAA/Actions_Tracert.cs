using System.IO;

namespace INAC.Helpers.AAA
{
     public static partial class Actions
    {
         public static class Tracert
         {
             public static bool TracertCisco()
             {
                 return TracertFunc(Environment.BrasCisco);
             }

             public static bool TracertEricson()
             {
                 return TracertFunc(Environment.BrasEricson);
             }

             private static bool TracertFunc(string brasIp)
             {
                 DeleteFiles();
                 var sw = new StreamWriter(Environment.Path + "//tracert.txt");
                 sw.WriteLine(brasIp);
                 sw.Close();
                 System.Threading.Thread.Sleep(3000);

                 int iter = 0;
                 while (iter++ < 20)
                 {
                     DirectoryInfo dir = new DirectoryInfo(Environment.Path);
                     foreach (var item in dir.GetFiles())
                     {
                         if (item.Name.IndexOf("res.txt") != -1)
                         {
                             var sr = new StreamReader(Environment.Path + "\\res.txt");
                             var res = sr.ReadLine();
                             sr.Close();
                             DeleteFiles();
                             return res.Equals("pass");
                         }
                     }
                     System.Threading.Thread.Sleep(1000);
                 }
                 DeleteFiles();
                 return false;
             }
         }
    }
}
