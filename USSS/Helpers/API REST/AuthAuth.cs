using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;

namespace FTTB.API
{
     public static class AuthAuth
    {
        // описываем структуру по которой будем десериализовать возвращаемый json 
        public class RootObject
        {
            public ApiResponse meta { get; set; }
            public string token { get; set; }
            public bool tempPassInd { get; set; }
            public bool newUserInd { get; set; }
        }

        public class ApiResponse
        {
            public string status { get; set; }
            public int code { get; set; }
            public object message { get; set; }
        }

        public struct AuthResponse
        {
            public String request;
            public String all;
            public String code;
            public String message;
            public String token;
        }



        public static AuthResponse authAuth(string login, string password)
        {
            // Создаем переменные
            WebRequest Request;
            WebResponse Response;
            StreamReader Reader;
            AuthResponse authResponse = new AuthResponse();

            // Создаем запрос "POST"
            Request = WebRequest.Create(ReaderTestData.ReadCExel(13, 7)+"/api/auth/auth?login=" + login + "&userType=Fttb");
            Request.Method = "PUT";
            ((HttpWebRequest)Request).UserAgent = "Apache-HttpClient/4.2.3";
            ((HttpWebRequest)Request).KeepAlive = true;
            ((HttpWebRequest)Request).Proxy = null; //Ставим обязательно, это также ускоряет подключение.
            ((HttpWebRequest)Request).Referer = ReaderTestData.ReadCExel(13, 7); 
            ((HttpWebRequest)Request).ContentType = "application/json";

            string data = "{\"password\":\"" + password + "\"}";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] arr = encoding.GetBytes(data);
            ((HttpWebRequest)Request).ContentLength = arr.Length;
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();

            authResponse.request = Request.RequestUri.ToString();

            Response = Request.GetResponse();
            Reader = new StreamReader(Response.GetResponseStream(),
            Encoding.GetEncoding("utf-8"));

            // Ставим true/false для перенаправления на другую страницу. Но нежелательно, т.к. могут потеряться куки, надо будет следить за каждым запросом
            ((HttpWebRequest)Request).AllowAutoRedirect = false;

            // Создаем переменную для страницы
            string response = Reader.ReadToEnd();
            authResponse.all = response;
            // string json =
            //     @"{""meta"":{""status"":""OK"",""code"":20000,""message"":null},""token"":""6111B5A3B91C717CA775D96FC5DE1885"",""tempPassInd"":false,""newUserInd"":false}";

            //десериализуем json, все данные будут в полях класса apiresponse
            RootObject apiresponse = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<RootObject>(response);



            authResponse.code = apiresponse.meta.code.ToString(); //response.Substring(30, 5);
            authResponse.token = apiresponse.token;
            if (apiresponse.meta.message != null)
            {
                authResponse.message = apiresponse.meta.message.ToString();
            }

            // Закрываем поток, освобождаем память
            Reader.Close();
            Response.Close();
            return authResponse;
        }
    }
}
