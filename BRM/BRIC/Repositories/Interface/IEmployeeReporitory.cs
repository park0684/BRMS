using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using common.DTOs;

namespace BRIC.Repositories
{
    public interface IEmployeeReporitory
    {
        DataTable LoadEmployeeList(int empStatus);
        DataRow LoadEmployeeInfo(int empCode);
        void InsertEmployee(EmployeeDto employee, SqlConnection connection, SqlTransaction transaction);
        void UpdateEmployee(EmployeeDto employee, SqlConnection connection, SqlTransaction transaction);
    }
}
