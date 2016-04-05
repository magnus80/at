using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using AT.Global;

namespace AT.DataBase
{
    internal partial class DB
    {
        private OracleType ParseOracleType(string type)
        {
            switch (type.ToLower())
            {
                case "varchar":
                    return OracleType.VarChar;
                case "number":
                    return OracleType.Number;
                case "datetime":
                    return OracleType.DateTime;

                default:
                    return OracleType.VarChar;
            }
        }
        
        private OracleParameter[] OracleParamList
        {
            get
            {
                try
                {
                    var Params = new List<OracleParameter>();
                    foreach (var par in Executor.ProcedureParamList)
                    {
                        Params.Add(new OracleParameter());
                        Params.Last().OracleType = ParseOracleType(par.Type);

                        if (par.IsReturnParam)
                        {
                            Params.Last().Direction = ParameterDirection.ReturnValue;
                            Params.Last().Size = 255;
                        }
                        else
                        {
                            Params.Last().ParameterName = par.Name;
                            Params.Last().Value = par.Value;
                        }
                    }

                    Executor.ProcedureParamList.Clear();
                    return Params.ToArray();
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return null;
                }
            }
        }

        private SqlParameter[] MsSqlParamList
        {
            get
            {
                try
                {
                    var Params = new List<SqlParameter>();
                    foreach (var par in Executor.ProcedureParamList)
                    {
                        Params.Add(new SqlParameter());
                        Params.Last().TypeName = par.Type;

                        if (par.IsReturnParam)
                        {
                            Params.Last().Direction = ParameterDirection.ReturnValue;
                        }
                        else
                        {
                            Params.Last().ParameterName = par.Name;
                            Params.Last().Value = par.Value;
                        }
                    }

                    Executor.ProcedureParamList.Clear();
                    return Params.ToArray();
                }
                catch (Exception ex)
                {
                    GlobalEvents.ExeptionFounded(ex);
                    return null;
                }
            }
        }

        private string ExecuteProcedureOracle(string procedureName)
        {
            try
            {
                OracleReconnect();

                OracleTransaction transaction = _oracleConnection.BeginTransaction(IsolationLevel.Serializable);
#pragma warning disable 612,618
                OracleCommand command = new OracleCommand(procedureName, _oracleConnection, transaction);
#pragma warning restore 612,618
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(OracleParamList);
                command.ExecuteNonQuery();
                command.Transaction.Commit();

                foreach (var par in command.Parameters.Cast<OracleParameter>())
                {
                    if(par.Direction.Equals(ParameterDirection.ReturnValue))
                    {
                        return par.Value.ToString();
                    }
                }

                return "success";
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return "error";
            }
        }

        private string ExecuteProcedureMsSql(string procedureName)
        {
            try
            {
                if (_mssqlConnection.State != ConnectionState.Open)
                    MsSqlConnect();

                SqlTransaction transaction = _mssqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);
#pragma warning disable 612,618
                SqlCommand command = new SqlCommand(procedureName, _mssqlConnection, transaction);
#pragma warning restore 612,618
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(MsSqlParamList);
                command.ExecuteNonQuery();
                command.Transaction.Commit();

                foreach (var par in command.Parameters.Cast<SqlParameter>())
                {
                    if (par.Direction.Equals(ParameterDirection.ReturnValue))
                    {
                        return par.Value.ToString();
                    }
                }

                return "success";
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return "error";
            }
        }
    }
}
