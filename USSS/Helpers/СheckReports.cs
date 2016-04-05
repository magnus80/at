using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.IO;
using System.Reflection;
using iTextSharp.text.pdf;


namespace USSS.Helpers
{

    class СheckReports
    {
        public bool CheckDetalizationXLSReport(string path, List<string> currentReport)
        {
            //Application excel = new Application();
            //Workbook wb = excel.Workbooks.Open(path);
            //Worksheet excelSheet = wb.ActiveSheet;


            List<string> strs = new List<string>();
            //Read the first cell
            //string test = excelSheet.Cells[4, 1].Value.ToString();
            //foreach (var qwe in excelSheet.Cells)
            //{
            //    strs.Add(qwe.ToString());
            //}


            //wb.Close();


            try
            {
                Application app = new Application();
                app.Visible = false;
                app.ScreenUpdating = false;
                app.DisplayAlerts = false;

                Workbook book = app.Workbooks.Open(path,
                                                   Missing.Value, Missing.Value, Missing.Value,
                                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                   Missing.Value, Missing.Value, Missing.Value);

                Worksheet sheet = (Worksheet) book.Worksheets[1];
                Range last = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                Range range = sheet.get_Range("A1", last);

                string execPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().CodeBase);

                object[,] values = (object[,]) range.Value2;

                for (int i = 1; i <= values.GetLength(0); i++)
                {
                    for (int j = 1; j <= values.GetLength(1); j++)
                    {
                        if (values[i, j] != null)
                        {
                            strs.Add(values[i, j].ToString());
                        }
                    }
                }
            }
            catch
            {
            }
            return true;
        }
        
        public bool CheckPdfReport(string path)
        {
      
          //  string result = pdfParser.ExtractText(path, path);
            //Console.WriteLine(result);
            return true;
        }

        public bool CheckReportSOAPReport(string SoapRequest, List<string> currentReport)
        {
            //WebRequest webRequest = WebRequest.Create("http://localhost/AccountSvc/DataInquiry.asmx");
            //HttpWebRequest httpRequest = (HttpWebRequest)webRequest;
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "text/xml; charset=utf-8";
            //httpRequest.Headers.Add("SOAPAction: http://tempuri.org/" + methodName);
            //httpRequest.ProtocolVersion = HttpVersion.Version11;
            //httpRequest.Credentials = CredentialCache.DefaultCredentials;
            //Stream requestStream = httpRequest.GetRequestStream();
            ////Create Stream and Complete Request             
            //StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.ASCII);

            //StringBuilder soapRequest = new StringBuilder("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            //soapRequest.Append(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ");
            //soapRequest.Append("xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body>");
            //soapRequest.Append("<GetMyName xmlns=\"http://tempuri.org/\"><name>Sam</name></GetMyName>");
            //soapRequest.Append("</soap:Body></soap:Envelope>");

            //streamWriter.Write(soapRequest.ToString());
            //streamWriter.Close();
            ////Get the Response    
            //HttpWebResponse wr = (HttpWebResponse)httpRequest.GetResponse();
            //StreamReader srd = new StreamReader(wr.GetResponseStream());
            //string resulXmlFromWebService = srd.ReadToEnd();
            return true;
        }

        public bool CompareFilesCheck(string login,string password,string start,string end, string file2_path)
        {
        //    try
        //    {
        //        int i = 0;
        //        string s;
        //        StreamReader file1_sr = new StreamReader(file1_path);
        //        StreamReader file2_sr = null;
        //     //   CheckDateInArhiv("C:\\Users\\KPodberezin\\AppData\\Local\\Temp\\" + file2_path, "");
        //        try
        //        {
        //            file2_sr = new StreamReader("C:\\Users\\KPodberezin\\AppData\\Local\\Temp\\" + file2_path);
        //        }
        //        catch 
        //        {
        //            file2_sr = new StreamReader("C:\\Users\\KPodberezin\\Downloads\\" + file2_path);
        //        }
        //        while (!file1_sr.EndOfStream)
        //        {
        //            if (file2_sr.EndOfStream)
        //                return false;
        //            if (file1_sr.Read() != file2_sr.Read())
        //                i = i + 1;
        //            if (i > 500000)
        //                return false;
        //        }
        //        file1_sr.Dispose();
        //        file2_sr.Dispose();
                 
        //        return true;

        //    }
        //    catch
        //    {
        //        return false;
        //    }
            return true;
        }


         private bool CheckDateInArhiv(string path, string date)
        {
            string extractPath = @"c:\example\extract\"+date;
            ZipFile.ExtractToDirectory(path, extractPath);
            string fileName = Directory.GetFiles(extractPath)[0];
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open(fileName);
            Worksheet excelSheet = wb.ActiveSheet;
            
            string test = excelSheet.Cells[9, 2].Value.ToString();
           if (test == date)
           {
               wb.Close();
               return true;
           }
           wb.Close();
           return false;
            
             
        }
    }
}
