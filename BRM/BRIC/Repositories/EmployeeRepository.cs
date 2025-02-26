using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using common.Helpers;
using common.DTOs;
using common.Reposities;
namespace BRIC.Repositories
{
    public class EmployeeRepository : BaseReporsitory, IEmployeeReporitory
    {
        public void InsertEmployee(EmployeeDto employee, SqlConnection connection, SqlTransaction transaction)
        {
            
        }

        public DataRow LoadEmployeeInfo(int empCode)
        {
            string query = $"SELECT emp_name, emp_password, emp_cell, emp_level, emp_email, emp_addr, emp_status, emp_idate, emp_udate, emp_memo FROM employee WHERE emp_code = {empCode}";
            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable LoadEmployeeList(int empStatus)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT emp_code,emp_name, emp_level, emp_cell, emp_email, emp_addr, emp_status,emp_idate, emp_udate, emp_memo FROM employee");
            if (empStatus != -1)
                query.Append($" WHERE emp_status = {empStatus}");
            query.Append(" ORDER BY emp_code");
            return SqlAdapterQuery(query.ToString());
        }

        public void UpdateEmployee(EmployeeDto employee, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameter[] empParameter =
            {
                new SqlParameter("@empCode", SqlDbType.Int){Value = employee.EmployeeCode},
                new SqlParameter("@empName", SqlDbType.Int){Value = employee.EmployeeName},
                new SqlParameter("@empPassword", SqlDbType.Int){Value = employee.EmployeePassword},
                new SqlParameter("@empLevel", SqlDbType.Int){Value = employee.EmployeeLevel},
                new SqlParameter("@empCell", SqlDbType.Int){Value = employee.EmployeePhon},
                new SqlParameter("@empEmain", SqlDbType.Int){Value = employee.EmployeeEmail},
                new SqlParameter("@empAddr", SqlDbType.Int){Value = employee.EmployeeAddress},
                new SqlParameter("@empStatus", SqlDbType.Int){Value = employee.EmployeeStatus},
                new SqlParameter("@empMemo", SqlDbType.Int){Value = employee.Employeememo}
            };

            SqlExecuteNonQuery(StoredProcedures.UpdateEmployee, connection, transaction, empParameter);
            //using(SqlConnection connection = OpenSql())
            //{
            //    SqlTransaction transaction = connection.BeginTransaction();
            //    try
            //    {
            //        SqlExecuteNonQuery(StoredProcedures.InsertEmployee, connection, transaction, empParameter);
            //    }
            //    catch(Exception ex)
            //    {
            //        transaction.Rollback();
            //        throw new Exception(ex.Message);
            //    }
            //}
        }
        
    }
}
