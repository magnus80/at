using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static class Netphone
        {
            public static void Confirm(string login, string nickname, string phone)
            {
                Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "p_login", login));
                Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "p_nickname", nickname));
                Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "p_phone", phone));
                Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
                Executor.ExecuteProcedure("inac.netphone.confirm", Environment.InacDb);
            }

            public static void Delete(string login, string blocktype)
            {
                Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "p_login", login));
                Executor.ProcedureParamList.Add(new ProcedureParam("Number", "p_block_type", blocktype));
                Executor.ProcedureParamList.Add(new ProcedureParam("Varchar", "p_oper", "lk"));
                Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
                Executor.ExecuteProcedure("inac.netphone.del", Environment.InacDb);
            }
        }
    }
}
