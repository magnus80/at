//автор: NGadiyak
using INAC_Test.Classes.Utilites;
using INAC_Test.Classes;
using INAC_Test.WebPages;
using NUnit.Framework;

namespace INAC_Test.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Настройки"), Category("Оповещения"), Description("Созадние")]
    public class test_012 : TestBase
    {

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            ContinueFlag = true;
        }
    }
}
