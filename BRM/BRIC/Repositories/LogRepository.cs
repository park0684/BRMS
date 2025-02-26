using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Reposities;
using common.DTOs;
using common.Helpers;

namespace BRIC.Repositories
{
    class LogRepository : BaseReporsitory, ILogRepository
    {
        public DataRow LoadCategorInfo(int catCode)
        {
            string query = "SELECT c.cat_name_kr as cat_name, CASE WHEN c.cat_mid = 0 THEN t.cat_name_kr WHEN c.cat_mid != 0 AND c.cat_bot = 0 THEN t.cat_name_kr  + '>' + m.cat_name_kr ELSE t.cat_name_kr  + '>' + m.cat_name_kr + '>' + b.cat_name_kr END as cat_full " +
                $"FROM category c LEFT JOIN  category t on t.cat_top = c.cat_top AND t.cat_mid = 0 AND t.cat_bot = 0 LEFT JOIN category m on m.cat_top = c.cat_top AND m.cat_mid = c.cat_mid AND m.cat_bot = 0 LEFT JOIN category b on b.cat_top = c.cat_top AND b.cat_mid = c.cat_mid AND b.cat_bot = c.cat_bot WHERE c.cat_code =  {catCode}";

            return SqlAdapterQuery(query).Rows[0];
        }

        public string LoadSupplierName(int supCode)
        {
            string query = $"SELECT sup_name FROM supplier WHERE sup_code = {supCode}";
            return SqlScalarQuery(query).ToString();
        }

        public DataTable LoadEmpAccessLog(LogSearchDto search)
        {
            throw new NotImplementedException();
        }

        public string LoadEmployee(int empCode)
        {
            string query = $"SELECT emp_name FROM employee WHERE emp_code = {empCode}";
            return SqlScalarQuery(query).ToString();
        }

        public DataTable LoadEmployeeLog(LogSearchDto search)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();

            query.Append("SELECT emplog_type, emplog_before,emplog_after, emplog_param,emplog_param2, emplog_emp, emplog_date FROM emplog");

            whereCondition.Add($"emplog_date > '{search.FromDate.ToString("yyyy-MM-dd")}'");
            whereCondition.Add($"emplog_date <= '{search.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (search.type != -1)
                whereCondition.Add($"emplog_type = {search.type}");
            if (!string.IsNullOrEmpty(search.SearchWord))
            {
                string empQuery = $"SELECT distinct(emp_code) FROM employee WHERE emp_name LIKE '%{search.SearchWord}%'";
                var resultTable = SqlAdapterQuery(empQuery);
                var empCode = resultTable.AsEnumerable().Select(row => row["emp_code"].ToString()).ToArray();
                if (resultTable.Rows.Count > 0)
                {
                    whereCondition.Add("emplog_param IN (" + string.Join(",", empCode) + ")");
                }
            }
            query.Append(" WHERE " + string.Join(" AND ", whereCondition) + " ORDER BY emplog_date, emplog_type");
            return SqlAdapterQuery(query.ToString());
        }

        public DataRow LoadProductINfo(int pdtCode)
        {
            string query = $"SELECT pdt_number, pdt_name_kr FROM product WHERE pdt_code = {pdtCode}";
            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable LoadPurchaseLog(LogSearchDto search)
        {
            throw new NotImplementedException();
        }

        public DataTable LoadSupPaymentLog(LogSearchDto search)
        {
            throw new NotImplementedException();
        }

        public DataTable LoadProductLog(LogSearchDto search)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();

            query.Append("SELECT pdtlog_type, pdtlog_before, pdtlog_after, pdtlog_param, emp_name, pdtlog_date FROM productlog, employee  ");

            whereCondition.Add("pdtlog_emp = emp_code");
            whereCondition.Add($"pdtlog_date > '{search.FromDate.ToString("yyyy-MM-dd")}'");
            whereCondition.Add($"pdtlog_date <= '{search.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (search.type != -1)
                whereCondition.Add("pdtlog_type = " + search.type);
            if (!string.IsNullOrEmpty(search.SearchWord))
            {
                string pdtQuery = $"SELECT distinct(pdt_code) FROM product WHERE pdt_number like '%{search.SearchWord}%' or pdt_name_kr like '%{search.SearchWord}%' or pdt_name_en like '%{search.SearchWord}%'";
                var resultTale = SqlAdapterQuery(pdtQuery);
                StringBuilder pdtCodes = new StringBuilder();
                if (resultTale.Rows.Count > 0)
                {
                    foreach(DataRow code in resultTale.Rows)
                    {
                        if (pdtCodes.Length == 0)
                            pdtCodes.Append(code[0]);
                        else
                            pdtCodes.Append("," + code[0]);
                    }
                }
                whereCondition.Add("pdtlog_param IN" + pdtCodes);
            }
            query.Append("WHERE " + string.Join(" AND ", whereCondition) + " ORDER BY pdtlog_date, pdtlog_type");
            return SqlAdapterQuery(query.ToString());
        }
        public DataTable LoadCustomerLog(LogSearchDto search)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();

            query.Append("SELECT custlog_type, custlog_before, custlog_after, custlog_param, custlog_emp, custlog_date FROM customerlog ");
            whereCondition.Add($"custlog_date > '{search.FromDate.ToString("yyyy-MM-dd")}'");
            whereCondition.Add($"custlog_date <= '{search.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (search.type != -1)
                whereCondition.Add("custLog_type = " + search.type);
            if (!string.IsNullOrEmpty(search.SearchWord))
            {
                string custSeachrQuery = $"SELECT disticnt(cust_code) as cust_code FROM customer WHERE cust_name LIKE '%{search.SearchWord}%'";
                var resultTable = SqlAdapterQuery(custSeachrQuery);
                var custCode = resultTable.AsEnumerable().Select(row => row["cust_code"].ToString()).ToArray();
                if(resultTable.Rows.Count > 0)
                {
                    whereCondition.Add("custlog_param IN (" +  string.Join(",", custCode) + ")");
                }
            }
            query.Append("WHERE " + string.Join(" AND ", whereCondition) + "ORDER BY custlog_date, custlog_type");
            return SqlAdapterQuery(query.ToString());
        }
        public DataTable LoadSupplierLog(LogSearchDto search)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();

            return SqlAdapterQuery(query.ToString());
        }
        
        public void InsertAccessLog(int parameterCode, int empCode, int TypeCode, SqlConnection connection, SqlTransaction transaction)
        {
            DataTable paramTable = LogHelper.CreateAccessLogTable(TypeCode, empCode, parameterCode);
            SqlParameter[] parameters =
            {
                new SqlParameter(LogHelper.Parameters.Target,SqlDbType.VarChar){Value = LogHelper.Targets.AccessLog},
                new SqlParameter
                {
                    ParameterName = LogHelper.Parameters.Logs,
                    SqlDbType = SqlDbType.Structured,
                    TypeName = LogHelper.TypeNames.LogInfo,
                    Value = paramTable
                }
            };
            SqlExecuteNonQuery(StoredProcedures.InsertLog, connection, transaction, parameters);
        }
        public void InsertLog(List<LogDto> logs,string proceduerTaget, SqlConnection connection, SqlTransaction transaction)
        {
            DataTable paramTable = LogHelper.CreateLogTable();

            foreach (var log in logs)
            {
                paramTable.Rows.Add(
                    log.logType,
                    log.OriginalValue,
                    log.EditValue,
                    log.EmployeeCode,
                    log.Param
                    );
            }
            SqlParameter[] parameters =
            {
                new SqlParameter (LogHelper.Parameters.Target, DbType.String){ Value = proceduerTaget},
                new SqlParameter
                {
                    ParameterName = LogHelper.Parameters.Logs,
                    SqlDbType = SqlDbType.Structured,
                    TypeName = LogHelper.TypeNames.LogInfo,
                    Value = paramTable
                }
            };
            SqlExecuteNonQuery(StoredProcedures.InsertLog, connection, transaction, parameters);
        }

    }
}
