using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class LogSearchDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int type { get; set; }
        public string SearchWord { get; set; }
    }
}
