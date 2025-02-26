using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class LogDto
    {
        public int logType {get; set;}
        public string OriginalValue { get; set; }
        public string EditValue { get; set; }
        public int Param { get; set; }
        public int EmployeeCode { get; set; }
    }
}
