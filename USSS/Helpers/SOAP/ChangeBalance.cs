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
    class ChangeBalance
    {
        public XmlDocument SetBalance(string phoneNumber, string cost)
        {
            ServicePointManager.ServerCertificateValidationCallback +=(sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(ReaderTestData.ReadCExel(2, 7)));

            req.Method = "POST";
            req.Headers.Add("Accept-Encoding", "gzip,deflate");
            req.ContentType = "text/xml;charset=utf-8";
            req.Accept = "text/xml";
            req.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";
            req.ProtocolVersion = HttpVersion.Version10;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:com=\"http://www.comverse.com\">");
            sb.AppendLine(" <soapenv:Header/>");
            sb.AppendLine("  <soapenv:Body>");
            sb.AppendLine("  <com:SubscriberAdjustBalanceInstance>");
            sb.AppendLine("  <com:input>");
            sb.AppendLine(" <userIdName>sapiuser</userIdName>");
            sb.AppendLine(" <securityToken>"+Aut()+"</securityToken>");
            sb.AppendLine(" <subscriberId>");
            sb.AppendLine("<subscriberId set=\"true\">");
            sb.AppendLine(" <value>" + phoneNumber + "</value>");
            sb.AppendLine("</subscriberId>");
            sb.AppendLine(" <subscriberExternalIdType set=\"true\">");
            sb.AppendLine(" <value>1</value>");
            sb.AppendLine(" </subscriberExternalIdType>");
            sb.AppendLine(" </subscriberId>");
            sb.AppendLine(" <balanceName>CORE BALANCE</balanceName>");
            sb.AppendLine(" <valueDelta>" + cost + "</valueDelta>");
            sb.AppendLine(" <CurrencyCode value=\"RUR\"/>");
            sb.AppendLine(" <mtrComment>SCRIPT_ADJUST</mtrComment>");
            sb.AppendLine(" </com:input>");
            sb.AppendLine("</com:SubscriberAdjustBalanceInstance>");
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

        private string Aut()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(ReaderTestData.ReadCExel(2, 10)));

            req.Method = "POST";
            req.Headers.Add("Accept-Encoding", "gzip,deflate");
            req.ContentType = "text/xml;charset=utf-8";
            req.Accept = "text/xml";
            req.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";
            req.ProtocolVersion = HttpVersion.Version10;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:auth=\"https://org.comverse.rtbd.sec/webservice/auth\">");
            sb.AppendLine(" <soapenv:Header/>");
            sb.AppendLine("  <soapenv:Body>");
            sb.AppendLine("  <auth:proxyLogin>");
            sb.AppendLine("  <String_1>sapiuser</String_1>");
            sb.AppendLine("  <String_2>sapipass1</String_2>");
            sb.AppendLine("  <String_3>SAPI</String_3>");
            sb.AppendLine("</auth:proxyLogin>");
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
                                string token = xdoc.GetElementsByTagName("result")[0].InnerXml;
                                token = token.Remove(0, token.IndexOf("Token&gt;") + 9);
                                token = token.Remove(token.IndexOf("&lt;/Token"));
                                return token;
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
    }
}
