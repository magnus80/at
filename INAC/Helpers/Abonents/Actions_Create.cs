using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public static partial class Actions
    {
        public static class Creation
        {
            /// <summary>
            /// создание стандартного абонента (москва, рандомный ШПД, 0 на балансе)
            /// </summary>
            /// <returns>логин</returns>
            public static string Create()
            {
                int i = 0;
                var login = "error";
                while (login.Equals("error") && i++ < 10)
                {
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                    login = Executor.ExecuteProcedure("inac.New_abon_for_test", Environment.InacDb);
                }
                return login;
            }

            /// <summary>
            /// создание абонента (москва, рандомный шпд)
            /// </summary>
            /// <param name="balance">баланс</param>
            /// <returns>логин</returns>
            public static string Create(int balance)
            {
                int i = 0;
                var login = "error";
                while (login.Equals("error") && i++ < 10)
                {
                    Executor.ProcedureParamList.Add(new ProcedureParam("number", "summ", balance.ToString()));
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                    login = Executor.ExecuteProcedure("inac.New_abon_for_test", Environment.InacDb);
                }
                return login;
            }

            /// <summary>
            /// создание абонента (москва)
            /// </summary>
            /// <param name="balance">баланс</param>
            /// <param name="service">сервис</param>
            /// <returns>логин</returns>
            public static string Create(string service, int balance)
            {
                int i = 0;
                var login = "error";
                while (login.Equals("error") && i++ < 10)
                {
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "serv_id", service));
                    Executor.ProcedureParamList.Add(new ProcedureParam("number", "summ", balance.ToString()));
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                    login = Executor.ExecuteProcedure("inac.New_abon_for_test", Environment.InacDb);
                }
                return login;
            }

            /// <summary>
            /// создание абонента (москва) с заданным паролем
            /// </summary>
            /// <param name="balance">баланс</param>
            /// <param name="service">сервис</param>
            ///  <param name="password">паоль</param>
            /// <returns>логин</returns>
            public static string Create(string service, int balance, string password)
            {
                int i = 0;
                var login = "error";
                while (login.Equals("error") && i++ < 10)
                {
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "serv_id", service));
                    Executor.ProcedureParamList.Add(new ProcedureParam("number", "summ", balance.ToString()));
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "password", password));
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                    login = Executor.ExecuteProcedure("inac.New_abon_for_test", Environment.InacDb);
                }
                return login;
            }

            /// <summary>
            /// создание абонента (москва)
            /// </summary>
            /// <param name="service">сервис</param>
            /// <returns></returns>
            public static string Create(string service)
            {
                int i = 0;
                var login = "error";
                while (login.Equals("error") && i++ < 10)
                {
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar", "serv_id", service));
                    Executor.ProcedureParamList.Add(new ProcedureParam("varchar"));
                    login = Executor.ExecuteProcedure("inac.New_abon_for_test", Environment.InacDb);
                }
                return login;
            }
        }
    }
}
