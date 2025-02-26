using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;

namespace ClubManagement.Members.Presenters
{
    public class StatementDetailPresenter 
    {
        private IStatementDetailView _view;
        private IDuesRepsotiry _repository;
        private StatementModel _model;

        public StatementDetailPresenter(IStatementDetailView view, IDuesRepsotiry repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new StatementModel();
            this._view.CloseEvent += CloseAction;
            this._view.SaveEvent += SatatementSave;
            this._view.SelectMemberEvent += MemberSelect;
            this._view.DeleteEvent += StatementDelete;
            this._view.TypeChaingedEvnet += TypeChainge;
            this._model.IsNew = true;
            this._view.TypeChaingedSet();
        }

        private void TypeChainge(object sender, EventArgs e)
        {
            _view.TypeChaingedSet();
            if (_view.IsWithdrawal)
            {
                _model.MemberCode = 0;
            }
        }

        public void OnMemberSelected(int memberCode, string memberName)
        {
            _model.MemberCode = memberCode;
            _model.MemberName = memberName;

            // 뷰에 멤버 정보를 업데이트
            _view.MemberName = memberName;
        }
        private void MemberSelect(object sender, EventArgs e)
        {

            IMemberSearchView view = new MemberSearchView();
            IMemberRepository repository = new MemberRepository();
            var searchPresenter = new MemberSearchPresenter(view, repository, OnMemberSelected);
            view.ShowView();

        }

        private void StatementDelete(object sender, EventArgs e)
        {
            int code = (int)_model.StatementCode;
            _repository.DeleteStatment(code);
            _view.CloseForm();
        }

        public void LoadStatement(int code)
        {
            _model.StatementCode = code;
            DataRow result = _repository.LoadStatmet(code);
            _model.IsNew = false;
            _model.MemberCode = Convert.ToInt32(result["du_memcode"]);
            _model.StatementDate = Convert.ToDateTime(result["du_date"]);
            _model.StatementAmount = Convert.ToInt32(result["du_pay"]);
            _model.DueCount = Convert.ToInt32(result["du_apply"]);
            _model.Memo = result["du_memo"].ToString().Trim();
            _model.StatementType = Convert.ToInt32(result["du_type"]);
            _model.StatementDetail = result["du_detail"].ToString().Trim();
            _model.IsWithdrawal = _model.StatementType == 1 || _model.StatementType == 3 ? false : true;

            _view.StatementDate = _model.StatementDate;
            _view.StatementType = _model.StatementType;

            if (_model.IsWithdrawal == true)
            {
                _view.DueAmount = 0;
                _view.Withdrawal = _model.StatementAmount;
                _view.WithdrawalDetail = _model.StatementDetail;
            }
            else
            {
                _view.DueAmount = _model.StatementAmount;
                _view.Withdrawal = 0;
                _view.MemberName = _model.StatementDetail;
            }
            _view.SetDeleteButtonVisivle();
        }

        public void SetModelDetail()
        {
            _model.StatementDate = _view.StatementDate;
            _model.IsWithdrawal = _view.IsWithdrawal;
            if (_model.IsWithdrawal)
            {
                _model.StatementAmount = _view.Withdrawal;
                _model.StatementDetail = _view.WithdrawalDetail;
            }
            else
            {
                _model.StatementAmount = _view.DueAmount;
                _model.StatementDetail = _view.MemberName;
            }
            _model.Apply = _view.Apply;
            _model.DueCount = _view.DueCount;
            _model.StatementType = _view.StatementType;
            _model.Memo = _view.Memo;
        }
        private void SatatementSave(object sender, EventArgs e)
        {
            SetModelDetail();
            if (_model.IsNew == true)
            {
                _repository.InsertStatment(_model);
            }
            else
            {
                _repository.UpdateStatment(_model);
            }
            _view.CloseForm();
        }


        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
