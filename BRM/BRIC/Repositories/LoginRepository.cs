using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using common.DTOs;
using common.Reposities;

namespace BRIC.Repositories
{
    public class LoginRepository : BaseReporsitory, ILoginRepository
    {
        public DataRow LoadEmployee(int code)
        {
            string query = $"SELECT emp_name, emp_password FROM employee WHERE emp_code = {code}";
            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable LoadPermission(int code)
        {
            string query = $"SELECT acper_permission from accpermission WHERE acper_emp = {code} AND acper_status = 1 ";
            return SqlAdapterQuery(query);
        }
    }
}
