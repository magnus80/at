using AT.DataBase;
using AT.Tools;

namespace INAC.Helpers.AAA
{
    public static partial class Actions
    {
        public static class Bras
        {
            public static void SetBrasPerTarif(string ip)
            {
                var query = @"update inac.nas set n_f_ptarif = 1 where n_ip = '" + ip + "'";

                Executor.ExecuteUnSelect(query, Environment.InacDb);
            }

            public static void SetBrasPerUser(string ip)
            {
                var query = @"update inac.nas set n_f_ptarif = 0 where n_ip = '" + ip + "'";

                Executor.ExecuteUnSelect(query, Environment.InacDb);
            }

            public static string ExecuteCmdCisco(string login)
            {
                Cmd.Execute(@"psexec \\MS-D21225-12 -u VIMPELCOM_MAIN\MAtyakshev -p Ab79dVL34  test " + Environment.BrasCisco + " CISCO " +
                            login);

                foreach (var line in Cmd.ReadAll())
                {
                    if (line.StartsWith("#PARAM:")) return line.Replace(" ", "").Remove(0, "#PARAM:".Length);
                }

                return string.Empty;
            }
        }
    }
}
