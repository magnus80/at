using System;
using AT.Service;

namespace AT.Global
{
    internal static class GlobalEvents
    {
        private static event Action<string> ExeptionFoundEvent = delegate { };
        private static event Action<string> AssertionFailEvent = delegate { };
        private static event Action<string> ErrorFoundEvent = delegate { };

        private static bool IsEventsInitialized = false;
    
        private static void Init()
        {
            AssertionFailEvent += Logger.AssertionFailed;
            AssertionFailEvent += AssertsAccumulator.AddErrorToErrorList;
            AssertionFailEvent += AssertsAccumulator.FailTest;

            ExeptionFoundEvent += Logger.Write;

            ErrorFoundEvent += Logger.Write;

            IsEventsInitialized = true;
        }

        public static void ExeptionFounded(Exception ex)
        {
            if (IsEventsInitialized == false) 
                Init();
            ExeptionFoundEvent(ex.Message);
        }

        public static void ErrorFounded(string errorText)
        {
            if (IsEventsInitialized == false)
                Init();
            ErrorFoundEvent(errorText);
        }
    
        public static void AssertionFailed(string failMessage)
        {
            if (IsEventsInitialized == false) 
                Init();
            AssertionFailEvent(failMessage);
        }
    }
}
