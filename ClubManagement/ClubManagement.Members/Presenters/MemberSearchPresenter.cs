using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;

namespace ClubManagement.Members.Presenters
{
    public class MemberSearchPresenter
    {
        private IMemberSearchView _view;
        private IMemberRepository _repository;
        private MemberSearchModel _model;
        private readonly Action<int, string> _onMemberSelected;
        //private IMemberSelectCallback _callback;
        public MemberSearchPresenter(IMemberSearchView view, IMemberRepository repository, Action<int, string> onMemberSelected = null)
        {
            _view = view;
            _repository = repository;
            _onMemberSelected = onMemberSelected;

            _view.CloseFormEvent += CloseAction;
            _view.MemberSeachEvent += MemberSearch;
            _view.SelectMemberEvent += SelectMember;

            _model = new MemberSearchModel();
            MemberSearch(this, EventArgs.Empty);
        }

        private void SelectMember(object sender, EventArgs e)
        {
            int code = _view.MemberCode;
            string name = _view.MemberName;
            _onMemberSelected?.Invoke(code, name);
            _view.CloseForm();
        }

        private void MemberSearch(object sender, EventArgs e)
        {
            _model.SearchWord = _view.SearchWord;
            _model.IsInclude = _view.IsInculde;
            DataTable resutl = _repository.LoadMember(_model);
            _view.SetMemberList(resutl);
        }

        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
