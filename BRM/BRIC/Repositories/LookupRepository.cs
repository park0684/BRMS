using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Reposities;

namespace BRIC.Repositories
{
    public class LookupRepository : BaseReporsitory, ILookupRepository
    {
        public string CategoryNameKr(int top, int mid, int bot)
        {
            string query = $"SELECT cat_name_kr FROM category WHERE cat_top = {top} AND cat_mid ={mid} AND cat_bot ={bot}";
            return SqlScalarQuery(query).ToString();
        }

        public string CategoryNameEn(int top, int mid, int bot)
        {
            string query = $"SELECT cat_name_en FROM category WHERE cat_top = {top} AND cat_mid ={mid} AND cat_bot ={bot}";
            return SqlScalarQuery(query).ToString();
        }

        public string CategoryName(int catCode)
        {
            string query = $"";
            return SqlScalarQuery(query).ToString();
        }

        public string CustomerName(int memCode)
        {
            string query = $"SELECT cust_name FROM customer WHERE cust_code = {memCode}";
            return SqlScalarQuery(query).ToString();
        }

        public string EmployeeName(int empCode)
        {
            string query = $"SELECT emp_name FROM employee WHERE emp_code = {empCode}";
            return SqlScalarQuery(query).ToString();
        }

        public string ProductName(int pdtCode)
        {
            string query = $"SELECT pdt_name_kr FROM product WHERE pdt_code = {pdtCode}";
            return SqlScalarQuery(query).ToString();
        }

        public string SupplierName(int supCode)
        {
            string query = $"SELECT sup_name FROM supplier WHERE sup_code = {supCode}";
            return SqlScalarQuery(query).ToString();
        }


    }
}
