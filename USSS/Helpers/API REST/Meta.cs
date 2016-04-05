using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.Helpers
{
    public class Meta
    {
        string[] ArrayMeta;

        public string Status
        {
            get 
            {
                if (ArrayMeta.Length > 2) return ArrayMeta[2];
                else return null;
            }
        }
        public string Code
        {
            get 
            {
                if (ArrayMeta.Length > 4) return ArrayMeta[4];
                else return null;
            }

        }
        public string Message
        {
            get 
            {
                if (ArrayMeta.Length > 6) return ArrayMeta[6];
                else return null;
            }
        }
        public string conflictType
        {
            get
            {
                for (int i = 6; i < ArrayMeta.Length-1; i++)
                {
                    if (ArrayMeta[i].Equals("conflictType"))
                        return ArrayMeta[i + 1];
                }
                return null;
            }
        }
        public string name
        {
            get
            {
                for (int i = 6; i < ArrayMeta.Length-1; i++)
                {
                    if (ArrayMeta[i].Equals("name"))
                        return ArrayMeta[i + 1];
                }
                return null;
            }
        }
        public string entityName
        {
            get
            {
                for (int i = 6; i < ArrayMeta.Length-1; i++)
                {
                    if (ArrayMeta[i].Equals("entityName"))
                        return ArrayMeta[i + 1];
                }
                return null;
            }
        }
        public string template
        {
            get
            {
                for (int i = 6; i < ArrayMeta.Length-1; i++)
                {
                    if (ArrayMeta[i].Equals("template"))
                        return ArrayMeta[i + 1];
                }
                return null;
            }
        }
        public string templateText
        {
            get
            {
                for (int i = 6; i < ArrayMeta.Length-1; i++)
                {
                    if (ArrayMeta[i].Equals("templateText"))
                        return ArrayMeta[i + 1];
                }
                return null;
            }
        }
        public string Find_Data(string param)
        {
            for (int i = 6; i < ArrayMeta.Length-1; i++)
            {
                if (ArrayMeta[i].Equals(param))
                    return ArrayMeta[i + 1];
            }
            return null;
        }

        public Meta(string response = null)
        {
            if (response == null)
            {
                    ArrayMeta = new string[] {""};
            }
            else
            {
                ArrayMeta = response.Split(new Char[] { '\"', '{', ':', '}', ',', '[', ']' },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
