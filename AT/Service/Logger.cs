using System;
using System.IO;
using AT.Global;

namespace AT.Service
{
    public static class Logger
    {
        public static void TestSetUp()
        {
            Write(GlobalVariables.CurrentTest + " run begin");
        }

        public static void TestTearDown()
        {
            Write(GlobalVariables.CurrentTest + " run end");
        }

        public static void StepSetUp()
        {
            //Write("test step run begin");
        }

        public static void StepTearDown()
        {
           // Write("test step run end");
        }

        public static void AssertionFailed(string errorText)
        {
            Write(errorText);
        }

        public static void Write(string errorMessage)
        {
            var sw = new StreamWriter(GlobalVariables.LogFileFullName, true);
            var line = DateTime.Now.ToString(GlobalVariables.DateTimeFormat) + ": " + errorMessage;

            sw.AutoFlush = true;
            sw.WriteLine(line);

            sw.Close();
        }
    }
}
