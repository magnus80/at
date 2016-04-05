//author: ngadiyak

using System;
using AT.Service;
using NUnit.Framework;

namespace AT
{
    public enum TestStatuses
    {
        [System.ComponentModel.Description("Passed")]
        Passed,

        [System.ComponentModel.Description("Failed")]
        Failed,

        [System.ComponentModel.Description("NotCompleted")]
        NotCompleted,

        [System.ComponentModel.Description("NotStarted")]
        NotStarted,

        [System.ComponentModel.Description("Broken")]
        Broken
    }

    /// <summary>
    /// Базовый класс тест-кейсов
    /// </summary>
    public partial class TestBase
    {
        public TestStatuses TestStatus;
        public string Id;
        public bool IsContinueOnStepFail; /* продолжать тест-кейс если найдена ошибка в степе, по умолчанию false */

        public event Action TestSetUp = () => { };      /* () => {} -- Создание анонимного метода и создание на его основе делегата подходящего типа. */
        public event Action TestTearDown = () => { };
        public event Action StepSetUp = () => { };
        public event Action StepTearDown = () => { };
        
        /// <summary>
        /// Конструтор, подписываемся на события
        /// </summary>
        public TestBase()
        {
            IsContinueOnStepFail = false;

            TestSetUp += Init;
            TestSetUp += CloseDbConnection;
            TestSetUp += CloseBrowser;
            TestSetUp += InitDbList;
            if (Environment.GetEnvironmentVariable("teamcity") == null)
            {
                //TestSetUp += InitBrowser;
                TestSetUp += KillBrowser;
            }
            else
            {
                //TestSetUp += InitBrowser;
            }
            TestSetUp += CheckPossibilityOfStarting;
            TestSetUp += Logger.TestSetUp;
            
            StepSetUp += Logger.StepSetUp;
            StepTearDown += Logger.StepTearDown;

            TestTearDown += AddTestToTestsList;
            TestTearDown += Logger.TestTearDown;
        }

        /// <summary>
        /// Запуск тест-кейса
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            TestSetUp();
        }

        /// <summary>
        /// Запуск степа
        /// </summary>
        [SetUp]
        public void SetUp()
        {
          //  Assertion("Шаг не выполнялся, ошибка на предыдущем шаге", () => Assert.IsTrue(CanContinue));
            StepSetUp();
        }

        /// <summary>
        /// Завершение степа
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            StepTearDown();
        }

        /// <summary>
        /// Завершение тест-кейса
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            TestTearDown();
        }

        /// <summary>
        /// Деструктор, отписываемся от событий
        /// </summary>
        ~TestBase()
        {
            TestSetUp -= Init;
            TestSetUp -= CloseDbConnection;
            TestSetUp -= CloseBrowser;
            TestSetUp -= InitDbList;
            if (Environment.GetEnvironmentVariable("teamcity") == null)
            {
               TestSetUp -= KillBrowser;
            }
            else
            {
                TestSetUp -= InitBrowser;
            }
            TestSetUp -= CheckPossibilityOfStarting;
            TestSetUp -= Logger.TestSetUp;

            StepSetUp -= Logger.StepSetUp;
            StepTearDown -= Logger.StepTearDown;

            TestTearDown -= AddTestToTestsList;
            TestTearDown -= Logger.TestTearDown;
        }
    }
}
