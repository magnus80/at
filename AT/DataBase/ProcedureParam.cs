using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AT.DataBase
{
    public class ProcedureParam
    {
        private string _type, _name, _value;

        public bool IsReturnParam
        {
            get { return _name == null; }
        }

        public string Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }


        public ProcedureParam(string type, string name, string value)
        {
            _type = type;
            _name = name;
            _value = value;
        }

        public ProcedureParam(string type)
        {
            _type = type;
        }
    }
}
