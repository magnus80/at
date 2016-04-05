using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using INAC_test.Classes.Utilites;
using NUnit.Framework;


namespace INAC_Test.Tests.HD
{
    [TestFixture]
    [Category("Service"), Category("HD")]
    public class z_end_test
    {
        [Test]
        public void end()
        {
            LogWritter.Add(DateTime.Now + ": run test end" + Environment.NewLine);
            LogWritter.Add("******************************************************************************************" + Environment.NewLine + Environment.NewLine);
            
            Classes.Utilites.Notifier.FormNotif();
            Classes.Utilites.Notifier.SendNotif();
        }
    }
}
