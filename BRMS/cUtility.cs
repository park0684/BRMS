using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BRMS
{
    class cUtility
    {
        
        public static void InsertCustPointHistry(int custCode, int type, int point, int param, SqlConnection connection, SqlTransaction transaction)
        {
            cDatabaseConnect dbconn = new cDatabaseConnect();
            object resultObj = new object();
            string query = $"SELECT cust_point FROM customer WHERE cust_code = {custCode}";
            dbconn.sqlScalaQuery(query, out resultObj);
            int previousPoint = cDataHandler.ConvertToInt(resultObj);

            query = "INSERT INTO pointhistocy(ph_type, ph_cust, ph_param, ph_point, ph_previous, ph_date)" +
                "VALUES(@type, @cust, @param, @point, @previous, GETDATE())";
            SqlParameter[] parameters =
            {
                new SqlParameter("@type",SqlDbType.Int){Value = type},
                new SqlParameter("@type",SqlDbType.Int){Value = custCode},
                new SqlParameter("@type",SqlDbType.Int){Value = param},
                new SqlParameter("@type",SqlDbType.Int){Value = point},
                new SqlParameter("@type",SqlDbType.Int){Value = previousPoint}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
        }
    }
}
