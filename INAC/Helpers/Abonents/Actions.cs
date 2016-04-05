using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static void ReaccAll()
        {
            Executor.ExecuteProcedure("inac.api.reacc_all", Environment.InacDb);
        }

        public static void Reaccount(string contract)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "c", contract));
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "force", "1"));
            Executor.ExecuteProcedure("inac.api.reaccount", Environment.InacDb);
        }

        public static void ReaccLogin(string login)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "login", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "domain", "-"));
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "force", "1"));
            Executor.ExecuteProcedure("inac.api.reacc_login", Environment.InacDb);
        }

        public static void Reactivate(string login, string summ, string oper)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "LOGIN", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "DOMAIN", "-"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "Summ", summ.ToString()));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "OPER", oper));
            Executor.ExecuteProcedure("inac.reactivate_login", Environment.InacDb);
        }

        public static void ShiftBC(string login, int days)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "login", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "domain", "-"));
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "days", days.ToString()));
            Executor.ExecuteProcedure("inac.api.shift_login_bc", Environment.InacDb);
            System.Threading.Thread.Sleep(1000);
        }

        public static string ChangeServ(string login, string service_old, string service_new)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "login", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "domain", "-"));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "os", service_old));
            Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "ns", service_new));
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "ALLOW_PP_F", "1"));
            return Executor.ExecuteProcedure("inac.api.changeserv", Environment.InacDb);
        }

        public static void SetBlockType(string login, int block_type)
        {
            var query = @"update inac.logins set l_block_type = " + block_type + " where l_login = '" + login + "'";
            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
    }
}
