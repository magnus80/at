using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static class TurboButton
        {
            public static void Connect(string login, string tb_service)
            {
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "login", login));
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "domain", "-"));
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "v_serv", tb_service));
                Executor.ExecuteProcedure("inac.turbo_button.activate_turbo_button", Environment.InacDb);
            }
        }
        
    }
}
