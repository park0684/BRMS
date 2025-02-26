using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IMatchPlayerManageView
    {
        string SearchWord { get; set; }
        bool SecessMember { get; set; }

        event EventHandler SearchMemberEvent;
        event EventHandler PlayerUpdateEvent;
        event EventHandler<PlayerButtonEventArgs> PlayerAddEvent;
        event EventHandler<PlayerButtonEventArgs> PlayerRemoveEvent;
        event EventHandler SavePlayerListEvent;
        event EventHandler CloseEvent;
        event EventHandler AddGuestEvent;

        void UpdateButtonColor(int memberCode, bool isAdded);
        void SetMemberList(List<PlayerInfoDto> members);
        void SetPlayerList(List<PlayerInfoDto> players);
        void CloseForm();
        void ShowForm();
    }
}
