using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhon { get; set; }
        public string EmployeeLevel { get; set; }
        public int EmployeeStatus { get; set; }
        public string EmployeePassword { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeAddress { get; set; }
        public string Employeememo { get; set; }
        public DateTime EmployeeIdate {get; set;}
        public DateTime EmployeeUdate { get; set; }

    }
}
