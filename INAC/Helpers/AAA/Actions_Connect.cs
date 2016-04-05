using System.IO;

namespace INAC.Helpers.AAA
{
     public static partial class Actions
    {
         public static class Connection
         {
             #region public

             public static bool ConnectToCisco(string login, string password)
             {
                 return ConnectToBras(login, password, Environment.ConnectionCisco, Environment.BrasCisco);
             }

             public static bool ConnectToEricson(string login, string password)
             {
                 return ConnectToBras(login, password, Environment.ConnectionEricson, Environment.BrasEricson);
             }

             public static void Disconnect(string connectionName)
             {
                 DeleteFiles();
                 var sw = new StreamWriter(Environment.Path + "//disconnect.txt");
                 sw.WriteLine(connectionName);
                 sw.Close();
                 System.Threading.Thread.Sleep(3000);
             }

             #endregion

             #region private
             
             private static bool ConnectToBras(string login, string password, string connectionName, string brasIp)
             {
                 Disconnect(connectionName);
                 DeleteFiles();

                 StreamWriter sw = new StreamWriter(Environment.Path + "//tmp.txt");
                 sw.AutoFlush = true;
                 sw.WriteLine(login);
                 sw.WriteLine(password);
                 sw.WriteLine(connectionName);
                 sw.WriteLine(brasIp);
                 sw.Close();

                 File.Move(Environment.Path + "//tmp.txt", Environment.Path + "//input.txt");

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

             #endregion
         }
    }
}
