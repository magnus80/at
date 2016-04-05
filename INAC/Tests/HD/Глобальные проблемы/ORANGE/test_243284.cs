//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange заведение ГП логин")]
    public class test_243284 : TestBase
    {
        private string gp_id;
        private string house;
        private string login;

        [Test]
        public void step_01()
        {
            house = Helpers.Adresses.Queries.GetConnectedHouse(Helpers.Adresses.Queries.GetConnectedStreet("12042"));
            login = Helpers.HD_Users.Queries.GetHdUser(1)[0];

            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_HOUSE", house));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "ID_FLAG", login));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "GTYPE", "15"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "2"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));

            var res = Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);
            gp_id = res.Substring(0, res.IndexOf('/'));

            Assertion("ошибка при создании ГП (orange)", () => Assert.IsNotEmpty(gp_id));
        }

        [Test]
        public void step_02()
        {
            var oper = Helpers.Query.GetFieldsOfTable("t_operator", "helpdesk.tickets", "t_id = " + gp_id)[0];
            Assertion("ошибка назначения ответственного ГП (orange)", () => Assert.AreEqual(oper, login));
        }

        [Test]
        public void step_03()
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_HOUSE", house));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar", "ID_FLAG", login + "1"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "GTYPE", "15"));
            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "SCALE", "2"));
            Executor.ProcedureParamList.Add(new ProcedureParam("VarChar"));

            var res = Executor.ExecuteProcedure("helpdesk.orange_func_new", Environment.InacDb);

            Assertion("ошибка при создании ГП (orange)", () => Assert.AreEqual(res, "error"));
        }
    }
}
