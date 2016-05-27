using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FTTB.Helpers;

namespace FTTB.API
{
    class FinInfo
    {
        // описываем структуру по которой будем десериализовать возвращаемый json 
        public class RootObject
        {
            public ResponseMeta meta { get; set; }
            public int blockType { get; set; }
            public string blockDate { get; set; }
            public string bcEndDate { get; set; }
            public double monthlyPayment { get; set; }
            public double nextBCSum { get; set; }
            public bool promisedPaymentAllowed { get; set; }
            public string finBlockStartDate { get; set; }
            public string blockDateEnd { get; set; }
            public int blockMaxLong { get; set; }
            public int countUnread { get; set; }
            public int billType { get; set; }
        }

        public class ResponseMeta
        {
            public string status { get; set; }
            public int code { get; set; }
            public object message { get; set; }
        }

       

        public struct FinInfoResponse
        {
            public String request;
            public String all;
            public String code;
            public String message;
            public int blockType;
            public DateTime blockDate;
            public DateTime bcEndDate;
            public double monthlyPayment;
            public double nextBCSum;
            public bool promisedPaymentAllowed;
            public DateTime finBlockStartDate;
            public DateTime blockDateEnd;
            public int blockMaxLong;
            public int countUnread;
            public int billType;
        }


        public static FinInfoResponse finInfo(string token)
        {
            // Создаем переменные
            WebRequest Request;
            WebResponse Response;
            StreamReader Reader;
            FinInfoResponse finInfoResponse = new FinInfoResponse();

            // Создаем запрос "GET"
            Request = WebRequest.Create(ReaderTestData.ReadCExel(13, 7) + "/api/1.0/inac/account/finInfo");
            Request.Method = "GET";
            ((HttpWebRequest)Request).UserAgent = "Apache-HttpClient/4.2.3";
            ((HttpWebRequest)Request).KeepAlive = true;
            ((HttpWebRequest)Request).Proxy = null; //Ставим обязательно, это также ускоряет подключение.
            ((HttpWebRequest)Request).Referer = ReaderTestData.ReadCExel(13, 7);
            //((HttpWebRequest)Request).ContentType = "application/json";
            CookieContainer cookies = new CookieContainer();
            Cookie cookie = new Cookie("token", token, "", "usssfttb-test.vimpelcom.ru");
            cookies.Add(cookie);
            ((HttpWebRequest)Request).CookieContainer = cookies;


            finInfoResponse.request = Request.RequestUri.ToString();


            Response = Request.GetResponse();
            Reader = new StreamReader(Response.GetResponseStream(),
            Encoding.GetEncoding("utf-8"));

            // Ставим true/false для перенаправления на другую страницу. Но нежелательно, т.к. могут потеряться куки, надо будет следить за каждым запросом
            ((HttpWebRequest)Request).AllowAutoRedirect = false;

            // Создаем переменную для страницы
            string response = Reader.ReadToEnd();
            finInfoResponse.all = response;
            // string json =
            //     @"{""meta"":{""status"":""OK"",""code"":20000,""message"":null},""token"":""6111B5A3B91C717CA775D96FC5DE1885"",""tempPassInd"":false,""newUserInd"":false}";

            //десериализуем json, все данные будут в полях класса apiresponse
            RootObject apiresponse = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<RootObject>(response);



            finInfoResponse.code = apiresponse.meta.code.ToString(); 
            if (apiresponse.meta.message != null)
            {
                finInfoResponse.message = apiresponse.meta.message.ToString();
            }
            DateTime.TryParse(apiresponse.bcEndDate,out finInfoResponse.bcEndDate);
            finInfoResponse.billType = apiresponse.billType;
            DateTime.TryParse(apiresponse.blockDate, out finInfoResponse.blockDate);
            DateTime.TryParse(apiresponse.blockDateEnd, out finInfoResponse.blockDateEnd);
            finInfoResponse.blockMaxLong = apiresponse.blockMaxLong;
            finInfoResponse.blockType = apiresponse.blockType;
            finInfoResponse.countUnread = apiresponse.countUnread;
            DateTime.TryParse(apiresponse.finBlockStartDate, out finInfoResponse.finBlockStartDate) ;
            finInfoResponse.monthlyPayment = apiresponse.monthlyPayment;
            finInfoResponse.nextBCSum = apiresponse.nextBCSum;
            finInfoResponse.promisedPaymentAllowed = apiresponse.promisedPaymentAllowed;
            // Закрываем поток, освобождаем память
            Reader.Close();
            Response.Close();
            return finInfoResponse;
        }

        public static string finInfoCheckDate(string login, FinInfoResponse finInfoResponse)
        {
            string errorText = "";
            bool isError = false;
            if (isError)
            {
                return errorText;
            }
            else
            {
                return "success";
            }
        }

    }
}
