using AT.WebDriver;
using Microsoft.Office.Interop.Excel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace USSS.Helpers.SOAP
{
    class CCBO
    {
        private XmlDocument GetDetails(string phoneNumber, string start, string end)
        {

            //CheckPdfReport("");
            string authInfo = "ECARE050914" + ":" + "qweasdzxc";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("http://pl-ccboapp-01:15000/ecare/?services=ODS_GET_DETAILS_FILE"));

            req.Method = "POST";
            req.Headers.Add("Accept-Encoding", "gzip,deflate");
            req.ContentType = "text/xml;charset=utf-8";
            req.Headers.Add("SOAPAction", "http://www.sap.com/ODS_GET_DETAILS_FILE");
            req.Headers.Add("Authorization", "Basic " + authInfo);//RUNBUkUwNTA5MTQ6cXdlYXNkenhj + authInfo);
            req.Host = "tst-ccbo.vimpelcom.ru:8210";
            req.KeepAlive = true;
            req.Accept = "text/xml";
            req.Credentials = new NetworkCredential("ECARE050914", "qweasdzxc", "http://pl-ccboapp-01:15000/ecare/?services=ODS_GET_DETAILS_FILE");
            req.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";
            req.PreAuthenticate = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">");
            sb.AppendLine(" <soapenv:Header/>");
            sb.AppendLine("  <soapenv:Body>");
            sb.AppendLine("  <urn:ODS_GET_DETAILS_FILE>");
            sb.AppendLine(" <I_BILL_SYS>23</I_BILL_SYS>");
            sb.AppendLine(" <I_DATE_FROM>2015-05-01</I_DATE_FROM>");
            sb.AppendLine(" <I_DATE_TO>2015-05-30</I_DATE_TO>");
            //sb.AppendLine(" <I_DATE_FROM>"+start+"</I_DATE_FROM>");
            //sb.AppendLine(" <I_DATE_TO>"+end+"</I_DATE_TO>");
            sb.AppendLine(" <I_FILTER>0</I_FILTER>");
            sb.AppendLine(" <I_FILTER_TECH>N</I_FILTER_TECH>");
            sb.AppendLine(" <I_FILTER_MAX_RECORDS>N</I_FILTER_MAX_RECORDS>");
            sb.AppendLine(" <I_SUBSC_NUM>9030335210</I_SUBSC_NUM>");
            sb.AppendLine(" <NEED_SPECIAL_ACCOUNT>N</NEED_SPECIAL_ACCOUNT>");
            sb.AppendLine(" <I_TYPE>3</I_TYPE>");
            sb.AppendLine(" </urn:ODS_GET_DETAILS_FILE>");
            sb.AppendLine(" </soapenv:Body>");
            sb.AppendLine("</soapenv:Envelope>");
            using (Stream stm = req.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(sb.ToString());
                }
            }

            XmlDocument xdoc = new XmlDocument();
            try
            {
                using (WebResponse response = req.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader srreader = new StreamReader(responseStream))
                            {
                                xdoc.Load(srreader);
                                string date = xdoc.GetElementsByTagName("E_DATA")[0].InnerXml;
                                return  DecoderData(date);
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse errRsp = ex.Response;
                using (Stream responseStream = errRsp.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader rdr = new StreamReader(responseStream))
                        {
                            string s = rdr.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        private XmlDocument DecoderData(string dataBase64)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(dataBase64);
            string s = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            XmlDocument xm = new XmlDocument();
            xm.LoadXml(s);
            int i = xm.GetElementsByTagName("record").Count;
            return xm;
        }

        public string ComparisonDet(string phoneNumber,string start,string end, string path)
        {
            Workbook wb = null;
            XmlDocument xm = GetDetails(phoneNumber, start, end);
            try
            {
                Application excel = new Application();
                wb = excel.Workbooks.Open(path);
                Worksheet excelSheet = wb.Worksheets.get_Item(2);
                int i = 3;
                int j = 0;
                while (true)
                {
                    int m = 1;
                    XmlNode xr = xm.GetElementsByTagName("record")[j].Clone().LastChild;
                    m = xr.ChildNodes.Count;
                    while (m != 0)
                    {
                        string d = excelSheet.Cells[i, 3].Value.ToString();
                        string x = ((XmlNode)(xm.GetElementsByTagName("record")[j])).ChildNodes[0].InnerText;// eCareDateTime.GetElementsByTagName
                        if (d != x)
                        {
                            return "Детализация не совпадает";
                        }
                        i = i + 1;
                        m = m - 1;
                    }
                    j = j + 1;
                }
            }
            catch (Exception )
            {
                wb.Close();
                return "Детализация совпадает";
            }         
        }

        public string ComparisonDet(string phoneNumber, string start, string end, WebElement ui)
        {
            
            XmlDocument xm = GetDetails(phoneNumber, start, end);
            try
            {
                int i = 1;
                int j = 0;
                while (true)
                {
                    int m = 1;
                    XmlNode xr = xm.GetElementsByTagName("record")[j].Clone().LastChild;
                    m = xr.ChildNodes.Count;
                    while (m != 0)
                    {
                        string d = ui.ByXPath("//div[@id='finInfoIndexPage:callDetailsPanel']//tbody[contains(@id,'finInfoIndexPage')]//tr[" + i + "]//td[1]").Text;
                        string x = ((XmlNode)(xm.GetElementsByTagName("record")[j])).ChildNodes[0].InnerText;// eCareDateTime.GetElementsByTagName
                        try
                        {
                            var qe = ((XmlNode)(xm.GetElementsByTagName("record")[j])).ChildNodes[3];//eCareOperator
                            if (qe.Name == "eCareOperator")
                            {
                                if (qe.InnerText != "")
                                {
                                    x = x + " " + qe.InnerText;
                                    try
                                    {
                                        while (x.IndexOf("  ") != -1)
                                        {
                                            x = x.Remove(x.IndexOf("  "), 1);
                                        }
                                       
                                    }
                                    catch (Exception)
                                    {
                                        
                                    } 
                                }
                            }

                        }
                        catch (Exception)
                        {
                            
                          
                        }
                        //eCareOperator
                        if (d != x)
                        {
                            return "Детализация не совпадает";
                        }
                        
                        m = m - 1;
                    }
                    i = i + 1;
                    j = j + 1;
                }

               
            }
            catch (Exception)
            {
                return "success";
            }
        }
    }
}
