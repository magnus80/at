using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using AT.Global;

namespace AT.DataBase
{
    internal partial class DB
    {
        private void OracleConnect()
        {
            try
            {
                //Connection Information
                string oracleDbConnection = "Data Source=(DESCRIPTION="
                                            + "(ADDRESS_LIST="
                                            + "(ADDRESS="
                                            + "(PROTOCOL=TCP)"
                                            + "(HOST=" + Host + ")"
                                            + "(PORT=" + Port + ")"
                                            + ")"
                                            + ")"
                                            + "(CONNECT_DATA="
                                            + "(SERVER=DEDICATED)"
                                            + "(SERVICE_NAME=" + Sid + ")"
                                            + ")"
                                            + ");"
                                            + " User Id=" + User + ";Password=" + Password + "; ";

                //Connection to datasource, using connection parameters given above
#pragma warning disable 612,618
                _oracleConnection = new OracleConnection(oracleDbConnection);
#pragma warning restore 612,618
                //Open database connection
                _oracleConnection.Open();
            }

                // Catch exception when Error in connecting to database occurs

            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }

        private void OracleConnect(string host, string port, string sid, string user, string password)
        {
            try
            {
                //Connection Information
                string oracleDbConnection = "Data Source=(DESCRIPTION="
                                            + "(ADDRESS_LIST="
                                            + "(ADDRESS="
                                            + "(PROTOCOL=TCP)"
                                            + "(HOST=" + host + ")"
                                            + "(PORT=" + port + ")"
                                            + ")"
                                            + ")"
                                            + "(CONNECT_DATA="
                                            + "(SERVER=DEDICATED)"
                                            + "(SERVICE_NAME=" + sid + ")"
                                            + ")"
                                            + ");"
                                            + " User Id=" + user + ";Password=" + password + "; ";

                //Connection to datasource, using connection parameters given above
#pragma warning disable 612,618
                _oracleConnection = new OracleConnection(oracleDbConnection);
#pragma warning restore 612,618
                //Open database connection
                _oracleConnection.Open();
            }

                // Catch exception when Error in connecting to database occurs

            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }
        }
        private void MsSqlConnect()
        {
            try
            {
                var conStr = "Server=" + Host + (Port.Length > 0 ? ":" + Port : "") + ";Database=" + Sid + ";Uid=" +
                             User + ";Pwd=" + Password + ";";

                _mssqlConnection = new SqlConnection(conStr);
                _mssqlConnection.Open();
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
            }

        }

        private  void OracleReconnect()
        {
            OracleDisconnect();
            OracleConnect();
        }

        private void OracleReconnect(string host, string port, string sid, string user, string password)
        {
            OracleDisconnect();
            OracleConnect(host, port, sid, user, password);
        }

        private void OracleDisconnect()
        {
            if (_oracleConnection.State == ConnectionState.Open) _oracleConnection.Close();
        }

        private void MsSqlDisconnect()
        {
            if (_mssqlConnection.State == ConnectionState.Open) _mssqlConnection.Close();
        }
    }
}
