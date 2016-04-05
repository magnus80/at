//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange заведение ГП дежурный")]
    public class test_243285 : TestBase
    {
        private string gp_id;
        private string house;

        [Test]
        public void step_01()
        {
            house = Helpers.Adresses.Queries.GetConnectedHouse(Helpers.Adresses.Queries.GetConnectedStreet("12042"));

           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_HOUSE", house));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "ID_FLAG", "1"));
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "GTYPE", "15"));
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "2"));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));

            var res = Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);
            gp_id = res.Substring(0, res.IndexOf('/'));

            Assertion( "ошибка при создании ГП (orange)", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_02()
        {
            var oper = Helpers.Query.GetFieldsOfTable("t_operator", "helpdesk.tickets", "t_id = " + gp_id)[0];
            Assertion( "ошибка назначения ответственного ГП (orange)", () => Assert.AreEqual(oper, "duty_home_MSK"));
        }

        [Test]
        public void step_03()
        {
            house = Helpers.Adresses.Queries.GetConnectedHouse(Helpers.Adresses.Queries.GetConnectedStreet("12042"));

           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_HOUSE", house));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "ID_FLAG", "2"));
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "GTYPE", "15"));
           Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "2"));
           Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));

            var res = Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);
            gp_id = res.Substring(0, res.IndexOf('/'));

            Assertion( "ошибка при создании ГП (orange)", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_04()
        {
            var oper = Helpers.Query.GetFieldsOfTable("t_operator", "helpdesk.tickets", "t_id = " + gp_id)[0];
            Assertion( "ошибка назначения ответственного ГП (orange)", () => Assert.AreEqual(oper, "duty_home_SPB"));
        }
    }
}
