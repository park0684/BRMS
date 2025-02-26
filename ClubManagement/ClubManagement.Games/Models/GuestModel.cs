using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
namespace ClubManagement.Games.Models
{
    public class GuestModel
    {
        private string GuestName { get; set; }
        private int? Handycap { get; set; }
        public bool Gender { get; set; }
        public bool IsPro { get; set; }
        public List<PlayerInfoDto> GuestList { get; set; }
    }
}
