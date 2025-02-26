using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Models
{
    public class MemberModel
    {
        public string MemberName { get; set; }
        public string Birth { get; set; }
        public string Memo { get; set; }
        public int? Code { get; set; }
        public int? Status { get; set; }
        public int? Position { get; set; }
        public int? Gender { get; set; }
        public DateTime? AccessDate { get; set; }
        public DateTime? SecessDate { get; set; }
        public bool IsNew { get; set; }
        public bool IsPro { get; set; }
    }
}
