using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AT.Global;

namespace AT.DataBase
{
    internal partial class DB
    {
        private DBResult ExecuteSelectOracle(string query)
        {
            try
            {
                DBResult res = new DBResult();

                OracleReconnect();

#pragma warning disable 612,618
                var command = new OracleCommand(query, _oracleConnection);
#pragma warning restore 612,618

                OracleDataReader oracleReader = command.ExecuteReader();
                Read(oracleReader, ref res);

                oracleReader.Close();

                return res;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        private DBResult ExecuteSelectOracle(string query, string host, string port, string sid, string user, string password)
        {
            try
            {
                DBResult res = new DBResult();

                OracleReconnect(host, port, sid, user, password);

#pragma warning disable 612,618
                var command = new OracleCommand(query, _oracleConnection);
#pragma warning restore 612,618

                OracleDataReader oracleReader = command.ExecuteReader();
                Read(oracleReader, ref res);

                oracleReader.Close();

                return res;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }
        
        private DBResult ExecuteSelectMsSql(string query)
        {
            try
            {
                DBResult res = new DBResult();

                if (_mssqlConnection.State != ConnectionState.Open)
                    MsSqlConnect();

                var command = new SqlCommand(query, _mssqlConnection);
                SqlDataReader sqlReader = command.ExecuteReader();

                Read(sqlReader,ref res);
                sqlReader.Close();

                return res;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }


        private void Read(DbDataReader reader, ref DBResult res)
        {
            int iter = 0;
            
            if (reader.HasRows)
            {
                while (reader.Read() && ++iter <= GlobalVariables.SelectedRowsLimit)
                {
                    var row = new List<string>();
                    int i = 0;
                    while (i < reader.FieldCount)
                    {
                        row.Add(reader[i].ToString());
                        i++;
                    }
                    res.Rows.Add(row);
                }
            }
        }
    }
}
