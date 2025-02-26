using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Views;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Presenters
{
    class MatchListPresenter
    {
        IMatchListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;
        public MatchListPresenter(IMatchListView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
            //뷰이벤트 핸들러 설정
            this._view.EditMatchEvent += EditGame;
            this._view.AddMatchEvent += AddGame;
            this._view.SearchMatchEvent += SearchGame;
            this._view.SearchPlayerEvent += SearchPlayerList;
            this._view.EditPlayerEvent += EidtPlayerList;

            LoadGameList();
        }

        private void LoadGameList()
        {
            try
            {
                
                _model.FromDate = _view.MatchFromDate;
                _model.ToDate = _view.MatchToDate;
                _model.MatchType = _view.MatchType ?? 0;
                _model.ExcludeType = _view.ExcludeType;

                DataTable result = _repository.LoadMatchList(_model);
                result.Columns.Add("No");
                int i = 1;
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                }

                _view.SetGameListBinding(result);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        private void LoadPlayerList(int code)
        {
            try
            {
                DataTable players = _repository.LoadPlayerList(code);
                players.Columns.Add("No");
                players.Columns.Add("gender");
                players.Columns.Add("memberType");
                int i = 1;
                foreach (DataRow row in players.Rows)
                {
                    row["No"] = i++;
                    row["gender"] = MemberHelper.GetMemberGenger(Convert.ToInt32(row["mem_gender"]));
                    row["memberType"] = GameHelper.GetPlayerType(Convert.ToInt32(row["att_memtype"]));
                }

                players.Columns.Remove("mem_gender");
                players.Columns.Remove("att_memtype");
                
                _view.SetPlayerListBinding(players);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void SearchPlayerList(object sender, EventArgs e)
        {
            try
            {
                int code = _view.GetMatchCode ?? 0;
                if (code <= 0)
                {
                    _view.ShowMessage("조회된 모임이 없습니다.");
                    return;
                }
                LoadPlayerList(code);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void SearchGame(object sender, EventArgs e)
        {
            LoadGameList();
        }
        private void EidtPlayerList(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = new MatchRepository();
            MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);
            presenter.PlayerManageCall(code);
            view.ShowForm();
            LoadPlayerList(code);
        }

        private void AddGame(object sender, EventArgs e)
        {
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = new MatchRepository();
            new MatchPlayerManagePresenter(view, repository);
            view.ShowForm();
            LoadGameList();
        }

        private void EditGame(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IMatchDetailView view = new MatchDetailView();
            IMatchRepository repository = new MatchRepository();
            MatchDetailPresenter presenter = new MatchDetailPresenter(view, repository);
            presenter.LoadGame(code);
            view.ShowForm();
            LoadGameList();

        }
    }
}
