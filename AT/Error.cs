using System;

namespace AT
{
    internal class Error
    {
        public string Test { get; set; }
        public string Step { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public Error(string test, string step, string message)
        {
            Test = test;
            Step = step;
            Message = message;
            Date = DateTime.Now; // формат даты .ToString("yyyy-MMM-dd -> hh:mm:ss")
        }

    }
}
