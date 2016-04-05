using System.IO;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.Helpers
{
   static  class ReaderTestData
    {
        public static string ReadExel(string nameTest, string nameParam)
        {
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData2.xls");
            Worksheet excelSheet = wb.ActiveSheet;
            try
            {
                int i = 3;
               
                while (true)
                {
                    try
                    {
                        string d = excelSheet.Cells[i, 1].Value;
                        if (d == nameTest)
                        {
                            d = null;
                            while (d == null)
                            {

                                string m = excelSheet.Cells[i, 2].Value;
                                if (m == nameParam)
                                {
                                    try
                                    {
                                        string s = excelSheet.Cells[i, 3].Value.ToString();
                                        wb.Close();
                                        return s;
                                    }
                                    catch (Exception)
                                    {
                                        wb.Close();
                                        return null;
                                    }
                                   

                                }
                                i = i + 1;
                                d = excelSheet.Cells[i, 1].Value;
                            }
                            wb.Close();
                            return null;
                        }
                    }

                    catch
                    {

                        //string m = excelSheet.Cells[i - 1, 2].Value.ToString();
                        //if (m == nameParam)
                        //{
                        //    string s = excelSheet.Cells[i - 1, 3].Value.ToString();
                        //    wb.Close();
                        //    return s;

                        //}


                    }
                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                wb.Close();
                return ex.Message;
            }
        }

        public static string ReadCExel(int i,int j)
        {
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData2.xls");
            Worksheet excelSheet = wb.ActiveSheet;
            string d = excelSheet.Cells[i, j].Value.ToString();
            wb.Close();
            return d;
        }

        public static string GetXMLTestData(string tagname)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/TestData/TestData.xml");
            return xmlDoc.SelectSingleNode("USSS/" + tagname).InnerText;
        }
   
    }
}
