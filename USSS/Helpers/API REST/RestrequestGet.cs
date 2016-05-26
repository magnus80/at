//using learncsharp.Service;
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
    public class RestrequestGet
    {
        protected HttpWebRequest request = null;

        protected HttpWebResponse response = null;

        protected HttpStatusCode statusCode;

        private static string token = new TokenHashRestAPI().GetSystemTokenREST();

        public Meta outputt = null;

        public string outputstr;

        public void RestRequestGet(string URL, string paramToken = "default")
        {
            if(paramToken.Equals("default"))
                paramToken = token;
            Cookie cookie = new Cookie("token", paramToken, "", "dev-web01");
            //создаём запрос
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "GET";
                request.Accept = "application/json";
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
}
