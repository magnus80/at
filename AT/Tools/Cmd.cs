using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AT.Global;

namespace AT.Tools
{
    public static class Cmd
    {
        private static Process process;

       public static void Execute(string comand)
        {
            try
            {
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\\Windows\System32\cmd.exe",
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    }
                };

                process.Start();
                using (StreamWriter writter = process.StandardInput)
                {
                    foreach (var line in comand.Split('\n'))
                        writter.WriteLine(line);
                }                
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

       public static List<string> ReadAll()
       {
           try
           {
               var lines = new List<string>();

               using (StreamReader reader = process.StandardOutput)
               {
                   while (!process.StandardOutput.EndOfStream)
                   {
 		       var str = reader.ReadLine();
                      // var line = Other.ChangeEncoding(1251, 866, reader.ReadLine());
                       lines.Add(str);
                   }
               }
               return lines;
           }
           catch (Exception ex)
           {
               GlobalEvents.ExeptionFounded(ex);
               return null;
           }
       }

        public static string ReadLine()
        {
            try
            {
                using (StreamReader reader = process.StandardOutput)
                    return reader.ReadLine();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        public static void Close()
        {
            try
            {
                process.Close();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }
    }
}
