//автор: NGadiyak
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.HD
{
    [TestFixture]
    [Category("HD"), Category("Глобальные проблемы"), Description("Изменение масштаба ГП")]
    public class test_241815 : TestBase
    {
        private string area = "";
        private string gp_id = "";

        [Test]
        public void step_01()
        {
            Pages.HD.Login.LoginAsGod();
            Pages.HD.New_global.Open();
            Pages.HD.New_global.NewGP_crash();
            gp_id = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Scalechange.Open("?ticket_id=" + gp_id);

            area = Helpers.Adresses.Queries.GetArea("12042");
            Pages.HD.Scalechange.SetScaleArea(area);

            var scale = Helpers.Query.GetFieldsOfTable("gp_type", "helpdesk.global_problems", "gp_id=" + gp_id)[0];
            Assertion("не изменился масштаб, ГП: " + gp_id,
                      () => Assert.AreEqual(scale, "1"));

            var area1 = Helpers.Query.GetFieldsOfTable("pa_area", "helpdesk.problem_areas", "pa_problem = " + gp_id)[0];
            Assertion("некорректные затронутые районы, ГП: " + gp_id,
                      () => Assert.AreEqual(area1, area));
        }

        [Test]
        public void step_03()
        {
            Pages.HD.Scalechange.Open("?ticket_id=" + gp_id);

            var street_id = Helpers.Adresses.Queries.GetConnectedStreet("12042");
            var street = Helpers.Adresses.Queries.GetStreetName(street_id);
            var house_id = Helpers.Adresses.Queries.GetConnectedHouse(street_id);

            var scaleChangeResult = Pages.HD.Scalechange.SetScaleHouse(street, street_id, house_id);

            Assertion(
                "ошибка при изменении масштаба ГП = " + AT.Tools.Other.GetParamFromCurrentUrl("ticket_id") +
                ", street_id = " + street_id + ", house_id = " + house_id, () => Assert.IsTrue(scaleChangeResult));

            var scale = Helpers.Query.GetFieldsOfTable("gp_type", "helpdesk.global_problems", "gp_id=" + gp_id)[0];
            Assertion("не изменился масштаб, ГП: " + gp_id, () => Assert.AreEqual(scale, "2"));

            var house1_id = Helpers.Query.GetFieldsOfTable("ph_house", "helpdesk.problem_houses",
                                                           "ph_problem = " + gp_id)[0];
            Assertion("некорректные затронутые дома, ГП: " + gp_id, () => Assert.AreEqual(house1_id, house_id));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.Scalechange.Open("?ticket_id=" + gp_id);
            Pages.HD.Scalechange.SetScaleAllNet();

            var query = @"
            SELECT pa_area 
            FROM   helpdesk.problem_areas 
            WHERE  pa_problem = " + gp_id;

            var scale = Helpers.Query.GetFieldsOfTable("gp_type", "helpdesk.global_problems", "gp_id=" + gp_id)[0];
            Assertion("не изменился масштаб, ГП: " + gp_id, () => Assert.AreEqual(scale, "0"));

            var result = Executor.ExecuteSelect(query, Environment.InacDb);
            Assertion("данная ГП (вся сеть) не должна быть привязана к районам: " + gp_id,
                      () => Assert.IsTrue(result.Count == 0));

            query = @"
            SELECT ph_house 
            FROM   helpdesk.problem_houses 
            WHERE  ph_problem = " + gp_id;

            result = Executor.ExecuteSelect(query, Environment.InacDb);
            Assertion("данная ГП (вся сеть) не должна быть привязана к домам: " + gp_id,
                      () => Assert.IsTrue(result.Count == 0));
        }
    }
}
