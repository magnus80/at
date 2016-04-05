using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;

namespace USSS.Helpers.API_REST
{
    public class RestRequestPut
    {
        protected HttpWebRequest request = null;

        protected HttpWebResponse response = null;

        protected HttpStatusCode statusCode;

        private static string token = new TokenHashRestAPI().GetSystemToken2();

        public Meta outputt = null;

        public string outputstr;

        public RestRequestPut(string URL, string paramToken = "default")
        {
            string values = @"{featureParameters: [feature={FREEMI}&paramName={IMEI}&paramValue={654635432}]}";
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
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookie);
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
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


}
