using System;
using AT.Global;
using Exc = Microsoft.Office.Interop.Excel;

namespace AT.Tools
{
    public class ExcelWorker
    {
        private Exc.Application ObjExcel = new Exc.Application();
        private Exc.Workbook ObjWorkBook;
        private Exc.Worksheet ObjWorkSheet;

        public ExcelWorker(string filePath)
        {
            ObjWorkBook = ObjExcel.Workbooks.Open(filePath, 0, false, 5, "", "", false, Exc.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            ObjWorkSheet = (Exc.Worksheet)ObjWorkBook.Sheets[1];
        }

        public ExcelWorker(string filePath, int sheetNumber)
        {
            ObjWorkBook = ObjExcel.Workbooks.Open(filePath, 0, false, 5, "", "", false, Exc.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            ObjWorkSheet = (Exc.Worksheet)ObjWorkBook.Sheets[sheetNumber];
        }

        public void SaveAndClose()
        {
            Save();
            Close();
        }

        public void ChangeSheet(int index)
        {
            try
            {
                ObjWorkSheet = (Exc.Worksheet)ObjWorkBook.Sheets[index];
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }   
        }

        public void Save()
        {
            ObjWorkBook.Save();
        }

        public void SaveAs(string path)
        {
            ObjWorkBook.SaveAs(path);
        }

        public void Close()
        {
            ObjExcel.Quit();
        }

        /*cell - это номер ячейки в 
            стандартном Excel формате
            Например A1*/
        public string GetCell(string cell)
        {
            Exc.Range range = ObjWorkSheet.get_Range(cell);
            return range.Text.ToString();
        }

        public void SetCell(string cell, string value)
        {
            Exc.Range range = ObjWorkSheet.get_Range(cell);
            range.Value = value;
        }
    }
}
