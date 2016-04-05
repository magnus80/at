using System;
using System.Linq;
using AT.DataBase;
using AT.Global;
using AT.Service;
using AT.WebDriver;
using Microsoft.Office.Interop.Excel;
using Action = System.Action;


namespace AT
{
    public partial class TestBase
    {
        #region init

        private void Init()
        {
            GlobalVariables.CurrentTest = Id = GetType().Name.Replace(GlobalVariables.TestCasePrefix, "");
            TestStatus = TestStatuses.NotStarted;
        }

        private void InitDbList()
        {
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData.xls");
            Worksheet excelSheet = wb.ActiveSheet;
            string d = excelSheet.Cells[4, 8].Value.ToString();
            wb.Close();

                var db = new DB(d);
                db.OpenConnection();

                if(db.Connected)
                    GlobalVariables.DbList.Add(db);
                else
                {
                    DbOk = false;
                }
            
        }

        private void InitBrowser()
        {
            try
            {
                Browser.Navigate(GlobalVariables.BrowserCheckUrl);
            }
            catch (Exception ex)
            {
                Browser.ReinitBrowser(GlobalVariables.BrowserCheckUrl);
                try
                {
                    Browser.Navigate(GlobalVariables.BrowserCheckUrl);
                }
                catch (Exception ex1)
                {
                    BrowserOK = false;
                    GlobalEvents.ExeptionFounded(ex1);
                }
                
            }
        }

        private void CloseDbConnection()
        {
            foreach (var db in GlobalVariables.DbList)
            {
                db.CloseConnection();
            }
        }

        private void CloseBrowser()
        {
            
        }

        #endregion

        private void AddTestToTestsList()
        {
            //GlobalVariables.Tests.Add(Id,
            //                          TestStatus == TestStatuses.NotCompleted
            //                              ? TestStatuses.Passed
            //                              : TestStatuses.Failed);
            /* если по окончании выполнения всех шагов статус тест-кейса не стал Failed, кейс пройден успешно (Passed) */
        }

        public void Assertion(string errorMessage, Action assert)
        {
            //AssertsAccumulator.Accumulate(this, errorMessage, assert);
        }

        static internal void KillBrowser()
        {
            try
            {
                System.Diagnostics.Process.GetProcesses()
               .Where(i => i.ProcessName.Equals(Config.GetStringParam("browser")) || i.ProcessName.Equals("IEDriverServer"))
               .ToList()
               .ForEach(i => i.Kill());
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }
    }
}
