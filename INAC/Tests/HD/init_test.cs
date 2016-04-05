using System;
using System.Text;
using INAC_test.Classes.Utilites;
using NUnit.Framework;
using System.IO;


namespace INAC_Test.Tests.HD
{
    [TestFixture]
    [Category("Service"), Category("HD")]
    public class init_test
    {
        [Test]
        public void init()
        {
            LogWritter.Add(Environment.NewLine + Environment.NewLine + "******************************************************************************************" );
            LogWritter.Add(DateTime.Now + ": run test begin" + Environment.NewLine);
            var sw = new StreamWriter("Launches\\" + "run_" + DateTime.Now.ToShortDateString() + ".txt", false, Encoding.Default);
            sw.Close();
        }
    }
}
