//автор: NGadiyak
using System;
using INAC_test.Classes;
using INAC_test.WebPages;
using NUnit.Framework;


namespace INAC_Test.Tests.HD
{
    [TestFixture]
    [Category("check")]
    public class check: TestBase
    {
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            Assert.IsTrue(true);
        }
    }
}
