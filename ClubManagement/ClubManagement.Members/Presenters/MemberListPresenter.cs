using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Views;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Models;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Presenters
{
    public class MemberListPresenter
    {
        IMemberListView _view;
        IMemberRepository _repository;

        public MemberListPresenter(IMemberListView view, IMemberRepository repository)
        {
            _view = view;
            _repository = repository;
            _view.SearchEvent += LoadMemberList;
            _view.AddMemberEvent += AddMember;
            _view.EidtMemberEvent += LoadMemberInfo;
        }

        private void LoadMemberInfo(object sender, EventArgs e)
        {
            int memberCode = _view.SelectedCode.Value;
            IMemberDetailView view = new MemberDetailView();
            IMemberRepository repository = new MemberRepository();
            MemberDetailPresenter presenter = new MemberDetailPresenter(view, repository);
            presenter.GetMemberInfo(memberCode);
            view.ShowForm();
        }

        private void AddMember(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadMemberList(object sender, EventArgs e)
        {
            try
            {
                var model = new MemberSearchModel();
                model.Status = _view.Status;
                model.SearchWord = _view.SearchWord;
                model.AccessCheck = _view.AccessCheck;
                model.AccFromDate = _view.AccFromDate;
                model.AccToDate = _view.AccToDate;
                model.SecessCheck = _view.SecessCheck;
                model.SecFromDate = _view.SecFromDate;
                model.SecToDate = _view.SecToDate;
                model.GameCheck = _view.GameCheck;
                model.GameFromDate = _view.GameFromDate;
                model.GameTodate = _view.GameToDate;
                model.ExcludeMember = _view.ExcludeMember;
                model.ExcludeRegular = _view.ExcludeRegular;
                model.ExcludeIrregular = _view.ExcludeIrregular;
                model.ExcludeEvent = _view.ExcludeEvent;
                DataTable result = _repository.GetMemberList(model);
                //if (result == null || result.Rows.Count == 0)
                //    return;

                int i = 1;
                result.Columns.Add("No");
                result.Columns.Add("status");
                result.Columns.Add("position");
                result.Columns.Add("gender");
                result.Columns.Add("regularRate");
                result.Columns.Add("nonPayment");
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                    row["status"] = MemberHelper.GetMemberStatus(Convert.ToInt32(row["mem_status"]));
                    row["gender"] = MemberHelper.GetMemberGenger(Convert.ToInt32(row["mem_gender"]));
                    row["position"] = MemberHelper.GetMemberPositon(Convert.ToInt32(row["mem_position"]));
                    int nonPayment = Convert.ToInt32(row["mem_dues"]) - Convert.ToInt32(row["Payment"]);
                    row["nonPayment"] = nonPayment < 0 ? 0 : nonPayment;
                    decimal rate = DataConvertHelper.ConvertRate(row["game_count"], row["reglar_count"]);
                    row["regularRate"] = rate;
                }
                result.Columns.Remove("mem_status");
                result.Columns.Remove("mem_gender");
                result.Columns.Remove("mem_position");
                result.Columns.Remove("game_count");
                result.Columns.Remove("mem_dues");
                result.AcceptChanges();

                _view.MemberListBinding(result);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
    }
}
