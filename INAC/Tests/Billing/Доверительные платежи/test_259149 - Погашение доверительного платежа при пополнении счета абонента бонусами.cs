using System.Linq;
using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Promised payments")]
    public class test_259149 : TestBase
    {
        private string login, adress, ticket;

        [Test]
        public void step_01()
        {
            var service = Helpers.Services.FTTB.Queries.GetFttbServiceForVsu("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service);
            var sum = Helpers.Abonents.Info.GetServicesPriceSum(login);
            var sum_f = float.Parse(sum) + 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(sum_f.ToString(), login);
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_03()
        {
            Helpers.Abonents.Actions.Payments.AddBonusToLogin("1000", login);
        }

        [Test]
        public void step_04()
        {
            adress = Helpers.Abonents.Info.GetAdressByLogin(login);
            Pages.HD.Address.Open("?address_id=" + adress);
            Pages.HD.Address.CreateTicket("Другая", "Выбор скорости");

            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("Ошибка при создании заявки 'Выбор скорости'", () => Assert.IsNotNullOrEmpty(ticket));
        }

        [Test]
        public void step_05()
        {
            Pages.HD.PTN.Add_Speed.Change_Speed.Open(login + "/" + ticket);
            Pages.HD.PTN.Add_Speed.Change_Speed.LevelUpClick();
            Pages.HD.PTN.Add_Speed.Change_Speed.NextClick();
            Pages.HD.PTN.Add_Speed.Change_Speed.AsseptConnectWithPromisedPay();
            Pages.HD.PTN.Add_Speed.Change_Speed.WaitOrdering();
        }

        [Test]
        public void step_06()
        {
            Pages.HD.Address.Open("?address_id=" + adress);
           

            var services = Helpers.Abonents.Info.GetServices(login);
            bool vsu_connected = false;


            foreach (var service in services)
            {
                var s_params = Helpers.Services.Queries.GetAllServiceParams(service);
                foreach(var param in s_params.Params)
                {
                    if (param.Name.Equals("VPDN_SPEED_UP"))
                    {
                        vsu_connected = true;
                        break;
                    }
                }
            }

            Assertion("Не подключилась опция 'выбор скорости' с использованием дов. платежа, логин: " + login, () => Assert.IsTrue(vsu_connected));

        }

        [Test]
        public void step_07()
        {
            var query = @"SELECT Count(*)
                          FROM   inac.promised_payments
                          WHERE  pp_login = '" + login + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Абоненту не активировался доверительный платеж при подключении VSU, логин: " + login,
                      () => Assert.IsTrue(res.Count == 1));
        }

        [Test]
        public void step_08()
        {
            Assert.Fail("Не работает оплата бонусами");
        }
    }
}
