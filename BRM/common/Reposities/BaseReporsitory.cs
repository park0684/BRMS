using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace common.Reposities
{
    public class BaseReporsitory
    {
        IniFileHelper _ini = new IniFileHelper();
        private string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = _ini.Read("Database Config", "Address") + "," + _ini.Read("Database Config", "Port"),
                InitialCatalog = _ini.Read("Database Config", "Database"),
                UserID = _ini.Read("Database Config", "User"),
                Password = _ini.Read("Database Config", "Password")

            };
            return builder.ConnectionString;
        }

        protected SqlConnection OpenSql()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            return connection;
        }

        protected DataTable SqlAdapterQuery(string query)
        {
            using(SqlConnection connection = OpenSql())
            {
                DataTable result = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(query, connection);
                dataAdapter.Fill(result);
                return result;
            }
        }

        protected object SqlScalarQuery(string query)
        {
            using(SqlConnection connection = OpenSql())
            {
                SqlCommand command = new SqlCommand(query, connection);
                return command.ExecuteScalar();
            }
        }

        protected void SqlExecuteNonQuery(string procedure, SqlConnection connection, SqlTransaction transaction, SqlParameter[] parameters)
        {
            using(SqlCommand command = new SqlCommand(procedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                {
                    if(transaction != null)
                    {
                        command.Transaction = transaction;
                    }
                    if(parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
