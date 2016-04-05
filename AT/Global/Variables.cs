using System;
using System.Collections.Generic;
using System.IO;
using AT.DataBase;
using AT.Service;

namespace AT.Global
{
    public static class GlobalVariables
    {
        public static int ErrorsCount = 0;

        public static Dictionary<string, TestStatuses> Tests = new Dictionary<string, TestStatuses>();
                                                       /* список тестов */

        internal static Dictionary<int, Error> Errors = new Dictionary<int, Error>(); /* список ошибок */


        public static string StepPrefix = Config.GetStringParam("test_step_prefix");
        public static string TestCasePrefix = Config.GetStringParam("test_case_prefix");
        public static string DateTimeFormat = Config.GetStringParam("datetime_string_format");
        public static string LogFileName = Config.GetStringParam("log_file_name");

        public static string DashPrefix = Config.GetStringParam("dash_prefix"); /* обозначение дефиса  */

        public static string ExtensionPrefix = Config.GetStringParam("extension_prefix");
                             /* обозначение расширения файла страницы */

        public static string FolderIndexPrefix = Config.GetStringParam("folder_index_prefix");
                             /* обозначение пустого имени страницы в папке (когда нужно открыть сайт/страница1/ ) */


        public static string CurrentTest = string.Empty; /* ид текущего теста */

        /* Browser_begin */

        public static string BrowserCheckUrl = Config.GetStringParam("browser_check_url");
                             /* адрес для проверки работы браузера */

        /* Browser_end */

        /* DB_begin */

        internal static List<DB> DbList = new List<DB>();
        public static string DbNamePrefix = Config.GetStringParam("database_name_prefix");
        public static int SelectedRowsLimit = Config.GetIntParam("database_selected_rows_limit");


        /* DB_end */

        public static string LogFileFullName
        {
            get
            {
                try
                {
                    string _logFileFullName;
                    var fileList = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\", LogFileName);

                    if (fileList.Length == 0) // если файла нет
                    {
                        _logFileFullName = Directory.GetCurrentDirectory() + "\\" + LogFileName;
                        File.Create(_logFileFullName).Close();
                    }
                    else // файл есть
                    {
                        var logFileFullName = fileList[0];
                        var fi = new FileInfo(logFileFullName);
                        var diff = (DateTime.Now - fi.CreationTime).Days;
                        // разница в днях между датой создания и сегодняшней датой

                        if (diff >= 7)
                        {
                            var newPartOfLogFileFullName = "_" + fi.CreationTime.ToShortDateString() + "-" +
                                                           DateTime.Now.ToShortDateString() +
                                                           ".log";
                            var newLogFileFullName = logFileFullName.Replace(".log", newPartOfLogFileFullName);


                            File.Move(logFileFullName, newLogFileFullName);
                            File.Create(logFileFullName).Close();
                            File.SetCreationTime(logFileFullName, DateTime.Now);
                        }

                        _logFileFullName = logFileFullName;
                    }

                    return _logFileFullName;
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return string.Empty;
                }

            }
        }
    }
}
