using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace USSS.Helpers
{
    internal class GetUrl
    {
        public string Url
        {
            get
            {
                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData2.xls");
                Worksheet excelSheet = wb.ActiveSheet;
                string d = excelSheet.Cells[1, 7].Value.ToString();
                wb.Close();
                return d;
            }
        }
    }
}
