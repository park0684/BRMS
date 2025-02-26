using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
namespace ClubManagement.Games.Models
{
    public class MatchModel
    {
        public int? MatchCode { get; set; }
        public string MatchTitle { get; set; }
        public string MatchHost { get; set; }
        public int? MatchType { get; set; }
        public DateTime? MatchDate { get; set; }
        public string MatchMemo { get; set; }

        public bool IsNew { get; set; }
        public List<PlayerInfoDto> PlayerList { get; set; }
        public List<PlayerInfoDto> MemberList { get; set; }
        public List<PlayerInfoDto> GuestList { get; set; }
    }
}
