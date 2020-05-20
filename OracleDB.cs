using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace ORACLE_DB
{

    abstract public class OracleDB
    {
        public interface IConnectionInfo
        {
            string host { get; }
            string port { get; }
            string service_name { get; }
            string user_id { get; }
            string password { get; }
        }

        abstract protected IConnectionInfo connectionInfo { get; }

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

            string ConnectionString = GetConnectionString(
                connectionInfo.host,
                connectionInfo.port,
                connectionInfo.service_name,
                connectionInfo.user_id,
                connectionInfo.password
                );
            return new OracleConnection(ConnectionString);
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
