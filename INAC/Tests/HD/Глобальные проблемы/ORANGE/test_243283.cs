//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;


namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("orange заведение ГП ответственный по сети")]
    public class test_243283 : TestBase
    {
        private string gp_id;
        private string house;

        [Test]
        public void step_01()
        {
            house = Helpers.Adresses.Queries.GetConnectedHouse(Helpers.Adresses.Queries.GetConnectedStreet("12042"));

            Executor.ProcedureParamList.Add(new ProcedureParam("Number", "ID_HOUSE", house));
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
            var query = @"
            SELECT t_operator AS resp 
            FROM   helpdesk.tickets 
            WHERE  t_id = " + gp_id + @"
            INTERSECT 
            SELECT b.u_login AS resp 
            FROM   helpdesk.shedule_crash_users a, 
                   helpdesk.users b 
            WHERE  scu_date = (SELECT Max (a.scu_date) 
                               FROM   helpdesk.shedule_crash_users a 
                               WHERE  a.scu_dealer = (SELECT a.h_dealer 
                                                      FROM   inac.houses0 a 
                                                      WHERE  h_id = " +
                        house + @")) 
                   AND a.scu_user = b.u_login 
                   AND a.scu_dealer = (SELECT a.h_dealer 
                                       FROM   inac.houses0 a 
                                       WHERE  h_id = " + house + ") ";

            var result = Executor.ExecuteSelect(query, Environment.InacDb).GetFirstCellFromColumn(0);

            Assertion("Выборка  пустая",
                      () => Assert.IsNotEmpty(result));

        }
    }
}
