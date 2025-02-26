using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class UserDto
    {
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePassword { get; set; }
        public List<int> PermissionList { get; set; } = new List<int>();
    }
}
