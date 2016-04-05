using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Properties;

namespace USSS.Helpers
{
    static class Logger
    {

        public static void PrintHeadTest(string name)
        {
            string outputMassage = "Test " +name;
            WriteString(outputMassage);
        }

        public static void PrintStepName(string name)
        {
            string outputMassage = "        " + name;
            WriteString(outputMassage);
        }

        public static void PrintAction(string name, string param)
        {
            string outputMassage = "            " + DateTime.Now.ToString() + ";       " + name + "; параметры: " + param;
            WriteString(outputMassage);
        }

        public static void PrintStr(string name)
        {
            string outputMassage = "            "+name;
            WriteString(outputMassage);
        }

        public static void PrintRezult(bool rezult, string massege)
        {
            string outputMassage;
            if (rezult)
            {
                outputMassage = "           PASSED;";
            }
            else
            {
                outputMassage = "       FAILED;";
            }
            outputMassage = outputMassage + "       " + massege;
            WriteString(outputMassage);
        }

        public static void PrintRezultTest(bool rezult)
        {
            string outputMassage;
            if (rezult)
            {
                outputMassage = "TC PASSED;";
            }
            else
            {
                outputMassage = "TC FAILED;";
            }
            WriteString(outputMassage);
            WriteString(" ");
        }

        static string logDirPath = "C:\\ATU\\Logs\\";
        static FileInfo currentFile = new FileInfo(logDirPath + DateTime.Now.ToString(@"yyyy_MM_dd_HH-mm-ss") +" 23 стенд" +  ".txt");

        private static void WriteString(String message)
        {
            try
            {
               
                DirectoryInfo dir = new DirectoryInfo(logDirPath);
                if (dir.Exists == false)
                {
                    dir.Create();
                }

                StreamWriter streamWriter;
                
                streamWriter = currentFile.AppendText();
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        public static void ErrorAccumulate(ref string ErrorMessageCheck, string ErrorText, ref int counterErr)
        {
            ErrorMessageCheck = ErrorMessageCheck + " " + ErrorText;
            counterErr++;

        }

        public static string ErrorReturn(ref string ErrorMessageCheck, ref int counterErr)
        {
            if (counterErr != 0) return ErrorMessageCheck;
            ErrorMessageCheck = "Не найдено: ";
            counterErr = 0;
            return "success";
        }
    }
}
