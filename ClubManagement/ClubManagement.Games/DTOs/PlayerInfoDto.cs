using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.DTOs
{
    public class PlayerInfoDto
    {
        public int MemberCode { get; set; }
        public string PalyerName { get; set; }
        public bool IsSelected { get; set; }
        public bool Gender { get; set; }
        public int Handycap { get; set; }
        public bool IsPro { get; set; }
    }
}
