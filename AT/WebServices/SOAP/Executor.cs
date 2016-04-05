using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace AT.WebServices.SOAP
{
    public static class SoapExecutor
    {
        public static Dictionary <string, string> Results = new Dictionary<string, string>();

        #region private

        #region request

        private static void RecursiveFormRequestBody(ref XElement current)
        {
            foreach (var el in SoapParamList.List)
            {
                if (SoapParamList.GetElementName(el.ParentId) == current.Name.ToString())
                {
                    var xel = new XElement(el.Name);
                    xel.Value = el.Value;
                    current.Add(xel);
                    RecursiveFormRequestBody(ref xel);
                }
            }
        }

        private static void Wait()
        {
            System.Threading.Thread.Sleep(5000);
        }

        private static string RequestBody
        {
            get
            {
                var main = new XElement(SoapParamList.List[0].Name);
                RecursiveFormRequestBody(ref main);

                SoapParamList.Clear();

                return new Regex(@"__\d+").Replace(main.ToString(), "");
            }
        }

        #endregion

        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest =
                (HttpWebRequest) WebRequest.Create(url);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        #endregion

        /// <summary>
        /// выполняет SOAP запрос
        /// </summary>
        /// <param name="url">адрес сервиса</param>
        /// <param name="requestBegin">заголовок запроса (до названия функции)</param>
        /// <param name="requestEnd">подвал запроса (после тега Body)</param>
        /// <returns>результат выполнения запроса</returns>
        public static bool Execute(string url, string requestBegin, string requestEnd)
        {
            try
            {
                Results.Clear();

                HttpWebRequest request = CreateWebRequest(url);
                XmlDocument soapEnvelopeXml = new XmlDocument();

                soapEnvelopeXml.LoadXml(requestBegin + RequestBody + requestEnd);

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                
                var doc = new XmlDocument();

                using (WebResponse response = request.GetResponse())
                {
                    using (var rd = new StreamReader(response.GetResponseStream()))
                    {
                        doc.LoadXml(rd.ReadToEnd());
                    }
                }

                foreach (XmlNode t in doc.SelectNodes("//*[text()]"))
                {
                    Results.Add(t.Name, t.InnerText);
                }

                Wait();
                return true;
            }
            catch (Exception ex)
            {
                Global.GlobalEvents.ExeptionFounded(ex);
                return false;
            }
        }
    }
}
