using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static class VSU
        {
            public static void Connect(string login, string vsu_service)
            {
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "login", login));
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "in_service", vsu_service));
                Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                Executor.ExecuteProcedure("inac.vpdn_speedup_pkg.add", Environment.InacDb);
            }
        }
    }
}
