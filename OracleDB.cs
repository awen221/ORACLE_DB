using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace ORACLE_DB
{

    abstract public class OracleDB
    {

        abstract protected string host { get; }
        abstract protected string port { get; }
        abstract protected string service_name { get; }
        abstract protected string user_id { get; }
        abstract protected string password { get; }

        OracleConnection GetConnection()
        {
            string GetConnectionString(string host, string port, string service_name, string user_id, string password)
            {
                return
                "Data Source=" +
                "(" +
                    "DESCRIPTION =" +
                    "(" +
                        "ADDRESS_LIST =" +
                        "(" +
                            "ADDRESS = (PROTOCOL = TCP)" +
                            "(HOST = " + host + ")(PORT = " + port + ")" +
                        ")" +
                    ")" +
                    "(" +
                        "CONNECT_DATA =(SERVICE_NAME = " + service_name + ")" +
                    ")" +
                ")" +
                ";Persist Security Info=True;User ID=" + user_id + ";Password=" + password;
            }

            return new OracleConnection(GetConnectionString(host, port, service_name, user_id, password));
        }

        protected DataTable DbDataAdapter_Fill(string sql)
        {
            DataTable dataTable = new DataTable();

            using (OracleConnection oracleConnection = GetConnection())
            {
                oracleConnection.Open();

                using (OracleCommand myCommand = new OracleCommand(sql, oracleConnection))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(myCommand);
                    adapter.Fill(dataTable);
                }

                oracleConnection.Close();
            }

            return dataTable;
        }

        protected int ExecuteNonQuery(string sql)
        {
            int returnCode;

            using (OracleConnection oracleConnection = GetConnection())
            {
                oracleConnection.Open();

                using (OracleCommand myCommand = new OracleCommand(sql, oracleConnection))
                {
                    returnCode = myCommand.ExecuteNonQuery();
                }

                oracleConnection.Close();
            }

            return returnCode;
        }

    }

}
