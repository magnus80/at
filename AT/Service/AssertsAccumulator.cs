using System;
using System.Diagnostics;
using System.Linq;
using AT.Global;
using NUnit.Framework;

namespace AT.Service
{
    public class AssertsAccumulator
    {
        private static object _testCase;
        private static string _failMessage;

        private static string Test
        {
            get
            {
                try
                {
                    return _testCase.GetType().GetField("Id").GetValue(_testCase).ToString();
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return string.Empty;
                }
            }
        }

        private static string Step
        {
            get
            {
                try
                {
                    return
                        (new StackTrace(true).GetFrames()
                            .Where(frame => frame.GetMethod().Name.StartsWith(GlobalVariables.StepPrefix))
                            .Select(frame => frame.GetMethod().Name)).First();
                }
                catch (Exception)
                {
                    GlobalEvents.ErrorFounded(
                        "Ошибка: не найден номер шага в stacktrace. Возникает, когда появляется 'ошибка на предыдущем шаге', т.к. метод, описывающий шаг, не запускался, и соот-но его нет в stacktrace");

                    return "След. шаг";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testCase"></param>
        /// <param name="errorMessage"></param>
        /// <param name="assert"></param>
        internal static void Accumulate(object testCase, string errorMessage, Action assert)
        {
            try
            {
                assert.Invoke();
            }
            catch (Exception)
            {
                _testCase = testCase;
                _failMessage = Test + " : " + Step + " : " + errorMessage;

                GlobalEvents.AssertionFailed(_failMessage);
            }
        }


        /// <summary>
        /// "фэйлит" тест
        /// </summary>
        /// <param name="errorMessage">текст ошибки</param>
        internal static void FailTest(string errorMessage)
        {
            try
            {
                _testCase.GetType().GetField("TestStatus").SetValue(_testCase, TestStatuses.Failed);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
            Assert.Fail(_failMessage);
        }

        /// <summary>
        /// Добавление ошибки в список
        /// </summary>
        /// <param name="errorMessage">текст ошибки</param>
        internal static void AddErrorToErrorList(string errorMessage)
        {
            GlobalVariables.Errors.Add(GlobalVariables.ErrorsCount++, new Error(Test, Step, errorMessage));
        }
    }
}
