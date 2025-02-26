using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class GuestAddPresenter
    {
        IGuestAddView _view;
        MatchModel _model;

        public GuestAddPresenter(IGuestAddView viewe)
        {
            _view = viewe;
            _model = new MatchModel();
            _view.AddGuestEvent += AddGuest;
            _view.SaveGuestEvent += SaveGuest;
            _view.CloseFormEvevnt += CloseForm;
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void SaveGuest(object sender, EventArgs e)
        {
            var guestList = _view.GusetDate();

            _model.GuestList = guestList.Select(g => new PlayerInfoDto
            {
                PalyerName = g.PalyerName,
                Gender = g.Gender,
                IsPro = g.IsPro,
                Handycap = g.Handycap,
                IsSelected = true,
                MemberCode = 0
            }).ToList();
            _view.CloseForm();
        }

        private void AddGuest(object sender, EventArgs e)
        {
            _view.AddGuestPanel();
            _view.GuestName = "";
        }

        public List<PlayerInfoDto> getGuestList()
        {
            return _model.GuestList;
        }
    }
}
