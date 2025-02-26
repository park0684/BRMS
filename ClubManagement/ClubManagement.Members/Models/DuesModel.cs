using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Models
{
    public class DuesModel
    {
        public int? DuesCode { get; set; }
        public string SearchWord { get; set; }
        public int? MemberCode { get; set; }
        public bool SecessInclude { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
