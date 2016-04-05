using System;
using System.Text;
using AT.WebDriver;

namespace AT.Tools
{
    public static class Other
    {
        /// <summary>
        /// Получает значение параметра из текущего URL браузера
        /// </summary>
        /// <param name="paramName">название параметра</param>
        /// <returns></returns>
        public static string GetParamFromCurrentUrl(string paramName)
        {
            try
            {
                var url = Browser.Url;
                if (url.IndexOf(paramName) != -1)
                {
                    url = url.Substring(url.IndexOf(string.Concat(paramName, "=")));
                    url = url.Replace(string.Concat(paramName, "="), "");
                    if (url.IndexOf('&') > -1)
                        url = url.Remove(url.IndexOf("&"));
                    return url;
                }
            }
            catch (Exception ex)
            {
                Global.GlobalEvents.ExeptionFounded(ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Смена кодировки строки
        /// </summary>
        /// <param name="encodingOld">кодировка "до"</param>
        /// <param name="encodindNew">кодировка "после"</param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string ChangeEncoding(int encodingOld, int encodindNew, string line)
        {
            var fromEncodind = Encoding.GetEncoding(encodingOld); //из какой кодировки
            var bytes = fromEncodind.GetBytes(line);
            var toEncoding = Encoding.GetEncoding(encodindNew); //в какую кодировку
           return toEncoding.GetString(bytes);

        }

        /// <summary>
        /// Возвращает значение переменной среды
        /// </summary>
        /// <param name="name">название переменной</param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name) ?? string.Empty;
        }
    }
}
