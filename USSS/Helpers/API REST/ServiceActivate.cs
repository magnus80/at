using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FTTB.API;
using Newtonsoft.Json;
using USSS.Helpers.API_REST;
using USSS.Helpers;
using org.bouncycastle.asn1.ocsp;
using USSS.AuthSoap;
using USSS.SubscriberInfoService;

namespace USSS.Helpers.API_REST
{
    public static class ServiceActivate
    {
        public class RootObject
        {
            public ServiceActivate.ApiResponse meta { get; set; }
            public string requestId { get; set; }
            //public bool tempPassInd { get; set; }
            //public bool newUserInd { get; set; }
        }

        public class ApiResponse
        {
            public string status { get; set; }
            public int code { get; set; }
            public object message { get; set; }
        }

        public struct ServiceActivateResponse
        {
            public String request;
            public String all;
            public String code;
            public String message;
            public String requestId;
        }


        public static ServiceActivateResponse serviceActivate(string ctn)
        {
            // Создаем переменные
            WebRequest Request;
            WebResponse Response;
            StreamReader Reader;
            ServiceActivateResponse serviceActivateResponse = new ServiceActivateResponse();

            // Создаем запрос "PUT"

            Request = WebRequest.Create(ReaderTestData.ReadCExel(12,7) + "/api/1.0/request/serviceActivate?ctn=" + ctn+ "&serviceName=FREEM_200");
            Request.Method = "PUT";
            ((HttpWebRequest) Request).UserAgent = "Apache-HttpClient/4.2.3";
            ((HttpWebRequest) Request).KeepAlive = true;
            ((HttpWebRequest) Request).Proxy = null; //Ставим обязательно, это также ускоряет подключение.
            ((HttpWebRequest) Request).Referer = ReaderTestData.ReadCExel(12, 7);
            ((HttpWebRequest) Request).ContentType = "application/json";

            string data = @"{'featureParameters':[{'feature':'FREEMI','paramName':'IMEI','paramValue':'654635432'}]}";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] arr = encoding.GetBytes(data);
            ((HttpWebRequest) Request).ContentLength = arr.Length;
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(arr, 0, arr.Length);
            dataStream.Close();

            serviceActivateResponse.request = Request.RequestUri.ToString();

            Response = Request.GetResponse();
            Reader = new StreamReader(Response.GetResponseStream(),
                Encoding.GetEncoding("utf-8"));

            // Ставим true/false для перенаправления на другую страницу. Но нежелательно, т.к. могут потеряться куки, надо будет следить за каждым запросом
            ((HttpWebRequest) Request).AllowAutoRedirect = false;

            // Создаем переменную для страницы
            string response = Reader.ReadToEnd();
            serviceActivateResponse.all = response;
            // string json =
            //     @"{""meta"":{""status"":""OK"",""code"":20000,""message"":null},""token"":""6111B5A3B91C717CA775D96FC5DE1885"",""tempPassInd"":false,""newUserInd"":false}";

            //десериализуем json, все данные будут в полях класса apiresponse
            AuthAuth.RootObject apiresponse =
                new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<AuthAuth.RootObject>(response);


            serviceActivateResponse.code = apiresponse.meta.code.ToString(); //response.Substring(30, 5);
            serviceActivateResponse.requestId = apiresponse.token;
            if (apiresponse.meta.message != null)
            {
                serviceActivateResponse.message = apiresponse.meta.message.ToString();
            }

            // Закрываем поток, освобождаем память
            Reader.Close();
            Response.Close();
            return serviceActivateResponse;
        }

    }
}
    

