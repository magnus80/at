using System;
using AT.Tools;

namespace INAC.Helpers.TL
{
    internal static class Techlist
    {
         public static string SaveAndClose(ref ExcelWorker ex, string prefix)
         {
             string filename = prefix + "_" + DateTime.Now.ToShortDateString() + "_" +
                             new Random().Next(100, 999).ToString();

             string fileFullName = Environment.TlOutputPath + filename + ".xls";
             ex.SaveAs(fileFullName);
             ex.Close();

             return fileFullName;
         }
    }
}
