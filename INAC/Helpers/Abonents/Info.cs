using System;
using System.Collections.Generic;
using AT.DataBase;

namespace INAC.Helpers.Abonents
{
    public class Info
    {
        /// <summary>
        /// Возвращает пароль пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static string GetPassword(string login)
        {
            var query = @"SELECT l_password
                          FROM   inac.logins
                          WHERE  l_login = '" + login + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        /// <summary>
        /// Считает суммарную стоимость подключенных сервисов у абонента
        /// </summary>
        /// <param name="login">логин абонента</param>
        /// <returns>значение суммы</returns>
        public static string GetServicesPriceSum(string login)
        {
            var query = @"SELECT Sum (s_price)
                         FROM   inac.services
                                JOIN inac.services_per_login
                                  ON spl_service = s_id
                         WHERE  spl_login = '" + login + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0,0];
        }

        public static string GetContract(string login)
        {
            var query = @"SELECT l_contract
                          FROM   inac.logins
                          WHERE  l_login = '" + login + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0,0];
        }

        public static string GetBlockType(string login)
        {
            var query = @"SELECT l_block_type
                          FROM   inac.logins
                          WHERE  l_login = '" + login + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        public static string GetBalanceByLogin(string login)
        {
            var contract = GetContract(login);

            return GetBalanceByContract(contract);
        }

        public static string GetBalanceByContract(string contract)
        {
            var query = @"SELECT c_payed - c_uses
                          FROM   inac.contracts
                          WHERE  c_id = " + contract;

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }

        /// <summary>
        /// считает сумму всех непокашенных обещанных платежей по логину (статус 1)
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static string GetPromisedPaymentSum(string login)
        {
            var query = @"SELECT Sum(pp_debted)
                     FROM   inac.promised_payments
                     WHERE  pp_login = '" + login + @"'
                              AND pp_status = 1";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }
        
        public static List<string> GetServices(string login)
        {
            try
            {
                var query = @"SELECT spl_service
                          FROM   inac.services_per_login
                          WHERE  spl_login = '" + login + "'";

                return Executor.ExecuteSelect(query, Environment.InacDb).GetAllCellsFromColumn(0);
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Возвращает ID адреса абонента
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static string GetAdressByLogin(string login)
        {
            var query = @"SELECT c_address0
                          FROM   inac.contracts
                                 JOIN inac.logins
                                   ON l_contract = c_id
                          WHERE  l_login = '" + login + "'";

            return Executor.ExecuteSelect(query, Environment.InacDb)[0, 0];
        }
    }
}
