using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIC.Repositories;
using System.Data;

namespace BRIC.Services
{
    public class LoginService
    {
        ILoginRepository _repository;

        public LoginService()
        {
            _repository = new LoginRepository();
        }
        public DataRow LoadEmployeeName(int code)
        {
            return _repository.LoadEmployee(code);
        }
        public List<int> LoadPermission(int code)
        {
            var result = _repository.LoadPermission(code);
            List<int> permissoin = new List<int>();

            foreach (DataRow row in result.Rows)
            {
                permissoin.Add(Convert.ToInt32(row["acper_permission"]));
            }

            return permissoin;
        }
    }
}
