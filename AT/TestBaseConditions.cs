
using NUnit.Framework;

namespace AT
{
    public partial class TestBase
    {
        private bool _dbOk = true;
        private bool _browserOk = true;


        /// <summary>
        /// Проверка возможности запустить на выполнение текущий тест-кейс
        /// </summary>
        private void CheckPossibilityOfStarting()
        {
            Assertion(
                "Тест-кейс не выполнялся, т.к. были ошибки при инициализации " +
                "[DB: " + DbOk + "; " +
                "Browser: " + BrowserOK + "]", () => Assert.IsTrue(CanStart));

            TestStatus = TestStatuses.NotCompleted;
        }

        private bool CanStart
        {
            get { return DbOk && BrowserOK; }
        }


        private bool DbOk
        {
            get { return _dbOk; }
            set { _dbOk = value; }
        }

        private bool BrowserOK
        {
            get { return _browserOk; }
            set { _browserOk = value; }
        }


        private bool CanContinue
        {
            get { return TestStatus == TestStatuses.Failed ? IsContinueOnStepFail : true; }
        }
    }
}
