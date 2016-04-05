using AT;
using AT.DataBase;
using NUnit.Framework;

namespace INAC.Tests.Billing
{
    [TestFixture]
    [Category("Billing"), Category("Promised payments")]
    public class test_259147 : TestBase
    {
        private string login, adress, ticket, pp_sum, price, service, change_serv;

        [Test]
        public void step_01()
        {
            service = Helpers.Services.FTTB.Queries.GetFttbServiceForVsu("12042");
            login = Helpers.Abonents.Actions.Creation.Create(service);
            price = Helpers.Abonents.Info.GetServicesPriceSum(login);
            var price_f = float.Parse(price) + 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(price_f.ToString(), login);
        }

        [Test]
        public void step_02()
        {
            Pages.HD.Login.LoginAsGod();
        }

        [Test]
        public void step_03()
        {
            adress = Helpers.Abonents.Info.GetAdressByLogin(login);
            Pages.HD.Address.Open("?address_id=" + adress);
            Pages.HD.Address.CreateTicket("Другая", "Выбор скорости");

            ticket = AT.Tools.Other.GetParamFromCurrentUrl("ticket_id");

            Assertion("Ошибка при создании заявки 'Выбор скорости'", () => Assert.IsNotNullOrEmpty(ticket));
        }

        [Test]
        public void step_04()
        {
            Pages.HD.PTN.Add_Speed.Change_Speed.Open(login + "/" + ticket);
            Pages.HD.PTN.Add_Speed.Change_Speed.LevelUpClick();
            Pages.HD.PTN.Add_Speed.Change_Speed.NextClick();
            Pages.HD.PTN.Add_Speed.Change_Speed.AsseptConnectWithPromisedPay();
            Pages.HD.PTN.Add_Speed.Change_Speed.WaitOrdering();
        }

        [Test]
        public void step_05()
        {
            Pages.HD.Address.Open("?address_id=" + adress);

            var services = Helpers.Abonents.Info.GetServices(login);
            bool vsu_connected = false;

            foreach (var service in services)
            {
                var s_params = Helpers.Services.Queries.GetAllServiceParams(service);
                foreach (var param in s_params.Params)
                {
                    if (param.Name.Equals("VPDN_SPEED_UP"))
                    {
                        vsu_connected = true;
                        break;
                    }
                }
            }
            Assertion("Не подключилась опция 'выбор скорости' с использованием дов. платежа, логин: " + login,
                      () => Assert.IsTrue(vsu_connected));
        }

        [Test]
        public void step_06()
        {
            var query = @"SELECT pp_debted
                          FROM   inac.promised_payments
                          WHERE  pp_login = '" + login + "'";

            var res = Executor.ExecuteSelect(query, Environment.InacDb);

            Assertion("Абоненту не активировался доверительный платеж при подключении VSU, логин: " + login,
                      () => Assert.IsTrue(res.Count == 1));

            pp_sum = res[0, 0];
        }

        [Test]
        public void step_07()
        {
            var pp_sum_f = float.Parse(pp_sum) - 1;
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin(pp_sum_f.ToString(), login);
            Executor.ExecuteProcedure("inac.promised_payments_pkg.payoff_time_p", Environment.InacDb);

            Pages.HD.Address.Open("?address_id=" + adress);
        }

        [Test]
        public void step_08()
        {
            change_serv = service;

            while (change_serv.Equals(service))
            {
                change_serv = Helpers.Services.FTTB.Queries.GetFttbServiceForVsu("12042");
            }

            var price_f = float.Parse(price) + 1;

            Helpers.Services.FTTB.Actions.SetServicePrice(change_serv, price_f.ToString());

            var result = Helpers.Abonents.Actions.ChangeServ(login, service, change_serv);

            Assertion("Ошибка. Сервис успешно сменен на большую стоимость при не закрытом ДП, логин " + login,
                      () => Assert.IsTrue(result.Equals("error")));
        }

        [Test]
        public void step_09()
        {
            Helpers.Abonents.Actions.Payments.AddPaymentToLogin("10", login);
            var result = Helpers.Abonents.Actions.ChangeServ(login, service, change_serv);

            Assertion("Ошибка. Не порлучилось выполнить смену тарифа, логин " + login,
                      () => Assert.IsTrue(result.Equals("success")));
        }

        [Test]
        public void step_10()
        {
            var balance_exp = float.Parse(Helpers.Abonents.Info.GetBalanceByLogin(login));
            float balance_act = 8;

            Assertion("Некорректное значение баланса, что-то пошло не так, логин: " + login,
                      () => Assert.AreEqual(balance_act, balance_exp));
        }
    }
}
