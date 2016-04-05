using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AT.Service
{
    public static class Config
    {
        /// <summary>
        /// Выбирает из конфига необходимый параметр
        /// </summary>
        /// <param name="paramName">название параметра</param>
        /// <returns>значение параметра</returns>
        public static string GetStringParam(string paramName)
        {
            return ConfigurationManager.AppSettings[paramName];
        }

        /// <summary>
        /// Выбирает из конфига необходимый параметр
        /// </summary>
        /// <param name="paramName">название параметра</param>
        /// <returns>значение параметра</returns>
        public static int GetIntParam(string paramName)
        {
            return int.Parse(ConfigurationManager.AppSettings[paramName]);
        }

        /// <summary>
        /// Возвращает из конфига список значений параметров, названия которых начинаются с startsWith
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns>список значений параметров</returns>
        public static List<string> GetStringParamStartsWith(string startsWith)
        {
            return
                (from k in ConfigurationManager.AppSettings.AllKeys
                 where k.StartsWith(startsWith)
                 select new {k}).Select(key => GetStringParam(key.k)).ToList();
        }
    }
}
