using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using common.Interface;
using common.Helpers;

namespace BRIC.Repositories
{
    public class DatabaseSession : IDatabaseSession
    {
        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }


        public DatabaseSession()
        {
            var ini = new IniFileHelper();

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ini.Read("Database Config", "Address") + "," + ini.Read("Database Config", "Port"),
                InitialCatalog = ini.Read("Database Config", "Database"),
                UserID = ini.Read("Database Config", "User"),
                Password = ini.Read("Database Config", "Password")
            };

            Connection = new SqlConnection(builder.ConnectionString);
            Connection.Open();
        }

        public void Begin()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void Commin()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Transaction = null;
        }
        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}
