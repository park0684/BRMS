using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using common.Reposities;

namespace BRIC.Repositories
{
    public class SupplierRepository : BaseReporsitory ,ISupplierRepository
    {
        public DataTable LoadSupplierSearchList()
        {
            string query = "SELECT sup_code as supCode, sup_name as supName FROM supplier WHERE sup_status =  1";
            return SqlAdapterQuery(query);
        }
    }
}
