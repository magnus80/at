//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange_смена кнотроллера ГП")]
    public class test_243279 : TestBase
    {
        private string gp_id;

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_02()
        {
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();
            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_03()
        {
            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);
            Pages.HD.GlobalComments.SetGpController("");
        }

        [Test]
        public void step_04()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_home_IPTV"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];


            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_home_IPTV"));
        }

        [Test]
        public void step_05()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_home_MSK"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_home_MSK"));
        }

        [Test]
        public void step_06()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_core_MSK"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_core_MSK"));
        }


        [Test]
        public void step_07()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_home_Region"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_home_Region"));
        }

        [Test]
        public void step_08()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_home_SPB"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_home_SPB"));
        }

        [Test]
        public void step_09()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "duty_sup"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "duty_sup"));
        }

        [Test]
        public void step_10()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_controller"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", ""));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_char",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'T_CONTROLLER' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, ""));
        }
    }
}
