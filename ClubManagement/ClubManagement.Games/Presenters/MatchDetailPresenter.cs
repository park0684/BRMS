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
namespace ClubManagement.Games.Presenters
{
    public class MatchDetailPresenter
    {
        private IMatchDetailView _view;
        private IMatchRepository _repository;
        private MatchModel _model;

        public MatchDetailPresenter(IMatchDetailView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchModel();
            this._view.CloseEvenvt += CloseAction;
            this._view.SaveEvent += SaveGame;
            _model.IsNew = true;
        }

        public void LoadGame(int code)
        {
            DataRow row = _repository.LoadMatchInfo(code);

            _model.MatchCode = code;
            _model.IsNew = false;
            _model.MatchTitle = row["game_title"].ToString();
            _model.MatchHost = row["game_host"].ToString();
            _model.MatchMemo = row["game_memo"].ToString();
            _model.MatchDate = (DateTime)row["game_date"];
            _model.MatchType = (int)row["game_type"];

            _view.GameTitle = _model.MatchTitle;
            _view.GameHost = _model.MatchHost;
            _view.GameMemo = _model.MatchMemo;
            _view.GameDate = (DateTime)_model.MatchDate;
            _view.GameType = (int)_model.MatchType;
        }
        private void SaveGame(object sender, EventArgs e)
        {
            _model.MatchTitle = _view.GameTitle;
            _model.MatchHost = _view.GameHost;
            _model.MatchDate = _view.GameDate;
            _model.MatchType = _view.GameType;
            _model.MatchMemo = _view.GameMemo;

            if (_model.IsNew)
            {
                _repository.InsertMatch(_model);
            }
            else
            {
                _repository.UpdateMatch(_model);
            }
            _view.CloseForm();
        }

        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }


    }
}
