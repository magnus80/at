using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using USSS.Helpers;
using USSS.SubscriberInfoService;
using Newtonsoft.Json;

namespace USSS.Helpers.API_REST
{
    public class RestRequestPut
    {
        protected HttpWebRequest request = null;

        protected HttpWebResponse response = null;

        protected HttpStatusCode statusCode;

        private static string token = new TokenHashRestAPI().GetSystemTokenREST();

        public Meta outputt = null;

        FeatureParameteres fp1 = new FeatureParameteres();     
        fp1.

        string jLoginString = JsonConvert.SerializeObject(featureParameters);

        public RestRequestPut(string URL, string paramToken = "default")
        {
            string values = "{featureParameters:[{feature:FREEMI,paramName:IMEI,paramValue:654635432}]}";
           // string requestPayload = "{AddressLine1:'1600 1st St.'}";

            string values1 = "{\"featureParameters\":[{\"feature\":\"FREEMI\",\"paramName\":\"IMEI\",\"paramValue\":\"654635432\"}]}";
            string a = values;
            string B = values1;
            //{ "featureParameters": [{"feature":"FREEMI","paramName":"IMEI","paramValue":"654635432"}]}
            var bytes = Encoding.ASCII.GetBytes(values);
            if(paramToken.Equals("default"))
                paramToken = token;
            Cookie cookie = new Cookie("token", paramToken, "", "dev-web01");


            //создаём запрос           
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);                
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Accept= "application/json";
                UTF8Encoding encoding = new UTF8Encoding();
                request.ContentLength = encoding.GetByteCount(values);
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookie);
                
            }
            catch (UriFormatException we)
            {
                Logger.PrintStr("url uncorrect : " + we.Message);
            }
            //получаем ответ
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException we)
            {
                response = (HttpWebResponse)we.Response;
            }

            statusCode = response.StatusCode;
            StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(65001));
            StringBuilder output = new StringBuilder();
            output.Append(myStreamReader.ReadToEnd());
            outputstr = output.ToString();
            response.Close();

            outputt = new Meta(output.ToString());
        }

    }

    public class FeatureParameteres
    {   public string feature { get; set; }
        public string paramName { get; set; }
        public string paramValue { get; set; }
    }
}
