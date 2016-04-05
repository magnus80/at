using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace AT.DataBase
{
    internal partial class DB
    {
#pragma warning disable 612,618
        private OracleConnection _oracleConnection;
#pragma warning restore 612,618
        private SqlConnection _mssqlConnection;

        #region settings

        /* настройки подключения к БД */
        private string _host = "";
        private string _port = "";
        private string _sid = "";
        private string _user = "";
        private string _password = "";
        private string _type = "";
        /******************************/

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string Sid
        {
            get { return _sid; }
            set { _sid = value; }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region constructors

        public DB()
        {
            
        }

        public DB(string connectionString)
        {
            var split = connectionString.Split(new[] { ';' });

            Host = split[0];
            Port = split[1];
            Sid = split[2];
            User = split[3];
            Password = split[4];
            Type = split[5];
        }

        #endregion

        #region connection

        public bool Connected
        {
            get
            {
                switch (Type)
                {
                    case "oracle":
                        return _oracleConnection.State == ConnectionState.Open;
                    case "mssql":
                        return _mssqlConnection.State == ConnectionState.Open;
                    default:
                        return false;
                }
            }
        }
        
        public void OpenConnection()
        {
            switch (Type)
            {
                case "oracle":
                    OracleConnect();
                    break;
                case "mssql":
                    MsSqlConnect();
                    break;
            }
        }

        public void CloseConnection()
        {
            switch (Type)
            {
                case "oracle":
                    OracleDisconnect();
                    break;
                case "mssql":
                    MsSqlDisconnect();
                    break;
            }
        }

        #endregion

        #region execute

        #region select

        /// <summary>
        /// Выполнение запроса select
        /// </summary>
        /// <param name="query">запрос</param>string host, string port, string sid, string user, string password
        /// <returns></returns>
        public DBResult ExecuteSelect(string query)
        {
            switch (Type)
            {
                case "oracle":
                    return ExecuteSelectOracle(query);
                case "mssql":
                    return ExecuteSelectMsSql(query);
                default:
                    return null;
            }
        }

        public DBResult ExecuteSelect(string query, string host, string port, string sid, string user, string password)
        {
            switch (Type)
            {
                case "oracle":
                    return ExecuteSelectOracle(query,host, port, sid, user, password);
                case "mssql":
                    return ExecuteSelectMsSql(query);
                default:
                    return null;
            }
        }
        #endregion

        #region unselect

        /// <summary>
        /// выполнение запроса не на выборку (update, insert, delete)
        /// </summary>
        /// <param name="query">запрос</param>
        public void ExecuteUnSelect(string query)
        {
            switch (Type)
            {
                case "oracle":
                    ExecuteUnSelectOracle(query);
                    break;
                case "mssql":
                    ExecuteUnSelectMsSql(query);
                    break;
            }
        }

        #endregion

        #region procedure

        public string ExecuteProcedure(string procedureName)
        {
            switch (Type)
            {
                case "oracle":
                    return ExecuteProcedureOracle(procedureName);
                case "mssql":
                    return ExecuteProcedureMsSql(procedureName);
                default:
                    return null;
            }
        }

        #endregion

        #endregion
    }
}
