using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using AT.Global;

namespace AT.DataBase
{
    internal partial class DB
    {
        private void ExecuteUnSelectOracle(string query)
        {
            try
            {
                OracleReconnect();

              //  OracleTransaction transaction = _oracleConnection.BeginTransaction(IsolationLevel.ReadCommitted);
#pragma warning disable 612,618
                var command = new OracleCommand(query, _oracleConnection);
#pragma warning restore 612,618

                command.ExecuteNonQuery();
               // command.Transaction.Commit();
                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        private void ExecuteUnSelectMsSql(string query)
        {
            try
            {
                if (_mssqlConnection.State != ConnectionState.Open)
                    MsSqlConnect();

                SqlTransaction transaction = _mssqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);

                var command = new SqlCommand(query, _mssqlConnection, transaction);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }
    }
}
