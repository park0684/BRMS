using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IGuestAddView
    {
        string GuestName { get; set; }
        List<PlayerInfoDto> GusetDate();

        event EventHandler AddGuestEvent;
        event EventHandler SaveGuestEvent;
        event EventHandler CloseFormEvevnt;

        void CloseForm();
        void AddGuestPanel();
        void TextBoxClear();
        void ShowForm();
    }
}
