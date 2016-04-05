using System;
using System.Collections.Generic;
using System.Linq;

namespace AT.WebServices.SOAP
{
    public static class SoapParamList 
    {
        internal static List<Element> List = new List<Element>();
        internal static int Count;

        public static int AddParam(string name, int parent_id = -1, string value = "")
        {
            List.Add(new Element(name, parent_id, value));

            return LastId;
        }

        public static int LastId
        {
            get
            {
                if (List.Count > 0) return List[List.Count - 1].Id;
                return -1;
            }
        }

        public static string GetElementName(int id)
        {
            try
            {
                return List.First(el => el.Id == id).Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        internal static void Clear()
        {
            List.Clear();
            Count = 0;
        }
    }
}
