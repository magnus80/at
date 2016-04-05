using System.Collections.Generic;
using System.Linq;
using AT.DataBase;

namespace INAC.Queries
{
    public static class Users
    {
        /// <summary>
        /// Возвращает логин и пароль юзера, который есть в группах, переданных в параметре
        /// </summary>
        /// <param name="groups">спсок групп (через запятую без пробелов)</param>
        /// <param name="dbName">им базы</param>
        /// <returns></returns>
        public static List<string> GetUserByGroups(string groups, string dbName)
        {
            var split = groups.Split(new[] {','});
            int i = 0;

            var query = @"SELECT iua_login,
                                 iua_password
                          FROM   inac.inac_user_auth
                                 JOIN inac.inac_user_group
                                   ON iua_login = iug_login
                          WHERE  iua_confirmcode IS NOT NULL
                                 AND iua_disable IS NULL
                                 AND iua_hash IS NOT NULL 
                           and iug_group = '" + split[i] + "'";

            i++;
            while (i < split.Count())
            {
                query += @" INTERSECT
                            SELECT iua_login,
                                   iua_password
                            FROM   inac.inac_user_auth
                                   JOIN inac.inac_user_group
                                     ON iua_login = iug_login 
                            WHERE iug_group = '" + split[i] + "'";
                i++;
            }

            var result = Executor.ExecuteSelect(query, dbName).GetAllCellsFromAnyRow();

            query = @"UPDATE inac.inac_user_auth
                     SET    iua_lastpwdchange = sysdate 
                     WHERE iua_login = '" + result[0] + "'";
            Executor.ExecuteUnSelect(query, dbName);

            return result;
        }
    }
}
