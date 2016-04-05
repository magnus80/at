using AT.DataBase;

namespace INAC.Helpers.Services.FTTB
{
    public static class Actions
    {
        public static void SetServicePrice(string s_id, string s_price)
        {
            var query = @"UPDATE inac.services
                          SET    s_price = " + s_price + @"
                          WHERE  s_id = '" + s_id + "'";

            Executor.ExecuteUnSelect(query, Environment.InacDb);
        }
    }
}
