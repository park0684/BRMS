using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Models
{
    public class MemberSearchModel
    {
        public string SearchWord { get; set; }
        public int? Status { get; set; }
        public DateTime? AccFromDate { get; set; }
        public DateTime? AccToDate { get; set; }
        public DateTime? SecFromDate { get; set; }
        public DateTime? SecToDate { get; set; }
        public DateTime? GameFromDate { get; set; }
        public DateTime? GameTodate { get; set; }
        public bool ExcludeRegular { get; set; }
        public bool ExcludeIrregular { get; set; }
        public bool ExcludeEvent { get; set; }
        public bool ExcludeMember { get; set; }
        public bool AccessCheck { get; set; }
        public bool SecessCheck { get; set; }
        public bool GameCheck { get; set; }
        public bool IsInclude { get; set; }
    }
}
