using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.Services
{
    public class SParams
    {
        public class SetviceParam
        {
            public string Name;
            public string ParamChar;
            public string ParamNumber;


            public SetviceParam(string PC, string PN, string N)
            {
                Name = N;
                ParamChar = PC;
                ParamNumber = PN;
            }
        }

        public List<SetviceParam> Params = new List<SetviceParam>();

        public void Add(string PC, string PN, string N)
        {
            Params.Add(new SetviceParam(PC, PN, N));
        }

        public SetviceParam this[string name]
        {
            get
            {
                try
                {
                    foreach (var p in Params)
                    {
                        if (p.Name.Equals(name)) return p;
                    }
                }
                catch (NullReferenceException)
                {
                    return null;
                }
                return null;
            }
        }
    }

    public static class Queries
    {
        public static SParams GetAllServiceParams(string id_service)
        {
            var query = @"SELECT param_char, param_number, param_name 
                      FROM   inac.services_param 
                      WHERE  id_service = '" + id_service + "'";

            var result = Executor.ExecuteSelect(query, Environment.InacDb);

            SParams SParam = new SParams();

            for (int i = 0; i < result.Count; i++)
            {
                var cells = result.GetAllCellsFromRow(i);
                SParam.Add(cells[0], cells[1], cells[2]);
            }
            
            return SParam;
        }


        /// <summary>
        /// возвращает цену сервиса
        /// </summary>
        /// <param name="service_id"></param>
        /// <returns></returns>
        public static string GetPrice(string service_id)
        {
            var query = @"SELECT s_price
                          FROM   inac.services
                          WHERE  s_id = '" + service_id + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }
    }
}
