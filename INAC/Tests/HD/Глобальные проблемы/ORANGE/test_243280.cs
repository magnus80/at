//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange_полное отсутствие сервиса")]
    public class test_243280 : TestBase
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
            Pages.HD.GlobalComments.ServiceFailure = false;
        }

        [Test]
        public void step_04()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_failure_flag"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            Pages.HD.GlobalComments.Open("?ticket_id=" + gp_id);

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.IsTrue(Pages.HD.GlobalComments.ServiceFailure));
        }

        [Test]
        public void step_05()
        {
            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_number",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'SERVICE FAILURE' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "1"));
        }

        [Test]
        public void step_06()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_GLOBAL", gp_id));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "SET_PARAM", "set_failure_flag"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "IN_PARAM_CHAR", "0"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));
            Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

        }

        [Test]
        public void step_07()
        {
            var param =
                Helpers.Query.GetFieldsOfTable("tt.param_number",
                                               "helpdesk.tickets t join helpdesk.tickets_params tt on t.t_id = tt.t_id",
                                               "tt.param_name = 'SERVICE FAILURE' and t.t_id = " +
                                               gp_id)[0];

            Assertion("ошибка установки флага полного отсутствия сервиса, ГП: " + gp_id,
                      () => Assert.AreEqual(param, "0"));
        }
    }
}
