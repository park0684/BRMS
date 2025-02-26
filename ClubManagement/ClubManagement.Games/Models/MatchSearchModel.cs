using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Models
{
    public class MatchSearchModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int MatchType { get; set; }
        public int MatchCode { get; set; }
        public bool ExcludeType { get; set; }
        public string SearchWord { get; set; }
        public bool IncludeSecessMember { get; set; }
    }
}
