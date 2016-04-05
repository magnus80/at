using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AT.Global;

namespace AT.DataBase
{
    public static class Executor
    {

        public static List<ProcedureParam> ProcedureParamList = new List<ProcedureParam>();


        public static DBResult ExecuteSelect(string query)
        {
            try
            {
                return GlobalVariables.DbList.FirstOrDefault().ExecuteSelect(query);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
            
        }

        public static DBResult ExecuteSelect(string query, string host, string port, string sid, string user, string password)
        {
            try
            {
                return GlobalVariables.DbList.FirstOrDefault().ExecuteSelect(query, host, port, sid, user, password);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }

        }

        public static void ExecuteUnSelect(string query)
        {
            try
            {
                GlobalVariables.DbList.FirstOrDefault().ExecuteUnSelect(query);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }

        }

        public static string ExecuteProcedure(string procedureName, string dbName)
        {
            try
            {
                var res = GlobalVariables.DbList.Find(db => db.Sid.Equals(dbName)).ExecuteProcedure(procedureName);
                System.Threading.Thread.Sleep(500);
                return res;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }
    }
}
