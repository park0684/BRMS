using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;
using ClubManagement.Games.DTOs;
namespace ClubManagement.Games.Presenters
{
    public class MatchPlayerManagePresenter
    {
        IMatchPlayerManageView _view;
        IMatchRepository _repository;
        MatchModel _model;
        MatchSearchModel _searchModel;

        public MatchPlayerManagePresenter(IMatchPlayerManageView view, IMatchRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new MatchModel();
            _searchModel = new MatchSearchModel();
            this._view.SearchMemberEvent += SearchMember;
            this._view.PlayerUpdateEvent += PlayerUpdate;
            this._view.AddGuestEvent += GuestAdd;
            this._view.PlayerAddEvent += PlayerAdd;
            this._view.PlayerRemoveEvent += PlayerRemove;
            this._view.SavePlayerListEvent += PlayerUpdate;
            this._view.CloseEvent += CloseForm;
            this._view.AddGuestEvent += GuestAdd;
        }

        private void GuestAdd(object sender, EventArgs e)
        {
            IGuestAddView view = new GuestAddView();
            IMatchRepository repository = new MatchRepository();
            GuestAddPresenter presenter = new GuestAddPresenter(view);
            view.ShowForm();
            _model.PlayerList.AddRange(presenter.getGuestList());
            _view.SetPlayerList(_model.PlayerList);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void PlayerRemove(object sender, PlayerButtonEventArgs e)
        {
            string memberName = e.MemberName;
            int memberCode = (int)e.MemberCode;

            var playerToRemove = _model.PlayerList.FirstOrDefault(p => p.MemberCode == memberCode && p.PalyerName == memberName);
            if (playerToRemove != null)
            {
                _model.PlayerList.Remove(playerToRemove);
                if (memberCode != 0)
                    _view.UpdateButtonColor(memberCode, false);
                _view.SetPlayerList(_model.PlayerList);
            }
        }

        private void PlayerAdd(object sender, PlayerButtonEventArgs e)
        {
            string memberName = e.MemberName;
            int memberCode = (int)e.MemberCode;
            if (_model.PlayerList.Any(p => p.MemberCode == memberCode))
            {
                return;
            }

            _model.PlayerList.Add(new PlayerInfoDto
            {
                MemberCode = memberCode,
                PalyerName = memberName
            });
            _view.UpdateButtonColor(memberCode, true);
            _view.SetPlayerList(_model.PlayerList);
        }
        public void PlayerManageCall(int gamecode)
        {
            _model.MatchCode = gamecode;
            LoadPlayer(gamecode);
            LoadMember();
        }

        private void PlayerUpdate(object sender, EventArgs e)
        {

            _repository.MatchPlayerUpdate(_model);
            _view.CloseForm();
        }

        private void SearchMember(object sender, EventArgs e)
        {
            _searchModel.SearchWord = _view.SearchWord;
            _searchModel.IncludeSecessMember = _view.SecessMember;
            LoadMember();
        }

        private void LoadMember()
        {
            DataTable resutl = _repository.LoadMember(_searchModel);
            List<PlayerInfoDto> memberList = resutl.AsEnumerable().Select(row => new PlayerInfoDto
            {
                MemberCode = row.Field<int>("mem_code"),
                PalyerName = row.Field<string>("mem_name"),
                IsSelected = _model.PlayerList.Any(p => p.MemberCode == row.Field<int>("mem_code"))
            }).ToList();
            _model.MemberList = memberList;
            _view.SetMemberList(_model.MemberList);
        }
        public void LoadPlayer(int code)
        {
            DataTable result = _repository.LoadPlayerList(code);
            List<PlayerInfoDto> playerList = result.AsEnumerable().Select(row => new PlayerInfoDto
            {
                MemberCode = row.Field<int>("att_memcode"),
                PalyerName = row.Field<string>("att_name"),
                IsSelected = true
            }).ToList();
            _model.PlayerList = playerList;
            _view.SetPlayerList(_model.PlayerList);
        }
    }
}
